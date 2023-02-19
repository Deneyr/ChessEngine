using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Players
{
    public abstract class APlayer: IPlayer
    {
        public virtual ChessPiece KingChessPiece
        {
            get;
            private set;
        }

        public virtual List<ChessPiece> ChessPiecesOwned
        {
            get;
            private set;
        }

        public virtual int XDirection
        {
            get;
            private set;
        }

        public virtual int YDirection
        {
            get;
            private set;
        }

        public APlayer(int xDirection, int yDirection)
        {
            this.ChessPiecesOwned = new List<ChessPiece>();
            this.KingChessPiece = null;

            this.XDirection = xDirection;
            this.YDirection = yDirection;
        }

        public virtual void AddChessPieceToPlayer(ChessPiece chessPieceToAdd)
        {
            this.ChessPiecesOwned.Add(chessPieceToAdd);

            if(chessPieceToAdd.ChessPieceType == ChessPieceType.KING)
            {
                this.KingChessPiece = chessPieceToAdd;
            }
        }

        public virtual void RemoveChessPieceOfPlayer(ChessPiece chessPieceToRemove)
        {
            this.ChessPiecesOwned.Add(chessPieceToRemove);
        }

        public void ClearChessPieces()
        {
            this.ChessPiecesOwned.Clear();
        }

        public abstract object Clone();
    }
}
