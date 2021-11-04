
using System.Collections.Generic;

namespace OnlineChess.Data
{
    public class FieldData
    {
        public List<List<FieldCell>> GameField = new List<List<FieldCell>>();
        public FieldData()
        {
            // initialize empty field
            for (int rowIndex = 0; rowIndex < 10; rowIndex++)
            {
                List<FieldCell> fRow = new List<FieldCell>();
                for (int cellIndex = 0; cellIndex < 10; cellIndex++)
                {
                    FieldCell fCell = new FieldCell(" ");
                    fRow.Add(fCell);
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
    }
}