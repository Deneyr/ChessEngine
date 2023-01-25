using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Maths;
using ChessEngine.Moves;

namespace ChessEngine.ChessModels
{
    public class ShiftActionChessModelAgreggator : IShiftActionChessModel
    {
        private List<IShiftActionChessModel> shiftActionChessModels;

        public ShiftActionChessModelAgreggator()
        {
            this.shiftActionChessModels = new List<IShiftActionChessModel>();
        }

        public void AddShiftActionChessModel(IShiftActionChessModel chessModelToAdd)
        {
            this.shiftActionChessModels.Add(chessModelToAdd);
        }

        public void RemoveShiftActionChessModel(IShiftActionChessModel chessModelToRemove)
        {
            this.shiftActionChessModels.Remove(chessModelToRemove);
        }

        public bool CreateMoveFrom(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition, out ChessPieceMovesContainer chessPieceMove)
        {
            chessPieceMove = null;
            List<IShiftActionChessModel>.Enumerator shiftActionModelsEnum = this.shiftActionChessModels.GetEnumerator();
            bool foundChessMove = false;

            while (foundChessMove == false && shiftActionModelsEnum.MoveNext())
            {
                foundChessMove = shiftActionModelsEnum.Current.CreateMoveFrom(chessBoard, concernedChessPiece, toPosition, out chessPieceMove);
            }

            return foundChessMove;
        }

        public bool IsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            List<IShiftActionChessModel>.Enumerator shiftActionModelsEnum = this.shiftActionChessModels.GetEnumerator();
            bool isMoveAllowed = false;

            while (isMoveAllowed == false && shiftActionModelsEnum.MoveNext())
            {
                isMoveAllowed = shiftActionModelsEnum.Current.IsMoveAllowed(chessBoard, concernedChessPiece, toPosition);
            }

            return isMoveAllowed;
        }

        public List<ChessPieceMovesContainer> GetAllPossibleMoves(ChessBoard chessBoard, ChessPiece concernedChessPiece)
        {
            List<ChessPieceMovesContainer> possibleMoves = new List<ChessPieceMovesContainer>();
            List<ChessPieceMovesContainer> currentPossibleMoves = null;

            foreach(IShiftActionChessModel shiftActionChessModel in this.shiftActionChessModels)
            {
                currentPossibleMoves = shiftActionChessModel.GetAllPossibleMoves(chessBoard, concernedChessPiece);
                if(currentPossibleMoves != null && currentPossibleMoves.Count > 0)
                {
                    possibleMoves.AddRange(currentPossibleMoves);
                }
            }

            return possibleMoves;
        }
    }
}
