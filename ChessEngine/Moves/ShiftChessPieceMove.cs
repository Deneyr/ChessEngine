using ChessEngine.ChessModels.Monitors;
using ChessEngine.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Moves
{
    public class ShiftChessPieceMove: AChessPieceMove
    {
        public virtual ChessPiecePosition FromPosition
        {
            get;
            private set;
        }

        public virtual ChessPiecePosition ToPosition
        {
            get;
            private set;
        }

        public ShiftChessPieceMove(ChessPiece concernedChessPiece, ChessPiecePosition fromPosition, ChessPiecePosition toPosition):
            base(concernedChessPiece)
        {
            this.FromPosition = fromPosition;
            this.ToPosition = toPosition;
        }

        public override bool ApplyMove(ChessBoard chessBoard)
        {
            if(this.ConcernedChessPiece.ChessPiecePosition == this.FromPosition)
            {
                chessBoard.MoveChessPieceTo(this.ConcernedChessPiece, this.ToPosition);

                return base.ApplyMove(chessBoard);
            }
            return false;
        }

        public override bool ApplyReverseMove(ChessBoard chessBoard)
        {
            if (this.ConcernedChessPiece.ChessPiecePosition == this.ToPosition)
            {
                chessBoard.MoveChessPieceTo(this.ConcernedChessPiece, this.FromPosition);

                return base.ApplyReverseMove(chessBoard);
            }
            return false;
        }

        public override IChessMoveInfluence CreateInfluence()
        {
            return new ShiftChessMoveInfluence(this.ToPosition);
        }
    }
}
