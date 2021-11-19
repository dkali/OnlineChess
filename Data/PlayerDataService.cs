using System.Collections.Generic;

namespace OnlineChess.Data
{
    public enum PlayerUiState
    {
        None,
        InLobby,
        InGame
    }

    public class PlayerDataService
    {
        public string AccountId;
        public PlayerUiState playerUiState { get; set; }
        public (int RowIndex, int CellIndex) SelectedCell;
        public bool FigureSelected { get; set; }
        //store the data how the cell should be highlighted
        public List<List<string>> StyleData = new List<List<string>>();

        public PlayerDataService()
        {
            playerUiState = PlayerUiState.None;
            FigureSelected = false;

            // init all cells to default style
            for (int rowIndex = 0; rowIndex < 10; rowIndex++)
            {
                List<string> row = new List<string>();
                for (int cellIndex = 0; cellIndex < 10; cellIndex++)
                {
                    row.Add("default");
                }
                StyleData.Add(row);
            }
        }

        public void SelectFigure(int row, int cell)
        {
            SelectedCell.RowIndex = row;
            SelectedCell.CellIndex = cell;
            StyleData[row][cell] = "selected-cell";
            FigureSelected = true;
        }

        public void ResetStyleData()
        {
            for (int rowIndex = 0; rowIndex < 10; rowIndex++)
            {
                for (int cellIndex = 0; cellIndex < 10; cellIndex++)
                {
                    StyleData[rowIndex][cellIndex] = "default";
                }
            }
        }
    }
}