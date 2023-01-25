using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Moves
{
    public abstract class AChessPieceMove : IChessPieceMove
    {
        public ChessPiece ConcernedChessPiece
        {
            get;
            private set;
        }


        public bool IsChessPieceFirstMove
        {
            get;
            private set;
        }

        public AChessPieceMove(ChessPiece concernedChessPiece)
        {
            this.ConcernedChessPiece = concernedChessPiece;

            this.IsChessPieceFirstMove = concernedChessPiece.HasAlreadyMoved == false;
        }

        public virtual bool ApplyMove(ChessBoard chessBoard)
        {
            if (this.IsChessPieceFirstMove)
            {
                this.ConcernedChessPiece.HasAlreadyMoved = true;
            }

            return true;
        }

        public virtual bool ApplyReverseMove(ChessBoard chessBoard)
        {
            if (this.IsChessPieceFirstMove)
            {
                this.ConcernedChessPiece.HasAlreadyMoved = false;
            }

            return true;
        }
    }
}
