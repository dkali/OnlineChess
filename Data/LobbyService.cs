using OnlineChess.Server.Hubs;
using System.Collections.Generic;
using System.Linq;
using System;

namespace OnlineChess.Data
{
    public class PlayerData
    {
        public HashSet<string> signalRConnections = new HashSet<string>();
        public string gameSession = "";
    }

    public class LobbyService
    {
        private LobbyHub _lobbyHub;
        private SQLiteDataService _sqliteDb;
        private Dictionary<string, PlayerData> _playerMap = new Dictionary<string, PlayerData>();
        // _playerMap <accountId, PlayerData>
        private List<string> _playersInLobby = new List<string>(); // updated each time player has joined or left
        private Dictionary<string, string> _accountsLUT = new Dictionary<string, string>();
        // _accountsLUT <connectionId, accountId>
        private Dictionary<string, GameSession> _gameSessions = new Dictionary<string, GameSession>();
        // _gameSessions <sessionId, GameSession> 

        public LobbyService(LobbyHub lobbyHub, SQLiteDataService sqliteDb)
        {
            _lobbyHub = lobbyHub;
            _sqliteDb = sqliteDb;
        }

        public bool AddPlayer(string accountId, string connectionId)
        {
            // bad code. needed to set a reference
            _lobbyHub.lobbyService = this;

            if (_playerMap.ContainsKey(accountId))
            {
                // same player has joined from a second device
                _playerMap[accountId].signalRConnections.Add(connectionId);
            }
            else
            {
                // player joind the first time
                PlayerData pData = new PlayerData();
                pData.signalRConnections.Add(connectionId);
                _playerMap.Add(accountId, pData);
                _playersInLobby.Add(accountId);
            }

            _accountsLUT.Add(connectionId, accountId);

            // notify other players
            _lobbyHub.RefreshPlayerList(_playersInLobby);
            
            return true;
        }

        public void RemovePlayer(string connectionId)
        {
            string accountId = _accountsLUT[connectionId];
            _accountsLUT.Remove(connectionId);

            _playerMap[accountId].signalRConnections.Remove(connectionId);
            if (_playerMap[accountId].signalRConnections.Count() == 0){
                _playerMap.Remove(accountId);
                _playersInLobby.Remove(accountId);
            }

            _lobbyHub.RefreshPlayerList(_playersInLobby);
        }

        public bool PlayerInGameSession(string accountId)
        {
            return !string.IsNullOrEmpty(_playerMap[accountId].gameSession);
        }

        public SessionState GetSessionState(string accountId)
        {
            return _gameSessions[_playerMap[accountId].gameSession].SessionState;
        }

        public void CreateGameSession(string accountId)
        {
            GameSession gSession = new GameSession();
            gSession.OwnerId = accountId;
            gSession.Players.Add(accountId);

            _gameSessions[gSession.SessionId] = gSession;
            _playerMap[accountId].gameSession = gSession.SessionId;
        }

        public void RefreshPLayerList()
        {
            _lobbyHub.RefreshPlayerList(_playersInLobby);
        }
    }
}