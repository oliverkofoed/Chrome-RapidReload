Chrome RapidReload
==================
Chrome RapidReload is a small chrome extensions that 
monitors one or more directories on your machine and
automatically reloads the current browser window/tab
whenever a changes occur in those directories.

Motivation, or, Why would I ever want this?
===========================================
When you've optimized everything else about your
development flow, it's time to look at the little things.

Chrome RapidReload optimizes one of those tiny key 
processes web developers do a milion times a day. 

Why spend spend 3 keyboard shortcuts on viewing a
change in your browser when you can spend just one? 

Go from this:
1.	Ctrl-S
2.	Alt-Tab
3.	Ctrl-R
	
To this:
1.	Ctrl-S
	
That's it. Chrome will automatically reload your
browser window and show up the updated page.

Your changes will already be waiting for you when
you switch over to the browser window. 

If you've got a dual-or-more-monitor setup, it's even 
better. Just position the browser window on another
screen and you'll just have to glance to see your
changes after a save. 


Installation and usage
======================
Installation is very, very simple:

1.	Install the Google Chrome extension from 

	[Install Chrome RapidReload](https://github.com/oliverkofoed/Chrome-RapidReload/raw/master/Download/Plugin.crx)
	
2.	Start "Chrome RapidReload Monitor.exe"
	Add it to your startup items for extra bonus points
	
	[Download Chrome RapidReload Monitor.exe](https://github.com/oliverkofoed/Chrome-RapidReload/raw/master/Download/Chrome%20RapidReload.exe)
	
Usage is also very simple

1.	Click the new Chrome RapidReload tray icon:

	![](https://github.com/oliverkofoed/Chrome-RapidReload/raw/master/screenshot-trayicon.png)

	and select which directories to monitor in the dialog: 
	
	![](https://github.com/oliverkofoed/Chrome-RapidReload/raw/master/screenshot-directoryselector.png)
   
2.	Click the Chrome RapidReload icon in Google Chrome
	to enable or disable the plugin in the browser.
	
	![](https://github.com/oliverkofoed/Chrome-RapidReload/raw/master/screenshot-browsericon.png)

The icon in Google Chrome will let you know when a
connection to the monitor app is established.

Tech Details
======================
The most interresting thing about the code is that
the monitor app implements a very minimal WebSocket
server in C# for communicating with the Google Chrome
browser extension

The server is running on port 1984 which is assumed
to be available on the host machine.

I've only tested the software on Windows 7.


Credits
=======
The icon used is by Umar Irshad (umarirshad.com) via designmoo.com:
http://designmoo.com/resources/simple-icons-psd/