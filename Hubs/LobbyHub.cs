using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using OnlineChess.Data;
using Microsoft.Extensions.Logging;

namespace OnlineChess.Server.Hubs
{
    public class LobbyHub : Hub
    {
        private List<string> _players;
        public LobbyService lobbyService;
        private ILogger<LobbyHub> _logger;
        public LobbyHub(ILogger<LobbyHub> logger)
        {
            _logger = logger;
        }

        // public async Task SendMessage(string user, string message)
        // {
        //     await Clients.All.SendAsync("ReceiveMessage", user, message);
        // }

        // // The user identifier for a connection can be accessed by the Context.UserIdentifier property in the hub.
        // public Task SendPrivateMessage(string user, string message)
        // {
        //     return Clients.User(user).SendAsync("ReceiveMessage", message);
        // }

        // public async Task NewPlayerJoined(string playerName, string connectionId)
        // {
        //     // string userId = Context.UserIdentifier;
        //     await Clients.Others.SendAsync("NewClientJoined", playerName, connectionId);
        // }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"[SignalR] User connected {Context.ConnectionId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"[SignalR] User disconnected {Context.ConnectionId}");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
            lobbyService.RemovePlayer(Context.ConnectionId);
        }

        public async Task RefreshPlayerList(List<string> players)
        {
            _players = players;
            await Clients.All.SendAsync("RefreshPlayerList", players);
        }

        // TODO add SignalR group as parameter
        public async Task ReRenderGameView(string targetComponent)
        {
            await Clients.All.SendAsync("ReRenderGameView", targetComponent);
        }
    }
}