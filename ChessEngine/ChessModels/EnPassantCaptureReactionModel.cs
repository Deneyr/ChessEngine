using ChessEngine.Maths;
using ChessEngine.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.ChessModels
{
    internal class EnPassantCaptureReactionChessModel : AReactionChessModel
    {
        public override bool InternalVisitChessMove(ChessBoard chessBoard, ChessPieceMovesContainer chessPieceMoveContainer, IChessPieceMove chessPieceMove)
        {
            if (chessPieceMove is ShiftChessPieceMove)
            {
                ShiftChessPieceMove shiftChessPieceMove = chessPieceMove as ShiftChessPieceMove;

                if (Math.Abs(shiftChessPieceMove.ToPosition.X - shiftChessPieceMove.FromPosition.X) > 0
                    && Math.Abs(shiftChessPieceMove.ToPosition.Y - shiftChessPieceMove.FromPosition.Y) > 0)
                {
                    ChessPiecePosition chessPiecePosition = new ChessPiecePosition(shiftChessPieceMove.ToPosition.X, shiftChessPieceMove.FromPosition.Y);
                    ChessPiece chessPieceAtDestination = chessBoard.GetChessPieceAt(chessPiecePosition);
                    if (chessPieceAtDestination != null)
                    {
                        if (chessPieceAtDestination.Owner != shiftChessPieceMove.ConcernedChessPiece.Owner)
                        {
                            KillChessPieceMove killChessPieceMove = new KillChessPieceMove(chessPieceAtDestination, chessPieceAtDestination.ChessPiecePosition);
                            chessPieceMoveContainer.ChessPieceMoves.Add(killChessPieceMove);
                        }
                    }
                }
            }
            return true;
        }
    }
}
