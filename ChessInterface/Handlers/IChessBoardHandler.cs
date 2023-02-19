using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine;
using ChessEngine.Players;
using ChessInterface.Events;

namespace ChessInterface.Handlers
{
    public interface IChessBoardHandler: IDisposable
    {
        ChessBoardInterface ParentInterface
        {
            get;
            set;
        }

        void OnChessGameStarting();

        void OnChessGameStarted();

        void OnPlayerAddedToBoard(IPlayer playerAdded);

        void OnPieceAddedToBoard(ChessPiece chessPieceAdded);

        void EnqueueChessEvent(ChessEvent chessEvent);
    }
}
