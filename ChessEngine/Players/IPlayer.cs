using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Players
{
    public interface IPlayer
    {
        List<ChessPiece> ChessPiecesOwned
        {
            get;
        }

        void AddChessPieceToPlayer(ChessPiece chessPieceToAdd);

        void RemoveChessPieceOfPlayer(ChessPiece chessPieceToRemove);

        void ClearChessPieces();
    }
}
