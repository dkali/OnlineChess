using OnlineChess.Server.Hubs;
using System.Collections.Generic;
using System.Linq;

namespace OnlineChess.Data
{
    public class LobbyService
    {
        private LobbyHub _lobbyHub;
        private SQLiteDataService _sqliteDb;
        private Dictionary<string, HashSet<string>> PlayerMap = new Dictionary<string, HashSet<string>>();
        // PlayerMap <accountId, HashSet<connectionId>>
        private List<string> _playersInLobby = new List<string>(); // updated each time player has joined or left
        private Dictionary<string, string> AccountsLUT = new Dictionary<string, string>();
        // AccountsLUT <connectionId, accountId>

        public LobbyService(LobbyHub lobbyHub, SQLiteDataService sqliteDb)
        {
            _lobbyHub = lobbyHub;
            _sqliteDb = sqliteDb;
        }

        public bool AddPlayer(string accountId, string connectionId, string playerName)
        {
            _lobbyHub.lobbyService = this;

            if (PlayerMap.ContainsKey(accountId))
            {
                // same player has joined from a second device
                PlayerMap[accountId].Add(connectionId);
            }
            else
            {
                // player joind the first time
                PlayerMap.Add(accountId, new HashSet<string> { connectionId });
                _playersInLobby.Add(playerName);
            }

            AccountsLUT.Add(connectionId, accountId);

            // notify other players
            _lobbyHub.RefreshPlayerList(_playersInLobby);
            
            return true;
        }

        public void RemovePlayer(string connectionId)
        {
            string accountId = AccountsLUT[connectionId];
            AccountsLUT.Remove(connectionId);

            PlayerMap[accountId].Remove(connectionId);
            if (PlayerMap[accountId].Count() == 0){
                PlayerMap.Remove(accountId);
                _playersInLobby.Remove(_sqliteDb.GetPlayerName(accountId));
            }

            _lobbyHub.RefreshPlayerList(_playersInLobby);
        }
    }
}