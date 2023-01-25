using ChessEngine.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.ChessModels.Monitors
{
    public interface IChessMoveInfluence
    {
        bool CreateMove(ChessBoard chessBoard, ChessPiece concernedChessPiece, out ChessPieceMovesContainer chessPieceMove);

        bool IsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece);
    }
}
