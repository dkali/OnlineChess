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
            await lobbyService.RemovePlayer(Context.ConnectionId);
        }

        public async Task RefreshPlayerListWith(List<string> players)
        {
            _players = players;
            await Clients.All.SendAsync("RefreshPlayerList", players);
        }

        public async Task RefreshPlayerList()
        {
            await Clients.All.SendAsync("RefreshPlayerList", _players);
        }

        public async Task ReRenderGameView(string groupName, string targetComponent)
        {
            await Clients.Group(groupName).SendAsync("ReRenderGameView", targetComponent);
        }

        public async Task KickPlayer(string playerId)
        {
            List<string> playerConnections = lobbyService.GetPlayerConnections(playerId);
            foreach (string connectionId in playerConnections)
            {
                await Clients.Client(connectionId).SendAsync("KickPlayer", playerId);
            }
        }

        public async Task NotifyPlayerLeft(string playerId, string groupName)
        {
            List<string> excludedConnections = lobbyService.GetPlayerConnections(playerId);
            await Clients.GroupExcept(groupName, excludedConnections).SendAsync("PlayerLeft", playerId);
        }

        public async Task InsertIntoGroup(string connectionId, string groupName)
        {
            await Groups.AddToGroupAsync(connectionId, groupName);
        }

        public async Task RemoveFromGroup(string connectionId, string groupName)
        {
            await Groups.RemoveFromGroupAsync(connectionId, groupName);
        }
    }
}