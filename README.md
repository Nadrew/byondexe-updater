# BYONDexe updater
**By Nadrew**
Version 1.0

I created this little script because I got tired of having to navigate through the BYOND builds directory to find the latest BYONDexe download, since there's no link on the download page.
Eventually it evolved a bit into my main method of getting BYONDexe outright.

**What It Does**

It's pretty simple, if the program detects an existing BYONDexe install in the same directory as it, it'll grab the version from one of the DLL files and check that against the latest version on the BYOND website.

If the local copy if outdated, it'll download the new version and extract it over top of the existing one, skipping any existing files that aren't part of the core installation files.

This way it will ignore anything the user has modified like byondexe.ini and hub.css/hub.html and the sort.

If it doesn't detect an existing BYONDexe install, it'll do all of that anyways and by nature of how it works, you'll get a fresh BYONDexe install in the directory you run it in.

**How To Use It**

All you have to do is put byondexe-updater.exe in the same directory as byondexe.exe, it'll check for byondcore.dll in setup/bin of that directory and go from there.

You can pass the "--verbose" argument to the program and it'll output more details as it does things, if you're curious about what's going on, or it's failing for some reason.

I'm not really a C# programmer, and this is my first time using it in at least 15 years, so feedback and help are much appreciated, I'm sure there's better ways of doing a ton of this stuff.

**Thanks Nadrew!**

If you find this thing useful, and want to show your appreciation, I have a [Ko-Fi](https://ko-fi.com/nadrew) setup.
