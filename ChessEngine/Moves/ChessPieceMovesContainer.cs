using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Maths;

namespace ChessEngine.Moves
{
    public class ChessPieceMovesContainer : AChessPieceMove
    {
        public bool IsEndTurn
        {
            get;
            internal set;
        }

        public List<IChessPieceMove> ChessPieceMoves
        {
            get;
            private set;
        }

        public ChessPieceMovesContainer(ChessPiece concernedChessPiece, bool isEndTurn):
            base(concernedChessPiece)
        {
            this.ChessPieceMoves = new List<IChessPieceMove>();

            this.IsEndTurn = isEndTurn;
        }

        //public virtual bool AcceptVisitor(ChessBoard chessBoard, IChessPieceMoveVisitor chessPieceMoveVisitor)
        //{
        //    return chessPieceMoveVisitor.Visit(chessBoard, this);
        //}

        //public void AddMove(IChessPieceMove chessPieceMoveToAdd)
        //{
        //    this.ChessPieceMoves.Add(chessPieceMoveToAdd);
        //}

        //public void RemoveMove(IChessPieceMove chessPieceMoveToRemove)
        //{
        //    this.ChessPieceMoves.Remove(chessPieceMoveToRemove);
        //}

        public override bool ApplyMove(ChessBoard chessBoard)
        {
            bool isSuccess = true;
            int i = 0;
            while(i < this.ChessPieceMoves.Count && isSuccess)
            {
                isSuccess &= this.ChessPieceMoves[i].ApplyMove(chessBoard);
                ++i;
            }

            return isSuccess;
        }

        public override bool ApplyReverseMove(ChessBoard chessBoard)
        {
            bool isSuccess = true;
            int i = this.ChessPieceMoves.Count - 1;
            while (i >= 0 && isSuccess)
            {
                isSuccess &= this.ChessPieceMoves[i].ApplyReverseMove(chessBoard);
                --i;
            }

            return isSuccess;
        }
    }
}
