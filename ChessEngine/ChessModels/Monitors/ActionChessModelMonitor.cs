using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Maths;
using ChessEngine.Moves;

namespace ChessEngine.ChessModels.Monitors
{
    public class ActionChessModelMonitor
    {
        public ShiftActionChessModelAgreggator ShiftActionChessModels
        {
            get;
            private set;
        }

        public PromoteActionChessModelAgreggator PromoteActionChessModels
        {
            get;
            private set;
        }

        public ActionChessModelMonitor()
        {
            this.ShiftActionChessModels = new ShiftActionChessModelAgreggator();
            this.PromoteActionChessModels = new PromoteActionChessModelAgreggator();
        }

        public bool CreateShiftMoveFrom(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition, out ChessPieceMovesContainer chessPieceMove)
        {
            return this.ShiftActionChessModels.CreateMoveFrom(chessBoard, concernedChessPiece, toPosition, out chessPieceMove);
        }

        public bool IsShiftMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPiecePosition toPosition)
        {
            return this.ShiftActionChessModels.IsMoveAllowed(chessBoard, concernedChessPiece, toPosition);
        }


        public bool CreatePromoteMoveFrom(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPieceType newType, out ChessPieceMovesContainer chessPieceMove)
        {
            return this.PromoteActionChessModels.CreateMoveFrom(chessBoard, concernedChessPiece, newType, out chessPieceMove);
        }

        public bool IsPromoteMoveAllowed(ChessBoard chessBoard, ChessPiece concernedChessPiece, ChessPieceType newType)
        {
            return this.PromoteActionChessModels.IsMoveAllowed(chessBoard, concernedChessPiece, newType);
        }

        public List<ChessPieceMovesContainer> GetAllPossibleMoves(ChessBoard chessBoard, ChessPiece concernedChessPiece)
        {
            List<ChessPieceMovesContainer> possibleMoves = new List<ChessPieceMovesContainer>();

            possibleMoves.AddRange(this.ShiftActionChessModels.GetAllPossibleMoves(chessBoard, concernedChessPiece));

            possibleMoves.AddRange(this.PromoteActionChessModels.GetAllPossibleMoves(chessBoard, concernedChessPiece));

            return possibleMoves;
        }
    }
}
