using ChessEngine.Moves;
using ChessEngine.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class ChessTurn
    {
        public bool IsTurnFinished
        {
            get
            {
                return this.TurnMoves.Any() && this.TurnMoves.Last().IsEndTurn;
            }
        }

        public int IndexPlayer
        {
            get;
            private set;
        }

        public List<ChessPieceMovesContainer> TurnMoves
        {
            get;
            private set;
        }

        public bool IsCurrentKingChecked
        {
            get;
            internal set;
        }

        public bool IsCurrentKingCheckMated
        {
            get;
            internal set;
        }

        public ChessTurn(int indexPlayer)
        {
            this.IndexPlayer = indexPlayer;

            this.TurnMoves = new List<ChessPieceMovesContainer>();

            this.IsCurrentKingChecked = false;
            this.IsCurrentKingCheckMated = false;
        }
    }
}
