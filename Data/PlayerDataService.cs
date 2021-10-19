using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChess.Data
{
    public class PlayerDataService
    {
        private PlayerData _currentPlayer;

        public PlayerDataService()
        {
            _currentPlayer = new PlayerData();
        }

        public string Name()
        {
            return _currentPlayer.Name;
        }

        public void Name(string newName)
        {
            _currentPlayer.Name = newName;
        }

        public PlayerState PlayerState()
        {
            return _currentPlayer.State;
        }

        public void PlayerState(PlayerState newState)
        {
            _currentPlayer.State = newState;
        }
    }
}