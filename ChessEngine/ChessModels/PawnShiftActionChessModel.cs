using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.ChessModels
{
    internal class PawnShiftActionChessModel : ShiftActionChessModel
    {
        public PawnShiftActionChessModel() : base(0, -1, 1)
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
    }
}
