using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Maths;
using ChessEngine.Moves;

namespace ChessEngine.ChessModels
{
    internal class EnPassantShiftActionChessModel : ShiftActionChessModel
    {
        public EnPassantShiftActionChessModel(int xShift) 
            : base(xShift, 1, 1)
        {
        }

        protected override bool InternalIsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            //ChessPiecePosition chessPiecePosition = new ChessPiecePosition(concernedChessPiece.ChessPiecePosition.X + this.GetXShift(concernedChessPiece), concernedChessPiece.ChessPiecePosition.Y + this.GetYShift(concernedChessPiece));
            if (chessBoard.IsPositionOnChessBoard(toPosition)
                && this.IsEnPassantValidAt(chessBoard, concernedChessPiece, toPosition) == false)
            {
                return false;
            }

            return true;
        }

        private bool IsEnPassantValidAt(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            if (chessBoard.ChessTurns.Count > 1)
            {
                MathHelper.ChangeBaseVector(0, -this.yShift, concernedChessPiece.Owner.XDirection, concernedChessPiece.Owner.YDirection, out int xGlobalShiftTo, out int yGlobalShiftTo);
                MathHelper.ChangeBaseVector(0, this.yShift, concernedChessPiece.Owner.XDirection, concernedChessPiece.Owner.YDirection, out int xGlobalShiftFrom, out int yGlobalShiftFrom);

                ChessPiecePosition enPassantToPosition = new ChessPiecePosition(toPosition.X + xGlobalShiftTo, toPosition.Y + yGlobalShiftTo);

                ChessPiece chessPiece = chessBoard.GetChessPieceAt(enPassantToPosition);
                if (chessPiece != null 
                    && chessPiece.Owner != concernedChessPiece.Owner
                    && chessPiece.ChessPieceType == ChessPieceType.PAWN)
                {
                    ChessTurn previousTurn = chessBoard.ChessTurns.ElementAt(chessBoard.ChessTurns.Count - 2);
                    ChessPieceMovesContainer previousMoves = previousTurn.TurnMoves.FirstOrDefault();
                    if (previousMoves != null)
                    {
                        ChessPiecePosition enPassantFromPosition = new ChessPiecePosition(toPosition.X + xGlobalShiftFrom, toPosition.Y + yGlobalShiftFrom);

                        IChessPieceMove previousMove = previousMoves.ChessPieceMoves.First();
                        return previousMove is ShiftChessPieceMove
                            && previousMove.ConcernedChessPiece == chessPiece
                            && (previousMove as ShiftChessPieceMove).FromPosition == enPassantFromPosition;
                    }
                }
            }

            return false;
        }

        protected override int GetXShift(ChessPiece concernedChessPiece)
        {
            MathHelper.ChangeBaseVector(this.xShift, this.yShift, concernedChessPiece.Owner.XDirection, concernedChessPiece.Owner.YDirection, out int xGlobalShift, out int yGlobalShift);

            return xGlobalShift;
        }

        protected override int GetYShift(ChessPiece concernedChessPiece)
        {
            MathHelper.ChangeBaseVector(this.xShift, this.yShift, concernedChessPiece.Owner.XDirection, concernedChessPiece.Owner.YDirection, out int xGlobalShift, out int yGlobalShift);

            return yGlobalShift;
        }
    }
}
