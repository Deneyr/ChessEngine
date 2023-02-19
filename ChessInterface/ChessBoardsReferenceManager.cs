using ChessEngine;
using ChessEngine.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInterface
{
    public class ChessBoardsReferenceManager
    {
        private Dictionary<IPlayer, IPlayer> PlayersOriginToDestination;
        private Dictionary<IPlayer, IPlayer> PlayersDestinationToOrigin;

        private Dictionary<ChessPiece, ChessPiece> ChessPiecesOriginToDestination;
        private Dictionary<ChessPiece, ChessPiece> ChessPiecesDestinationToOrigin;

        public ChessBoardsReferenceManager()
        {
            this.PlayersOriginToDestination = new Dictionary<IPlayer, IPlayer>();
            this.PlayersDestinationToOrigin = new Dictionary<IPlayer, IPlayer>();

            this.ChessPiecesOriginToDestination = new Dictionary<ChessPiece, ChessPiece>();
            this.ChessPiecesDestinationToOrigin = new Dictionary<ChessPiece, ChessPiece>();
        }

        public void RegisterPlayers(IPlayer origin, IPlayer destination)
        {
            this.PlayersOriginToDestination.Add(origin, destination);
            this.PlayersDestinationToOrigin.Add(destination, origin);
        }

        public void RegisterChessPieces(ChessPiece origin, ChessPiece destination)
        {
            this.ChessPiecesOriginToDestination.Add(origin, destination);
            this.ChessPiecesDestinationToOrigin.Add(destination, origin);
        }

        public IPlayer GetOriginFromDestination(IPlayer destination)
        {
            if(this.PlayersDestinationToOrigin.TryGetValue(destination, out IPlayer origin))
            {
                return origin;
            }
            return null;
        }
        public IPlayer GetDestinationFromOrigin(IPlayer origin)
        {
            if (this.PlayersOriginToDestination.TryGetValue(origin, out IPlayer destination))
            {
                return destination;
            }
            return null;
        }


        public ChessPiece GetOriginFromDestination(ChessPiece destination)
        {
            if (this.ChessPiecesDestinationToOrigin.TryGetValue(destination, out ChessPiece origin))
            {
                return origin;
            }
            return null;
        }
        public ChessPiece GetDestinationFromOrigin(ChessPiece origin)
        {
            if (this.ChessPiecesOriginToDestination.TryGetValue(origin, out ChessPiece destination))
            {
                return destination;
            }
            return null;
        }
    }
}
