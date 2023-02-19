using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.ChessModels.Monitors;

namespace ChessEngine.Moves
{
    public class PromoteChessPieceMove: AChessPieceMove
    {
        public virtual ChessPieceType FromChessType
        {
            get;
            private set;
        }

        public virtual ChessPieceType ToChessType
        {
            get;
            private set;
        }

        public PromoteChessPieceMove(ChessPiece concernedChessPiece, ChessPieceType fromChessType, ChessPieceType toChessType):
            base(concernedChessPiece)
        {
            this.FromChessType = fromChessType;
            this.ToChessType = toChessType;
        }

        public override bool ApplyMove(ChessBoard chessBoard)
        {
            if (this.ConcernedChessPiece.ChessPieceType != this.ToChessType)
            {
                chessBoard.PromoteChessPiece(this.ConcernedChessPiece, this.ToChessType);

                return base.ApplyMove(chessBoard);
            }
            return false;
        }

        public override bool ApplyReverseMove(ChessBoard chessBoard)
        {
            if (this.ConcernedChessPiece.ChessPieceType != this.FromChessType)
            {
                chessBoard.PromoteChessPiece(this.ConcernedChessPiece, this.FromChessType);

                return base.ApplyReverseMove(chessBoard);
            }
            return false;
        }

        public override IChessMoveInfluence CreateInfluence()
        {
            return new PromoteChessMoveInfluence(this.ToChessType);
        }
    }
}
