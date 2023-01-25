using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Moves;

namespace ChessEngine.ChessModels
{
    internal class PromoteActionChessModel : IPromoteActionChessModel
    {
        public bool CreateMoveFrom(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPieceType newType, out ChessPieceMovesContainer chessPieceMove)
        {
            chessPieceMove = null;

            return this.IsMoveAllowed(chessBoard, concernedChessPiece, newType);
        }

        public List<ChessPieceMovesContainer> GetAllPossibleMoves(ChessBoard chessBoard, ChessPiece concernedChessPiece)
        {
            return new List<ChessPieceMovesContainer>();
        }

        public bool IsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPieceType newType)
        {
            return false;
        }
    }
}
