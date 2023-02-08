using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Maths;
using ChessEngine.Moves;

namespace ChessEngine.ChessModels
{
    internal class CastleShiftActionChessModel : IShiftActionChessModel
    {
        protected int xShift;

        private ChessPiece currentOtherChessPiece;

        private int currentXGlobalShift;
        private int currentYGlobalShift;

        public CastleShiftActionChessModel(int xShift)
        {
            this.xShift = xShift;
        }

        public bool CreateMoveFrom(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition, out ChessPieceMovesContainer chessPieceMove)
        {
            chessPieceMove = null;

            if (this.IsMoveAllowed(chessBoard, concernedChessPiece, toPosition))
            {
                chessPieceMove = new ChessPieceMovesContainer(concernedChessPiece, true);

                chessPieceMove.ChessPieceMoves.Add(new ShiftChessPieceMove(concernedChessPiece, concernedChessPiece.ChessPiecePosition, toPosition));
                chessPieceMove.ChessPieceMoves.Add(new ShiftChessPieceMove(this.currentOtherChessPiece, this.currentOtherChessPiece.ChessPiecePosition, new ChessPiecePosition(toPosition.X - this.currentXGlobalShift, toPosition.Y - this.currentYGlobalShift)));

                return true;
            }
            return false;
        }

        public List<ChessPieceMovesContainer> GetAllPossibleMoves(ChessBoard chessBoard, ChessPiece concernedChessPiece)
        {
            List<ChessPieceMovesContainer> resultChessPieceMoves = new List<ChessPieceMovesContainer>();

            if (chessBoard.IsTurnFirstMove && concernedChessPiece.HasAlreadyMoved == false)
            {
                ChessPiecePosition castlingPosition = this.GetCastlingPosition(chessBoard, concernedChessPiece, out ChessPiecePosition otherChessPiecePosition);

                if (castlingPosition.X >= 0 && castlingPosition.Y > 0)
                {
                    if(this.CreateMoveFrom(chessBoard, concernedChessPiece, castlingPosition, out ChessPieceMovesContainer chessPieceMoves))
                    {
                        resultChessPieceMoves.Add(chessPieceMoves);
                    }
                }
            }

            return resultChessPieceMoves;
        }

        public bool IsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            this.currentOtherChessPiece = null;

            this.currentXGlobalShift = 0;
            this.currentYGlobalShift = 0;

            if (chessBoard.IsTurnFirstMove && concernedChessPiece.HasAlreadyMoved == false)
            {
                ChessPiecePosition castlingPosition = this.GetCastlingPosition(chessBoard, concernedChessPiece, out ChessPiecePosition otherChessPiecePosition);

                if (castlingPosition.X >= 0 && castlingPosition .Y > 0 
                        && toPosition == castlingPosition)
                {
                    this.currentOtherChessPiece = chessBoard.GetChessPieceAt(otherChessPiecePosition);

                    if (this.currentOtherChessPiece != null && this.currentOtherChessPiece.HasAlreadyMoved)
                    {
                        if (this.InternalIsMoveAllowed(chessBoard, concernedChessPiece, this.currentOtherChessPiece, toPosition))
                        {
                            MathHelper.ChangeBaseVector(this.xShift, 0, concernedChessPiece.Owner.XDirection, concernedChessPiece.Owner.YDirection, out int xGlobalShift, out int yGlobalShift);

                            ChessPiecePosition throughPosition = concernedChessPiece.ChessPiecePosition;

                            while(throughPosition != toPosition
                                && chessBoard.IsGivenMovesGetChecked(this.CreateTemporaryMove(concernedChessPiece, throughPosition)) == false)
                            {
                                throughPosition.X += xGlobalShift;
                                throughPosition.Y += yGlobalShift;
                            }

                            if(throughPosition == toPosition
                                && chessBoard.IsGivenMovesGetChecked(this.CreateTemporaryMove(concernedChessPiece, throughPosition)) == false)
                            {
                                this.currentXGlobalShift = xGlobalShift;
                                this.currentYGlobalShift = yGlobalShift;

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private ChessPieceMovesContainer CreateTemporaryMove(ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            ChessPieceMovesContainer resultChessMoves = new ChessPieceMovesContainer(concernedChessPiece, true);

            resultChessMoves.ChessPieceMoves.Add(new ShiftChessPieceMove(concernedChessPiece, concernedChessPiece.ChessPiecePosition, toPosition));

            return resultChessMoves;
        }

        private ChessPiecePosition GetCastlingPosition(ChessBoard chessBoard, ChessPiece concernedChessPiece, out ChessPiecePosition otherPiecePosition)
        {
            otherPiecePosition = new ChessPiecePosition(-1, -1);

            MathHelper.ChangeBaseVector(this.xShift, 0, concernedChessPiece.Owner.XDirection, concernedChessPiece.Owner.YDirection, out int xGlobalShift, out int yGlobalShift);

            int step = chessBoard.RayTrace(concernedChessPiece.ChessPiecePosition, xGlobalShift, yGlobalShift, 1, out ChessPiece chessPieceTouched);

            if (chessPieceTouched != null)
            {
                otherPiecePosition = chessPieceTouched.ChessPiecePosition;

                int xVectorTo = (int)Math.Ceiling((otherPiecePosition.X - concernedChessPiece.ChessPiecePosition.X) / 2f);
                int yVectorTo = (int)Math.Ceiling((otherPiecePosition.Y - concernedChessPiece.ChessPiecePosition.Y) / 2f);

                ChessPiecePosition castlingPosition = new ChessPiecePosition(
                    concernedChessPiece.ChessPiecePosition.X + xVectorTo,
                    concernedChessPiece.ChessPiecePosition.Y + yVectorTo);

                return castlingPosition;
            }

            return otherPiecePosition;
        }

        protected virtual bool InternalIsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiece otherChessPiece, ChessPiecePosition toPosition)
        {
            return otherChessPiece.ChessPieceType == ChessPieceType.ROOK;
        }
    }
}
