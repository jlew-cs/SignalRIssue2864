SignalRIssue2864
================

Minimal project reproduction of https://github.com/SignalR/SignalR/issues/2864

To reproduce:

1. Load the default page (SimpleConnect.html)
2. See "connecting", "connected", "hello" status lines appear in the browser
3. Wait about 10 seconds.
4. Refresh the browser.
5. App will display "connecting" and hang on the /signalr/connect request, until the client times out (the server side timeout is set to 5 minutes currently in SignalRStartup.cs)
