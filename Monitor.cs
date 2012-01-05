using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO.IsolatedStorage;

namespace ChromeRapidReload {
	public class Monitor : ApplicationContext {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Monitor());
		}

		private class Client{
			public TcpClient TcpClient;
			public NetworkStream Stream;
		}

		private DirectorySelector selector = null;
		private NotifyIcon trayIcon;
		private ContextMenu menu;
		private List<Client> clients = new List<Client>();
		private IsolatedStorageFile settingsStorage = IsolatedStorageFile.GetStore( IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
		public Dictionary<string, FileSystemWatcher> MonitoredDirectories = new Dictionary<string, FileSystemWatcher>();

		public Monitor() {
			// setup the context menu
			menu = new ContextMenu();
			menu.MenuItems.Clear();
			menu.MenuItems.Add(new MenuItem("Quit", delegate(Object sender, System.EventArgs e) {
				shutdown();
				Application.Exit();
			}));

			// listen for app closes
			Application.ApplicationExit += delegate(object sender, EventArgs e) {
				shutdown();
			};

			// setup the tray icon
			trayIcon = new NotifyIcon();
			trayIcon.Text = "Chrome RapidReload";
			trayIcon.Visible = true;
			trayIcon.Icon = new Icon(typeof(Monitor), "icon.ico");
			trayIcon.ContextMenu = menu;
			trayIcon.Click += delegate(object sender, EventArgs e) {
				if(selector == null) {
					selector = new DirectorySelector(this);
					selector.ShowDialog();
				} else {
					selector.Focus();
				}
			};

			// deserialized the monitored directories saved in settings file
			using(var reader = new StreamReader(new IsolatedStorageFileStream("settings", FileMode.OpenOrCreate,FileAccess.Read,FileShare.ReadWrite, settingsStorage))) {
				foreach(var dir in reader.ReadToEnd().Split('|')) {
					if(dir != "") {
						MonitorDirectory(new DirectoryInfo(dir));
					}
				}
			};

			// start a listener thread
			Thread t = new Thread(new ThreadStart(websocketListen));
			t.IsBackground = true;
			t.Start();
		}

		private void shutdown() {
			trayIcon.Visible = false;
			trayIcon.Dispose();
		}

		#region Monitoring Directories
		public void SelectorWindowClosed() {
			selector = null;
		}

		public void MonitorDirectory(DirectoryInfo directory) {
			try {
				var key = directory.FullName.ToLower();
				if(directory.Exists && !MonitoredDirectories.ContainsKey(key)) {

					// watch the dir
					var watcher = new FileSystemWatcher(directory.FullName);
					watcher.Filter = "";
					watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
					watcher.IncludeSubdirectories = true;
					watcher.Changed += sendReloadEvent;
					watcher.Renamed += sendReloadEvent;// renamed is required, because VS.NET writes to a temp file that it renames to actual file
					watcher.Deleted += sendReloadEvent;
					watcher.Created += sendReloadEvent;
					watcher.EnableRaisingEvents = true;

					MonitoredDirectories[key] = watcher;

					serializedMonitoredDirectories();
				}
			} catch { }  // disregard errors
		}

		public void StopMonitor(DirectoryInfo directory) {
			FileSystemWatcher watcher = null;
			if( MonitoredDirectories.TryGetValue(directory.FullName.ToLower(), out watcher) ){
				watcher.EnableRaisingEvents = false;
				MonitoredDirectories.Remove( directory.FullName.ToLower() );
			}
			serializedMonitoredDirectories();
		}

		private void serializedMonitoredDirectories() {
			using(var writer = new StreamWriter(new IsolatedStorageFileStream("settings", FileMode.Create, FileAccess.ReadWrite,FileShare.ReadWrite, settingsStorage))){
				var str = "";
				foreach(var dir in MonitoredDirectories.Keys) {
					str+= (str==""?"":"|") + dir;
				}
				writer.Write(str);
				writer.Close();
			}
		}
		#endregion

		#region WebSockets
		private void websocketListen() {
			var listener = new TcpListener(IPAddress.Loopback, 1984);
			listener.Start();
			while(true) {
				var client = new Client();
				client.TcpClient = listener.AcceptTcpClient();
				client.Stream = client.TcpClient.GetStream();

				// read the request
				Dictionary<string,string> headers = new Dictionary<string,string>();
				byte[] challengeResponse = new byte[16];
				var resource = "";
				var buffer = new byte[1024*10];
				int state = 0;
				int position = 0;
				int available = 0;
				while(state<4) {
					switch(state) {
						case 0: { // request line
							var line = readline(client.Stream, buffer, ref position, ref available);
							resource = line.Split(' ')[1];
							state = 1; // read headers
							break;}
						case 1:{ // headers
							var line = readline(client.Stream, buffer, ref position, ref available);
							if(line != "") {
								var index = line.IndexOf(":");
								headers[line.Substring(0, index).ToLower()] = line.Substring(index + 2);
							} else {
								state = 2;
							}
							break;}
						case 2: // body
							state = 4;

							// for old-style websocket connections
							if( !headers.ContainsKey("sec-websocket-key") ){
								if(available >= 8) {
									Array.Copy(buffer,position, challengeResponse, 8, 8);
									state = 4;
								} else { readmore(client.Stream, buffer, ref position, ref available); }
							}
							break;
					}
				}
				
				// Send response headers
				var writer = new StreamWriter(client.Stream, Encoding.UTF8);
				writer.WriteLine("HTTP/1.1 101 Switching Protocols");
				writer.WriteLine("Upgrade: websocket");
				writer.WriteLine("Connection: Upgrade");
				writer.WriteLine("Sec-WebSocket-Origin: "+headers["origin"]);
				writer.WriteLine("Sec-WebSocket-Location: ws://" + headers["host"] + resource);
				string subprotocol =null;
				if( headers.TryGetValue("sec-websocket-protocol", out subprotocol)){
					writer.WriteLine("Sec-WebSocket-Protocol: " + subprotocol);
				}

				// new style replies
				if( headers.ContainsKey("sec-websocket-key") ){
					var key = headers["sec-websocket-key"] + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
					var reply = Convert.ToBase64String(SHA1.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(key)));
					writer.WriteLine("Sec-WebSocket-Accept: " + reply);
					writer.WriteLine("");
					writer.Flush();
				}else{
					writer.WriteLine("");
					writer.Flush();
					// calculate and send the the challenge response
					addKeyBytes(challengeResponse, 0, headers["sec-websocket-key1"]);
					addKeyBytes(challengeResponse, 4, headers["sec-websocket-key2"]);
					var md5response = MD5.Create().ComputeHash(challengeResponse);
					client.Stream.Write(md5response, 0, md5response.Length);
				}

				// save clients
				lock(clients) {clients.Add(client);}
			}
		}

		private void addKeyBytes(byte[] challengeResponse, int position, string key) {
			var chars = key.ToCharArray();
			var keyNumber = long.Parse(new string(Array.FindAll(chars, c => (c >= '0' && c <= '9'))));
			var bytes = BitConverter.GetBytes((int)(keyNumber / Array.FindAll(chars, c => c == ' ').Length));
			if(BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}
			Array.Copy(bytes, 0, challengeResponse, position, 4);
		}

		private string readline(NetworkStream stream, byte[] buffer, ref int position, ref int available) {
			while(true) {
				var end = position + available;
				for(int i = position + 1; i < end; i++) {
					if(buffer[i - 1] == 0x0D && buffer[i] == 0x0A) {
						var str = Encoding.UTF8.GetString(buffer, position, i - position - 1);
						position = i + 1;
						available = end - position;
						return str;
					}
				}

				readmore(stream, buffer, ref position, ref available);
			}
		}

		private void readmore(NetworkStream stream, byte[] buffer, ref int position, ref int available) {
			// nothing found, move old bytes back
			for(int c = 0; c != available; c++) {
				buffer[c] = buffer[position + c];
			}
			position = 0;

			// read new bytes
			int read = stream.Read(buffer, available, buffer.Length - available);
			if(read != -1) {
				available += read;
			}
		}

		private void sendReloadEvent(object source, FileSystemEventArgs e) {
			try {
				lock(clients) {
					// remove dead connections
					clients.RemoveAll(c => !c.TcpClient.Connected);

					// send msg to all active
					//clients.ForEach(c => c.Stream.Write(new byte[] { 0x00, 0x42, 0xFF }, 0, 3)); // very minimal websocket message, old style
					clients.ForEach(c => c.Stream.Write(new byte[] { 0x81, 0x02, 0x68, 0x69 }, 0, 4)); // very minimal websocket message ("hi"), new framing
				}
			}catch{} // disregard errors
		}
		#endregion
	}
}