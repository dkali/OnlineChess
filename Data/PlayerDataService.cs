using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChess.Data
{
    public enum PlayerState
    {
        None,
        InLobby,
        InGame
    }

    public class PlayerDataService
    {
        public string accountId;
        public int? sessionId = null;
        public PlayerState playerState { get; set; }

        public PlayerDataService()
        {
            playerState = PlayerState.None;
        }
    }
}