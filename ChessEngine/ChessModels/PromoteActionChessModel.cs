using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Maths;
using ChessEngine.Moves;

namespace ChessEngine.ChessModels
{
    internal class PromoteActionChessModel : IPromoteActionChessModel
    {
        public PromoteActionChessModel()
        {
        }

        public bool CreateMoveFrom(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPieceType newType, out ChessPieceMovesContainer chessPieceMove)
        {
            chessPieceMove = null;

            if(chessBoard.IsTurnFirstMove == false)
            {
                if(this.IsMoveAllowed(chessBoard, concernedChessPiece, newType))
                {
                    chessPieceMove = new ChessPieceMovesContainer(concernedChessPiece, true);
                    chessPieceMove.ChessPieceMoves.Add(new PromoteChessPieceMove(concernedChessPiece, concernedChessPiece.ChessPieceType, newType));

                    return true;
                }
            }

            return false;
        }

        public List<ChessPieceMovesContainer> GetAllPossibleMoves(ChessBoard chessBoard, ChessPiece concernedChessPiece)
        {
            List<ChessPieceMovesContainer> resultChessPieceMoves = new List<ChessPieceMovesContainer>();

            if (chessBoard.IsTurnFirstMove == false)
            {
                if (IsInPromoteArea(chessBoard, concernedChessPiece, concernedChessPiece.ChessPiecePosition))
                {
                    Array chessPieceTypeValues = Enum.GetValues(typeof(ChessPieceType));
                    foreach(ChessPieceType chessPieceTypeValue in chessPieceTypeValues)
                    {                   
                        if (chessPieceTypeValue != ChessPieceType.KING
                            && concernedChessPiece.ChessPieceType != chessPieceTypeValue
                            && this.InternalIsMoveAllowed(chessBoard, concernedChessPiece, chessPieceTypeValue))
                        {
                            ChessPieceMovesContainer chessPieceMovesContainer = new ChessPieceMovesContainer(concernedChessPiece, true);
                            chessPieceMovesContainer.ChessPieceMoves.Add(new PromoteChessPieceMove(concernedChessPiece, concernedChessPiece.ChessPieceType, chessPieceTypeValue));

                            resultChessPieceMoves.Add(chessPieceMovesContainer);
                        }
                    }
                }
            }

            return resultChessPieceMoves;
        }

        public bool IsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPieceType newType)
        {
            if(chessBoard.IsTurnFirstMove)
            {
                if (this.InternalIsMoveAllowed(chessBoard, concernedChessPiece, newType)
                    && IsInPromoteArea(chessBoard, concernedChessPiece, concernedChessPiece.ChessPiecePosition))
                {
                    return concernedChessPiece.ChessPieceType != newType;
                }
            }

            return false;
        }

        internal static bool IsInPromoteArea(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            bool isXInPromote = true;
            if (concernedChessPiece.Owner.XDirection != 0)
            {
                int xLimit = chessBoard.Width / 2 + concernedChessPiece.Owner.XDirection * chessBoard.PromoteBorderDistance;

                if(xLimit < 0)
                {
                    xLimit = 0;
                }
                else if(xLimit >= chessBoard.Width)
                {
                    xLimit = chessBoard.Width - 1;
                }

                int xDiff = toPosition.X - xLimit;

                if(xDiff * concernedChessPiece.Owner.XDirection < 0)
                {
                    isXInPromote = false;
                }
            }

            bool isYInPromote = true;
            if (concernedChessPiece.Owner.YDirection != 0)
            {
                int yLimit = chessBoard.Height / 2 + concernedChessPiece.Owner.YDirection * chessBoard.PromoteBorderDistance;

                if (yLimit < 0)
                {
                    yLimit = 0;
                }
                else if (yLimit >= chessBoard.Height)
                {
                    yLimit = chessBoard.Height - 1;
                }

                int yDiff = toPosition.Y - yLimit;

                if (yDiff * concernedChessPiece.Owner.YDirection < 0)
                {
                    isYInPromote = false;
                }
            }

            return isXInPromote && isYInPromote;
        }

        protected virtual bool InternalIsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPieceType newType)
        {
            return true;
        }
    }
}
