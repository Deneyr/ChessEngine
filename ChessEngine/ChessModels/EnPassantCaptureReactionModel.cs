using ChessEngine.Maths;
using ChessEngine.Moves;
using ChessEngine.Players;
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
                    if (chessBoard.GetChessPieceAt(shiftChessPieceMove.ToPosition) == null)
                    {
                        IPlayer ownerChessPieceMove = shiftChessPieceMove.ConcernedChessPiece.Owner;
                        ChessPiecePosition chessPiecePosition = new ChessPiecePosition(shiftChessPieceMove.ToPosition.X - ownerChessPieceMove.XDirection, shiftChessPieceMove.ToPosition.Y - ownerChessPieceMove.YDirection);

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
            }
            return true;
        }
    }
}
