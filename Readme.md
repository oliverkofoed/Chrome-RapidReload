Chrome RapidReload
==================
Chrome RapidReload is a tiny Google Chrome extension that 
monitors directories for changes and automatically 
reloads the current browser window/tab
whenever files are changed.

Motivation, or, "Why would I ever want this?"
===========================================
When you've optimized everything else about your
development flow, it's time to look at the little things.

Chrome RapidReload optimizes one of those tiny key 
processes that web developers do a milion times every day. 

Why spend spend 3 keyboard shortcuts, when you can get
away with one?

Go from this:
1.	Ctrl-S (save current file)
2.	Alt-Tab (switch to browser)
3.	Ctrl-R (reload browser)
	
To this:
1.	Ctrl-S (save, and RapidReload will reload for you)
	
That's it. Chrome will automatically reload your
browser window, so your changes will already be loaded 
when you switch over to the browser window. 

Things get even better if you've got a dual-or-more-monitor setup. 
Just position the browser window on another
screen and you'll just have to glance to see your
changes after a save. 


Installation and usage
======================
Installation is quite simple:

1.	Install the Google Chrome extension by downloading the following file
	and *dragging it onto a Google Chrome window*

	[Download Chrome RapidReload Plugin](https://github.com/oliverkofoed/Chrome-RapidReload/raw/master/Download/Plugin.crx)
	
2.	Start "Chrome RapidReload Monitor.exe"
	(Add it to your startup items for extra bonus points)
	
	[Download Chrome RapidReload Monitor.exe](https://github.com/oliverkofoed/Chrome-RapidReload/raw/master/Download/Chrome%20RapidReload.exe)
	
Usage is also very simple:

1.	Click the Chrome RapidReload tray icon...

	![](https://github.com/oliverkofoed/Chrome-RapidReload/raw/master/screenshot-trayicon.png)

	... and select which directories to monitor in the dialog:
	
	![](https://github.com/oliverkofoed/Chrome-RapidReload/raw/master/screenshot-directoryselector.png)
   
2.	Click the Chrome RapidReload icon in Google Chrome
	to enable or disable the plugin in the browser.
	
	![](https://github.com/oliverkofoed/Chrome-RapidReload/raw/master/screenshot-browsericon.png)

The icon in Google Chrome will let you know when a
connection to the monitor app is established.

Tech Details
======================
The the only partly interesting thing about the code is the
very minimal WebSocket server in C# that is used to communicate
with the browser extension.

The server is running on port 1984 which is assumed
to be available on the host machine.

I've only tested the software on Windows 7.

Credits
=======
The icon used is by Umar Irshad (http://umarirshad.me/) and was found on designmoo.com:
	http://designmoo.com/resources/simple-icons-psd/