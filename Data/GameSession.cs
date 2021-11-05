using System.Collections.Generic;
using System;

namespace OnlineChess.Data
{
    public enum SessionState
    {
        Preparation,
        InGame
    }

    public class GameSession
    {
        public string SessionId { get; set; }
        public string OwnerId { get; set; }
        public List<string> Players { get; set; } // account IDs
        public SessionState SessionState { get; set; }
        public FieldData Field { get; set; }

        public GameSession()
        {
            Players = new List<string>();
            SessionId = Guid.NewGuid().ToString();
            SessionState = SessionState.Preparation;
            Field = new FieldData();
        }
    }
}