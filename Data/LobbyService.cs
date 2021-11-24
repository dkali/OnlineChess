using OnlineChess.Server.Hubs;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace OnlineChess.Data
{
    public class PlayerData
    {
        public HashSet<string> SignalRConnections = new HashSet<string>();
        public string GameSessionId = "";
        public bool Online;
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
                // same player has joined from a second device, or refreshed the page
                _playerMap[accountId].SignalRConnections.Add(connectionId);
                _playerMap[accountId].Online = true;
                if (!_playersInLobby.Contains(accountId))
                {
                    _playersInLobby.Add(accountId);
                }
            }
            else
            {
                // player joind the first time
                PlayerData pData = new PlayerData();
                pData.Online = true;
                pData.SignalRConnections.Add(connectionId);
                _playerMap.Add(accountId, pData);
                _playersInLobby.Add(accountId);
            }

            _accountsLUT.Add(connectionId, accountId);

            // notify other players
            _lobbyHub.RefreshPlayerListWith(_playersInLobby);
            
            return true;
        }

        public void RemovePlayer(string connectionId)
        {
            string accountId = _accountsLUT[connectionId];
            _accountsLUT.Remove(connectionId);

            _playerMap[accountId].SignalRConnections.Remove(connectionId);
            if (_playerMap[accountId].SignalRConnections.Count() == 0){
                _playerMap[accountId].Online = false;
                _playersInLobby.Remove(accountId);
                _lobbyHub.RefreshPlayerListWith(_playersInLobby);
            }
        }

        public bool PlayerInGameSession(string accountId)
        {
            // player has reference to a game session,
            // and that session still alive
            return !string.IsNullOrEmpty(_playerMap[accountId].GameSessionId) &&
                _gameSessions.ContainsKey(_playerMap[accountId].GameSessionId);
        }

        public string GetSessionId(string accountId)
        {
            return _playerMap[accountId].GameSessionId;
        }

        public GameSession GetGameSession(string accountId)
        {
            return _gameSessions[_playerMap[accountId].GameSessionId];
        }

        public SessionState GetSessionState(string accountId)
        {
            return _gameSessions[_playerMap[accountId].GameSessionId].SessionState;
        }

        public void CreateGameSession(string accountId)
        {
            GameSession gameSession = new GameSession();
            gameSession.OwnerId = accountId;
            gameSession.Players.Add(accountId);

            _gameSessions[gameSession.SessionId] = gameSession;
            _playerMap[accountId].GameSessionId = gameSession.SessionId;

            // player might have opened Chess game from several devices, need to add all of the connections
            // associated with this user to a group
            foreach (string connectionId in _playerMap[accountId].SignalRConnections)
            {
                _lobbyHub.InsertIntoGroup(connectionId, gameSession.SessionId);
            }
        }

        public void JoinGameSession(string accountId, string hostId)
        {
            GameSession gameSession = GetGameSession(hostId);
            if (gameSession.Players.Contains(accountId))
            {
                // if click on Join button twice and fast, two cliks are registered, which lead to 
                // exception in the dropdown list later
                return;
            }
            
            gameSession.Players.Add(accountId);
            _playerMap[accountId].GameSessionId = gameSession.SessionId;
            _lobbyHub.ReRenderGameView(gameSession.SessionId, "StatsField");

            // player might have opened Chess game from several devices, need to add all of the connections
            // associated with this user to a group
            foreach (string connectionId in _playerMap[accountId].SignalRConnections)
            {
                _lobbyHub.InsertIntoGroup(connectionId, gameSession.SessionId);
            }
        }

        public async Task LeaveGame(string sessionId, string accountId)
        {
            GameSession gameSession = _gameSessions[sessionId];
            if (gameSession.Winner != string.Empty)
            {
                // game is over, safe to leave
                PlayerLeftGame(gameSession, accountId);
            }
            else if (gameSession.OwnerId == accountId)
            {
                // owner left, terminate game session
                await _lobbyHub.NotifyPlayerLeft(accountId, gameSession.SessionId);
                TerminateGameSession(gameSession);
            }
            else if (gameSession.OponentId == accountId)
            {
                // second player left the game
                switch (gameSession.SessionState)
                {
                    case SessionState.Preparation:
                        // TODO: notify Host
                        gameSession.OponentId = string.Empty;
                        PlayerLeftGame(gameSession, accountId);
                        break;

                    case SessionState.InGame:
                        // terminate game session ant notify others
                        await _lobbyHub.NotifyPlayerLeft(accountId, gameSession.SessionId);
                        TerminateGameSession(gameSession);
                        break;
                }
            }
            else
            {
                // observer left, never mind
                PlayerLeftGame(gameSession, accountId);
            }
            await _lobbyHub.RefreshPlayerList();
        }

        public void PlayerLeftGame(GameSession gameSession, string accountId)
        {
            gameSession.Players.Remove(accountId);
            _playerMap[accountId].GameSessionId = string.Empty;
            _lobbyHub.ReRenderGameView(gameSession.SessionId, "StatsField");

            foreach (string connectionId in _playerMap[accountId].SignalRConnections)
            {
                _lobbyHub.RemoveFromGroup(connectionId, gameSession.SessionId);
            }

            if (gameSession.Players.Count() == 0)
            {
                TerminateGameSession(gameSession);
            }
        }

        public void TerminateGameSession(GameSession gameSession)
        {
            foreach (string playerId in gameSession.Players)
                {
                    _playerMap[playerId].GameSessionId = string.Empty;
                    _lobbyHub.KickPlayer(playerId);

                    foreach (string connectionId in _playerMap[playerId].SignalRConnections)
                    {
                        _lobbyHub.RemoveFromGroup(connectionId, gameSession.SessionId);
                    }
                }
                _gameSessions.Remove(gameSession.SessionId);
        }

        public void RefreshPLayerList()
        {
            _lobbyHub.RefreshPlayerListWith(_playersInLobby);
        }

        public FieldData GetSessionField(string sessionId)
        {
            return _gameSessions[sessionId].Field;
        }

        public void StartGame(string sessionId)
        {
            _gameSessions[sessionId].SessionState = SessionState.InGame;
            _lobbyHub.ReRenderGameView(sessionId, "StatsField");

            _gameSessions[sessionId].Field.InitFigures();
            _lobbyHub.ReRenderGameView(sessionId, "GameField");
        }

        public void ReRender(string groupName, string target)
        {
            _lobbyHub.ReRenderGameView(groupName, target);
        }

        public List<string> GetPlayerConnections(string accountId)
        {
            List<string> connections = new List<string>();
            foreach (string connection in _playerMap[accountId].SignalRConnections)
            {
                connections.Add(connection);
            }
            return connections;
        }
    }
}