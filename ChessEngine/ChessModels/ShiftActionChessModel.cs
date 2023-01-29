using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Maths;
using ChessEngine.Moves;

namespace ChessEngine.ChessModels
{
    internal class ShiftActionChessModel : IShiftActionChessModel
    {
        protected int xShift;
        protected int yShift;

        protected int numberCellrange;

        public ShiftActionChessModel(int xShift, int yShift, int numberCellrange)
        {
            this.xShift = xShift;
            this.yShift = yShift;

            this.numberCellrange = numberCellrange;
        }

        public bool CreateMoveFrom(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition, out ChessPieceMovesContainer chessPieceMove)
        {
            chessPieceMove = null;

            if (this.IsMoveAllowed(chessBoard, concernedChessPiece, toPosition))
            {
                chessPieceMove = new ChessPieceMovesContainer(concernedChessPiece, this.IsEndTurnMove(chessBoard, concernedChessPiece, toPosition));
                chessPieceMove.ChessPieceMoves.Add(new ShiftChessPieceMove(concernedChessPiece, concernedChessPiece.ChessPiecePosition, toPosition));

                return true;
            }

            return false;
        }

        public List<ChessPieceMovesContainer> GetAllPossibleMoves(ChessBoard chessBoard, ChessPiece concernedChessPiece)
        {
            List<ChessPieceMovesContainer> resultChessPieceMoves = new List<ChessPieceMovesContainer>();

            if (chessBoard.IsTurnFirstMove)
            {
                int maxStep = chessBoard.RayTrace(
                    concernedChessPiece.ChessPiecePosition,
                    this.GetXShift(concernedChessPiece),
                    this.GetYShift(concernedChessPiece),
                    this.GetNumberCellrange(chessBoard, concernedChessPiece),
                    out ChessPiece chessPieceTouched);

                if (chessPieceTouched != null && chessPieceTouched.Owner != concernedChessPiece.Owner)
                {
                    ++maxStep;
                }

                for (int i = 1; i <= maxStep; i++)
                {
                    ChessPiecePosition toPosition = new ChessPiecePosition(
                        concernedChessPiece.ChessPiecePosition.X + (i * this.GetXShift(concernedChessPiece)),
                        concernedChessPiece.ChessPiecePosition.Y + (i * this.GetYShift(concernedChessPiece)));

                    if (this.InternalIsMoveAllowed(chessBoard, concernedChessPiece, toPosition))
                    {
                        ChessPieceMovesContainer chessPieceMovesContainer = new ChessPieceMovesContainer(concernedChessPiece, this.IsEndTurnMove(chessBoard, concernedChessPiece, toPosition));
                        chessPieceMovesContainer.ChessPieceMoves.Add(new ShiftChessPieceMove(concernedChessPiece, concernedChessPiece.ChessPiecePosition, toPosition));

                        resultChessPieceMoves.Add(chessPieceMovesContainer);
                    }
                }
            }

            return resultChessPieceMoves;
        }

        public bool IsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            if (chessBoard.IsTurnFirstMove)
            {
                if (this.InternalIsMoveAllowed(chessBoard, concernedChessPiece, toPosition) && chessBoard.IsPositionOnChessBoard(toPosition))
                {
                    if (this.IsInRange(concernedChessPiece, concernedChessPiece.ChessPiecePosition, toPosition, out int nbStepToReach)
                        && nbStepToReach > 0
                        && nbStepToReach <= this.GetNumberCellrange(chessBoard, concernedChessPiece))
                    {
                        int maxStep = chessBoard.RayTrace(
                            concernedChessPiece.ChessPiecePosition,
                            this.GetXShift(concernedChessPiece),
                            this.GetYShift(concernedChessPiece),
                            nbStepToReach,
                            out ChessPiece chessPieceTouched);

                        return maxStep == nbStepToReach
                            || (maxStep == nbStepToReach - 1 && chessPieceTouched != null && chessPieceTouched.Owner != concernedChessPiece.Owner);
                    }
                }
            }

            return false;
        }

        private bool IsInRange(ChessPiece concernedChessPiece, ChessPiecePosition fromPosition, ChessPiecePosition toPosition, out int nbStepToReach)
        {
            nbStepToReach = 0;

            if (fromPosition == toPosition)
            {
                return true;
            }

            int xShift = this.GetXShift(concernedChessPiece);
            int yShift = this.GetYShift(concernedChessPiece);

            int xDiff = toPosition.X - fromPosition.X;
            int yDiff = toPosition.Y - fromPosition.Y;

            if (xShift == 0)
            {
                if (xDiff == 0
                    && yDiff % yShift == 0)
                {
                    nbStepToReach = yDiff / yShift;
                    return true;
                }
                return false;
            }

            if (yShift == 0)
            {
                if (yDiff == 0 
                    && xDiff % xShift == 0)
                {
                    nbStepToReach = xDiff / xShift;
                    return true;
                }
                return false;
            }

            if(xDiff % xShift == 0 
                && yDiff % yShift == 0)
            {
                int nbStepToReachX = xDiff / xShift;
                int nbStepToReachY = yDiff / yShift;

                if(nbStepToReachX == nbStepToReachY 
                    && nbStepToReachX > 0)
                {
                    nbStepToReach = nbStepToReachX;
                    return true;
                }
            }
            return false;
        }

        protected virtual bool InternalIsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            return true;
        }

        protected virtual int GetXShift(ChessPiece concernedChessPiece)
        {
            return this.xShift;
        }

        protected virtual int GetYShift(ChessPiece concernedChessPiece)
        {
            return this.yShift;
        }

        protected virtual int GetNumberCellrange(ChessBoard chessBoard, ChessPiece concernedChessPiece)
        {
            return this.numberCellrange;
        }

        protected virtual bool IsEndTurnMove(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            return true;
        }
    }
}
