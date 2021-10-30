# OnlineChess

## Architecture
* Main concept
  * Web App is based on the Blazor Server https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-5.0
  * Clients communicate to each other via SignalR https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr-blazor?view=aspnetcore-5.0&tabs=visual-studio&pivots=server
* Google web server auth 
  * Google Cloud console https://console.cloud.google.com/apis/credentials?project=onlinechess-329912
  * OAuth 2.0 https://developers.google.com/identity/protocols/oauth2/web-server 
  * people API https://developers.google.com/people/api/rest/v1/people/get
* Cookies 
  * LocalStorage https://www.thomasclaudiushuber.com/2021/04/19/store-data-of-your-blazor-app-in-the-local-storage-and-in-the-session-storage/ 


## Development settings
For development phase I have mapped
192.168.0.177   myOnlineChess.com
in private/etc/hosts, since google API do not redirect to private IP