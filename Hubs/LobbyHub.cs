using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace OnlineChess.Server.Hubs
{
    public class LobbyHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        // The user identifier for a connection can be accessed by the Context.UserIdentifier property in the hub.
        public Task SendPrivateMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }

        public async Task NewPlayerJoined(string playerName)
        {
            string userId = Context.UserIdentifier;
            await Clients.Others.SendAsync("NewClientJoined", playerName);
        }
    }
}