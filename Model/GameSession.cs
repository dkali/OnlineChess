using System.Collections.Generic;

namespace EFChessData
{
    public class GameSession
    {
        public int GameSessionId { get; set; }
        public string WhitePlayerId { get; set; }
        public string BlackPlayerId { get; set; }

        public List<ObserverPlayer> Observers { get; set; } = new List<ObserverPlayer>();

        // public GameBoard board { get; set; }; // TODO
    }
}