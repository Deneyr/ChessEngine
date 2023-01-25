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

        public APlayer()
        {
            this.ChessPiecesOwned = new List<ChessPiece>();
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
