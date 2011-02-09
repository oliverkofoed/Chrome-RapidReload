using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ChromeRapidReload {
	public partial class DirectorySelector : Form {
		private Monitor owner;
		public DirectorySelector(Monitor monitor) {
			InitializeComponent();
			this.owner = monitor;
			refreshDirectories();
		}

		private void refreshDirectories() {
			this.directories.Items.Clear();
			foreach(var dir in owner.MonitoredDirectories.Keys) {
				this.directories.Items.Add(dir);
			}
		}

		private void addButton_Click(object sender, EventArgs e) {
			var f = new FolderBrowserDialog();
			if(f.ShowDialog() == DialogResult.OK) {
				owner.MonitorDirectory(new DirectoryInfo(f.SelectedPath));
			}
			refreshDirectories();
		}

		private void removeButton_Click(object sender, EventArgs e) {
			directories.Items.Remove(directories.SelectedItem);
		}

		private void directories_SelectedIndexChanged(object sender, EventArgs e) {
			removeButton.Enabled = directories.SelectedIndex != -1;
		}

		private void DirectorySelector_FormClosing(object sender, FormClosingEventArgs e) {
			owner.SelectorWindowClosed();
		}
	}
}
