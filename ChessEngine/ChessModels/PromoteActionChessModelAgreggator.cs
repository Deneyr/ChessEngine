using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Moves;

namespace ChessEngine.ChessModels
{
    public class PromoteActionChessModelAgreggator : IPromoteActionChessModel
    {
        private List<IPromoteActionChessModel> promoteActionChessModels;

        public PromoteActionChessModelAgreggator()
        {
            this.promoteActionChessModels = new List<IPromoteActionChessModel>();
        }

        public void AddPromoteActionChessModel(IPromoteActionChessModel chessModelToAdd)
        {
            this.promoteActionChessModels.Add(chessModelToAdd);
        }

        public void RemovePromoteActionChessModel(IPromoteActionChessModel chessModelToRemove)
        {
            this.promoteActionChessModels.Remove(chessModelToRemove);
        }

        public bool CreateMoveFrom(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPieceType newType, out ChessPieceMovesContainer chessPieceMove)
        {
            chessPieceMove = null;
            List<IPromoteActionChessModel>.Enumerator promoteActionModelsEnum = this.promoteActionChessModels.GetEnumerator();
            bool foundChessMove = false;

            while (foundChessMove == false && promoteActionModelsEnum.MoveNext())
            {
                foundChessMove = promoteActionModelsEnum.Current.CreateMoveFrom(chessBoard, concernedChessPiece, newType, out chessPieceMove);
            }

            return foundChessMove;
        }

        public bool IsMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPieceType newType)
        {
            List<IPromoteActionChessModel>.Enumerator shiftActionModelsEnum = this.promoteActionChessModels.GetEnumerator();
            bool isMoveAllowed = false;

            while (isMoveAllowed == false && shiftActionModelsEnum.MoveNext())
            {
                isMoveAllowed = shiftActionModelsEnum.Current.IsMoveAllowed(chessBoard, concernedChessPiece, newType);
            }

            return isMoveAllowed;
        }

        public List<ChessPieceMovesContainer> GetAllPossibleMoves(ChessBoard chessBoard, ChessPiece concernedChessPiece)
        {
            List<ChessPieceMovesContainer> possibleMoves = new List<ChessPieceMovesContainer>();
            List<ChessPieceMovesContainer> currentPossibleMoves = null;

            foreach (IPromoteActionChessModel promoteActionChessModel in this.promoteActionChessModels)
            {
                currentPossibleMoves = promoteActionChessModel.GetAllPossibleMoves(chessBoard, concernedChessPiece);
                if (currentPossibleMoves != null && currentPossibleMoves.Count > 0)
                {
                    possibleMoves.AddRange(currentPossibleMoves);
                }
            }

            return possibleMoves;
        }
    }
}
