using ChessEngine.ChessModels.Monitors;
using ChessEngine.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Moves
{
    public class KillChessPieceMove : AChessPieceMove
    {
        public virtual ChessPiecePosition FromPosition
        {
            get;
            private set;
        }

        public KillChessPieceMove(ChessPiece concernedChessPiece, ChessPiecePosition fromPosition) 
            : base(concernedChessPiece)
        {
            this.FromPosition = fromPosition;
        }

        public override bool ApplyMove(ChessBoard chessBoard)
        {
            if (this.ConcernedChessPiece.ChessPiecePosition == this.FromPosition)
            {
                chessBoard.KillChessPiece(this.ConcernedChessPiece);

                return base.ApplyMove(chessBoard);
            }
            return false;
        }

        public override bool ApplyReverseMove(ChessBoard chessBoard)
        {
            if (this.ConcernedChessPiece.IsAlive == false)
            {
                chessBoard.ResurrectChessPiece(this.ConcernedChessPiece, this.FromPosition);

                return base.ApplyReverseMove(chessBoard);
            }
            return false;
        }

        public override IChessMoveInfluence CreateInfluence()
        {
            return null;
        }
    }
}
