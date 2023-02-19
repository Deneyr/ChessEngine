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
        public ChessPieceType ToChessType
        {
            get;
            private set;
        }

        public PromoteChessMoveInfluence(ChessPieceType toChessType)
        {
            this.ToChessType = toChessType;
        }

        public bool CreateMove(ChessBoard chessBoard, ChessPiece concernedChessPiece, out ChessPieceMovesContainer chessPieceMove)
        {
            return concernedChessPiece.ActionModelMonitor.CreatePromoteMoveFrom(chessBoard, concernedChessPiece, this.ToChessType, out chessPieceMove);
        }

        public bool IsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece)
        {
            return concernedChessPiece.ActionModelMonitor.IsPromoteMoveAllowed(chessBoard, concernedChessPiece, this.ToChessType);
        }
    }
}
