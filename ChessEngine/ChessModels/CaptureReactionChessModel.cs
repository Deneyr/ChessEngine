using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Moves;

namespace ChessEngine.ChessModels
{
    internal class CaptureReactionChessModel : AReactionChessModel
    {
        public override bool InternalVisitChessMove(ChessBoard chessBoard, ChessPieceMovesContainer chessPieceMoveContainer, IChessPieceMove chessPieceMove)
        {
            if(chessPieceMove is ShiftChessPieceMove)
            {
                ShiftChessPieceMove shiftChessPieceMove = chessPieceMove as ShiftChessPieceMove;

                ChessPiece chessPieceAtDestination = chessBoard.GetChessPieceAt(shiftChessPieceMove.ToPosition);
                if(chessPieceAtDestination != null)
                {
                    if(chessPieceAtDestination.Owner != shiftChessPieceMove.ConcernedChessPiece.Owner)
                    {
                        KillChessPieceMove killChessPieceMove = new KillChessPieceMove(chessPieceAtDestination, chessPieceAtDestination.ChessPiecePosition);
                        chessPieceMoveContainer.ChessPieceMoves.Add(killChessPieceMove);
                    }
                }
            }
            return true;
        }
    }
}
