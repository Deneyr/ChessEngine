using ChessEngine.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.ChessModels
{
    public interface IPromoteActionChessModel: IActionChessModel
    {
        bool CreateMoveFrom(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPieceType newType, out ChessPieceMovesContainer chessPieceMove);

        bool IsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPieceType newType);
    }
}
