# OnlineChess

## Architecture
* Web App is based on the Blazor Server https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-5.0
* Clients communicate to each other via SignalR https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr-blazor?view=aspnetcore-5.0&tabs=visual-studio&pivots=server
* Client Autentication is done using https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-5.0&tabs=visual-studio#scaffold-identity-into-a-blazor-server-project-without-existing-authorization
  * To host the SQL Server on mac we need to use Docker https://code.likeagirl.io/setting-up-sql-server-on-macos-and-managing-migrations-through-dotnet-cli-ead1a9eeeb78
