using OnlineChess.Server.Hubs;
using System.Collections.Generic;
using System.Linq;

namespace OnlineChess.Data
{
    public class LobbyService
    {
        private LobbyHub _lobbyHub;
        private Dictionary<string, PlayerData> PlayerMap = new Dictionary<string, PlayerData>();

        public LobbyService(LobbyHub lobbyHub)
        {
            _lobbyHub = lobbyHub;
        }

        public bool NameAvailable(string newName)
        {
            IEnumerable<string> name = 
                from pdata in PlayerMap.Values.ToList()
                where pdata.Name == newName
                select pdata.Name;

            if (name.Count() != 0)
                return false;
            return true;
        }
        public bool AddPlayer(string connectionId, PlayerData newPlayer)
        {
            _lobbyHub.lobbyService = this;

            if (PlayerMap.ContainsKey(newPlayer.Name))
                return false;

            PlayerMap.Add(connectionId, newPlayer);

            IEnumerable<string> players = 
                from pdata in PlayerMap.Values.ToList()
                select pdata.Name;
            _lobbyHub.RefreshPlayerList(players.ToList());
            
            return true;
        }

        public void RemovePlayer(string connectionId)
        {
            PlayerMap.Remove(connectionId);
            IEnumerable<string> players = 
                from pdata in PlayerMap.Values.ToList()
                select pdata.Name;
            _lobbyHub.RefreshPlayerList(players.ToList());
        }
    }
}