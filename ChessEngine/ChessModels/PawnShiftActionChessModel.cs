using ChessEngine.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.ChessModels
{
    internal class PawnShiftActionChessModel : ShiftActionChessModel
    {
        public PawnShiftActionChessModel() 
            : base(1, 1, 1)
        {
        }

        protected override int GetNumberCellrange(ChessBoard chessBoard, ChessPiece concernedChessPiece)
        {
            int result = base.GetNumberCellrange(chessBoard, concernedChessPiece);
            if (concernedChessPiece.HasAlreadyMoved == false)
            {
                result += 1;
            }

            return result;
        }

        protected override bool InternalIsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            ChessPiece chessPiece = chessBoard.GetChessPieceAt(toPosition);

            return chessPiece == null;
        }

        protected override int GetXShift(ChessPiece concernedChessPiece)
        {
            return this.xShift * concernedChessPiece.Owner.XDirection;
        }

        protected override int GetYShift(ChessPiece concernedChessPiece)
        {
            return this.yShift * concernedChessPiece.Owner.YDirection;
        }

        protected override bool IsEndTurnMove(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            return PromoteActionChessModel.IsInPromoteArea(chessBoard, concernedChessPiece, toPosition) == false;
        }
    }
}
