# OnlineChess

## Architecture
Main concept
* Web App is based on the Blazor Server https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-5.0
* Clients communicate to each other via SignalR https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr-blazor?view=aspnetcore-5.0&tabs=visual-studio&pivots=server

Authentication
* Authentication and authorization in ASP.NET Core SignalR https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-5.0
* ASP.NET Core Identity https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-5.0&tabs=netcore-cli
  * To host the SQL Server on mac we need to use Docker https://code.likeagirl.io/setting-up-sql-server-on-macos-and-managing-migrations-through-dotnet-cli-ead1a9eeeb78
* Google https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-5.0
