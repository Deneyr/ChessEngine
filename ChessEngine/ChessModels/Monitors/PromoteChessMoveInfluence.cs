using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Moves;

namespace ChessEngine.ChessModels.Monitors
{
    public class PromoteChessMoveInfluence : IChessMoveInfluence
    {
        public ChessPieceType ToType
        {
            get;
            private set;
        }

        public PromoteChessMoveInfluence(ChessPieceType toType)
        {
            this.ToType = toType;
        }

        public bool CreateMove(ChessBoard chessBoard, ChessPiece concernedChessPiece, out ChessPieceMovesContainer chessPieceMove)
        {
            return concernedChessPiece.ActionModelMonitor.CreatePromoteMoveFrom(chessBoard, concernedChessPiece, this.ToType, out chessPieceMove);
        }

        public bool IsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece)
        {
            return concernedChessPiece.ActionModelMonitor.IsPromoteMoveAllowed(chessBoard, concernedChessPiece, this.ToType);
        }
    }
}
