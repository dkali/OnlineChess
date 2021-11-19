
using System.Collections.Generic;

namespace OnlineChess.Data
{
    public class FieldData
    {
        public List<List<CellData>> GameField = new List<List<CellData>>();
        public FieldData()
        {
            bool StartFromWhite = true;
            // initialize empty field
            for (int rowIndex = 0; rowIndex < 10; rowIndex++)
            {
                List<CellData> fRow = new List<CellData>();
                for (int cellIndex = 0; cellIndex < 10; cellIndex++)
                {
                    CellData fCell;
                    if (cellIndex != 0 && cellIndex != 9 &&
                        rowIndex != 0 && rowIndex != 9)
                    {
                        if (StartFromWhite)
                        {
                            fCell = new CellData("white-cell");
                        }
                        else
                        {
                            fCell = new CellData("black-cell");
                        }
                        StartFromWhite = !StartFromWhite;
                    }
                    else
                    {
                        fCell = new CellData("border-cell");
                    }

                    fRow.Add(fCell);
                }
                if (rowIndex != 0)
                {
                    StartFromWhite = !StartFromWhite;
                }
                GameField.Add(fRow);
            }

            for (int rowIndex = 1; rowIndex < 9; rowIndex++){
                GameField[rowIndex][0].Value = (9 - rowIndex).ToString();
                GameField[rowIndex][9].Value = (9 - rowIndex).ToString();
            }

            char cur = 'A';
            for (int cellIndex = 1;  cellIndex < 9; cellIndex++)
            {
                GameField[0][cellIndex].Value = ((char)(cur + cellIndex - 1)).ToString();
                GameField[9][cellIndex].Value = ((char)(cur + cellIndex - 1)).ToString();
            }
        }

        public void InitFigures()
        {
            // white figures
            SetFigure("A1", Figures.WhiteChessRook);
            SetFigure("B1", Figures.WhiteChessKnight);
            SetFigure("C1", Figures.WhiteChessBishop);
            SetFigure("D1", Figures.WhiteChessQueen);
            SetFigure("E1", Figures.WhiteChessKing);
            SetFigure("F1", Figures.WhiteChessBishop);
            SetFigure("G1", Figures.WhiteChessKnight);
            SetFigure("H1", Figures.WhiteChessRook);
            SetFigure("A2", Figures.WhiteChessPawn);
            SetFigure("B2", Figures.WhiteChessPawn);
            SetFigure("C2", Figures.WhiteChessPawn);
            SetFigure("D2", Figures.WhiteChessPawn);
            SetFigure("E2", Figures.WhiteChessPawn);
            SetFigure("F2", Figures.WhiteChessPawn);
            SetFigure("G2", Figures.WhiteChessPawn);
            SetFigure("H2", Figures.WhiteChessPawn);

            // black figures
            SetFigure("A8", Figures.BlackChessRook);
            SetFigure("B8", Figures.BlackChessKnight);
            SetFigure("C8", Figures.BlackChessBishop);
            SetFigure("D8", Figures.BlackChessQueen);
            SetFigure("E8", Figures.BlackChessKing);
            SetFigure("F8", Figures.BlackChessBishop);
            SetFigure("G8", Figures.BlackChessKnight);
            SetFigure("H8", Figures.BlackChessRook);
            SetFigure("A7", Figures.BlackChessPawn);
            SetFigure("B7", Figures.BlackChessPawn);
            SetFigure("C7", Figures.BlackChessPawn);
            SetFigure("D7", Figures.BlackChessPawn);
            SetFigure("E7", Figures.BlackChessPawn);
            SetFigure("F7", Figures.BlackChessPawn);
            SetFigure("G7", Figures.BlackChessPawn);
            SetFigure("H7", Figures.BlackChessPawn);
        }

        public void SetFigure(string cell, Figures figureCode)
        {
            int rowInd = 9 - (cell[1] - '0');
            int cellInd = cell[0] - '@';

            GameField[rowInd][cellInd].Value = ChessFigures.Codes[(int)figureCode];
        }
    }
}