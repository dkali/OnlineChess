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
        public string WhitePlayer { get; set; }
        public string BlackPlayer { get; set; }
        private bool WhiteTurn { get; set; }

        public GameSession()
        {
            Players = new List<string>();
            SessionId = Guid.NewGuid().ToString();
            SessionState = SessionState.Preparation;
            Field = new FieldData();
            WhiteTurn = true;
        }

        public bool IsItMyTurn(string accountId)
        {
            if (WhiteTurn)
                return WhitePlayer == accountId;
            else
                return BlackPlayer == accountId;
        }

        public void MoveFigure((int fromRowIndex, int fromCellIndex) selectedCell, int toRowIndex, int toCellIndex)
        {
            Field.GameField[toRowIndex][toCellIndex].Value = Field.GameField[selectedCell.fromRowIndex][selectedCell.fromCellIndex].Value;
            Field.GameField[selectedCell.fromRowIndex][selectedCell.fromCellIndex].Value = string.Empty;
            WhiteTurn = !WhiteTurn;
        }
    }
}