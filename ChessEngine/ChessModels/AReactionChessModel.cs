using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Moves;

namespace ChessEngine.ChessModels
{
    internal abstract class AReactionChessModel : IReactionChessModel
    {
        public bool Visit(ChessBoard chessBoard, ChessPieceMovesContainer chessPieceMoveContainer)
        {
            if (chessPieceMoveContainer.ChessPieceMoves.Count > 0)
            {
                return this.InternalVisitChessMove(chessBoard, chessPieceMoveContainer, chessPieceMoveContainer.ChessPieceMoves.First());
            }

            return false;
        }

        public abstract bool InternalVisitChessMove(ChessBoard chessBoard, ChessPieceMovesContainer chessPieceMoveContainer, IChessPieceMove chessPieceMove);
    }
}
