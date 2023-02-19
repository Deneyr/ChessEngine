using ChessEngine.ChessModels.Monitors;
using ChessEngine.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Moves
{
    public interface IChessPieceMove
    {
        ChessPiece ConcernedChessPiece
        {
            get;
        }

        bool IsChessPieceFirstMove
        {
            get;
        }

        bool ApplyMove(ChessBoard chessBoard);

        bool ApplyReverseMove(ChessBoard chessBoard);

        IChessMoveInfluence CreateInfluence();
    }
}
