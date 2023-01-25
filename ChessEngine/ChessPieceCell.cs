using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    internal class ChessPieceCell
    {
        public ChessPiece mainChessPiece
        {
            get;
            private set;
        }

        public ChessPiece secondChessPiece
        {
            get;
            private set;
        }

        public void AddChessPieceToCell(ChessPiece chessPiece)
        {
            if (this.mainChessPiece != null)
            {
                this.secondChessPiece = chessPiece;
            }
            else
            {
                this.mainChessPiece = chessPiece;
            }
        }

        public void RemoveChessPieceOfCell(ChessPiece chessPiece)
        {
            if (this.mainChessPiece == chessPiece)
            {
                this.mainChessPiece = this.secondChessPiece;
                this.secondChessPiece = null;
            }
            else if (this.secondChessPiece == chessPiece)
            {
                this.secondChessPiece = null;
            }
        }

        public ChessPieceCell()
        {
            this.mainChessPiece = null;
            this.secondChessPiece = null;
        }
    }
}
