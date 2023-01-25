using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Maths;
using ChessEngine.Moves;

namespace ChessEngine.ChessModels.Monitors
{
    public class ShiftChessMoveInfluence : IChessMoveInfluence
    {
        public ChessPiecePosition ToPosition
        {
            get;
            private set;
        }

        public ShiftChessMoveInfluence(ChessPiecePosition toPosition)
        {
            this.ToPosition = toPosition;
        }

        public bool CreateMove(ChessBoard chessBoard, ChessPiece concernedChessPiece, out ChessPieceMovesContainer chessPieceMove)
        {
            return concernedChessPiece.ActionModelMonitor.CreateShiftMoveFrom(chessBoard, concernedChessPiece, this.ToPosition, out chessPieceMove);
        }

        public bool IsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece)
        {
            return concernedChessPiece.ActionModelMonitor.IsShiftMoveAllowed(chessBoard, concernedChessPiece, this.ToPosition);
        }
    }
}
