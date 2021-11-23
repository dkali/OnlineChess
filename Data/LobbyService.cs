using OnlineChess.Server.Hubs;
using System.Collections.Generic;
using System.Linq;
using System;

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
            GameSession gSession = new GameSession();
            gSession.OwnerId = accountId;
            gSession.Players.Add(accountId);

            _gameSessions[gSession.SessionId] = gSession;
            _playerMap[accountId].GameSessionId = gSession.SessionId;
        }

        public void JoinGameSession(string accountId, string hostId)
        {
            GameSession gSession = GetGameSession(hostId);
            gSession.Players.Add(accountId);
            _playerMap[accountId].GameSessionId = gSession.SessionId;
            _lobbyHub.ReRenderGameView("StatsField");
        }

        public void LeaveGame(string sessionId, string accountId)
        {
            GameSession gameSession = _gameSessions[sessionId];
            if (gameSession.Winner != string.Empty)
            {
                // game is over, safe to leave
                if (gameSession.OwnerId == accountId) // TODO, add case for second player
                {
                    // owner left, terminate game session
                    TerminateGameSession(gameSession);
                    _lobbyHub.PlayerLeft(accountId);
                }
                else
                {
                    PlayerLeftGame(gameSession, accountId);
                }
            }
            else if (gameSession.OwnerId == accountId) // TODO, add case for second player
            {
                // owner left, terminate game session
                TerminateGameSession(gameSession);
                _lobbyHub.PlayerLeft(accountId);
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
                        TerminateGameSession(gameSession);
                        _lobbyHub.PlayerLeft(accountId);
                        break;
                }
            }
            else
            {
                // observer left, never mind
                PlayerLeftGame(gameSession, accountId);
            }
            _lobbyHub.RefreshPlayerList();
        }

        public void PlayerLeftGame(GameSession gameSession, string accountId)
        {
            gameSession.Players.Remove(accountId);
            _playerMap[accountId].GameSessionId = string.Empty;
            _lobbyHub.ReRenderGameView("StatsField");
        }

        public void TerminateGameSession(GameSession gameSession)
        {
            foreach (string playerId in gameSession.Players)
                {
                    _playerMap[playerId].GameSessionId = string.Empty;
                    _lobbyHub.KickPlayer(playerId);
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
            _lobbyHub.ReRenderGameView("StatsField");
            _gameSessions[sessionId].Field.InitFigures();
            _lobbyHub.ReRenderGameView("GameField");
        }

        public void ReRender(string target)
        {
            _lobbyHub.ReRenderGameView(target);
        }
    }
}