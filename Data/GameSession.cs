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
        public string OponentId { get; set; }
        public bool WhiteTurn { get; set; }
        public string EatenWhites { get; set; }
        public string EatenBlacks { get; set; }
        public string Winner { get; set; }

        public GameSession()
        {
            Players = new List<string>();
            SessionId = Guid.NewGuid().ToString();
            SessionState = SessionState.Preparation;
            Field = new FieldData();
            WhiteTurn = true;
            EatenBlacks = "";
            EatenWhites = "";
            Winner = string.Empty;
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
            if (Winner != string.Empty)
                return;

            if (Field.GameField[toRowIndex][toCellIndex].Value != string.Empty)
            {
                if (ChessFigures.WhiteFigures.Contains(Field.GameField[toRowIndex][toCellIndex].Value))
                {
                    EatenWhites += Field.GameField[toRowIndex][toCellIndex].Value;
                    if ((Field.GameField[toRowIndex][toCellIndex].Value == ChessFigures.Codes[(int)Figures.WhiteChessKing]) && 
                        WhiteTurn == false)
                    {
                        Winner = BlackPlayer;
                    }
                }
                if (ChessFigures.BlackFigures.Contains(Field.GameField[toRowIndex][toCellIndex].Value))
                {
                    EatenBlacks += Field.GameField[toRowIndex][toCellIndex].Value;
                    if ((Field.GameField[toRowIndex][toCellIndex].Value == ChessFigures.Codes[(int)Figures.BlackChessKing]) && 
                        WhiteTurn == true)
                    {
                        Winner = WhitePlayer;
                    }
                }
            }

            Field.GameField[toRowIndex][toCellIndex].Value = Field.GameField[selectedCell.fromRowIndex][selectedCell.fromCellIndex].Value;
            Field.GameField[selectedCell.fromRowIndex][selectedCell.fromCellIndex].Value = string.Empty;
            WhiteTurn = !WhiteTurn;
        }
    }
}