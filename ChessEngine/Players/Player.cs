using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Players
{
    /// <summary>
    /// Test player
    /// </summary>
    public class Player: APlayer
    {
        public string PlayerName
        {
            get;
            private set;
        }

        public Player(string playerName, int xDirection, int yDirection)
            : base(xDirection, yDirection)
        {
            this.PlayerName = playerName;
        }
    }
}
