using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChess.Data
{
    public enum PlayerState
    {
        Ready,
        InGame
    }

    public class PlayerData
    {
        public string Name { get; set; }
        public PlayerState State { get; set; }

        public PlayerData(){}
    }
}