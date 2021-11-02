using System;
using System.Linq;
using System.Threading.Tasks;

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
        public string accountId;
        public PlayerUiState playerUiState { get; set; }

        public PlayerDataService()
        {
            playerUiState = PlayerUiState.None;
        }
    }
}