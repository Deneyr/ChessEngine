using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Players
{
    public abstract class APlayer: IPlayer
    {
        public virtual List<ChessPiece> ChessPiecesOwned
        {
            get;
            private set;
        }

        public int XDirection
        {
            get;
            private set;
        }

        public int YDirection
        {
            get;
            private set;
        }

        public APlayer(int xDirection, int yDirection)
        {
            this.ChessPiecesOwned = new List<ChessPiece>();

            this.XDirection = xDirection;
            this.YDirection = yDirection;
        }

        public virtual void AddChessPieceToPlayer(ChessPiece chessPieceToAdd)
        {
            this.ChessPiecesOwned.Add(chessPieceToAdd);
        }

        public virtual void RemoveChessPieceOfPlayer(ChessPiece chessPieceToRemove)
        {
            this.ChessPiecesOwned.Add(chessPieceToRemove);
        }

        public void ClearChessPieces()
        {
            this.ChessPiecesOwned.Clear();
        }
    }
}
