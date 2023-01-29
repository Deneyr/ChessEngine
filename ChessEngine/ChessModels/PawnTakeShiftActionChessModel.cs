using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Maths;

namespace ChessEngine.ChessModels
{
    internal class PawnTakeShiftActionChessModel : ShiftActionChessModel
    {
        public PawnTakeShiftActionChessModel(int xShift) 
            : base(xShift, 1, 1)
        {
        }

        protected override bool InternalIsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            ChessPiece chessPiece = chessBoard.GetChessPieceAt(toPosition);

            return chessPiece != null
                && chessPiece.Owner != concernedChessPiece.Owner;
        }

        protected override int GetXShift(ChessPiece concernedChessPiece)
        {
            MathHelper.ChangeBaseVector(this.xShift, this.yShift, concernedChessPiece.Owner.XDirection, concernedChessPiece.Owner.YDirection, out int xGlobalShift, out int yGlobalShift);

            return xGlobalShift;
        }

        protected override int GetYShift(ChessPiece concernedChessPiece)
        {
            MathHelper.ChangeBaseVector(this.xShift, this.yShift, concernedChessPiece.Owner.XDirection, concernedChessPiece.Owner.YDirection, out int xGlobalShift, out int yGlobalShift);

            return yGlobalShift;
        }
    }
}
