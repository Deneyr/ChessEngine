using ChessEngine;
using ChessEngine.Moves;
using ChessEngine.Players;
using ChessInterface.Events;
using ChessInterface.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInterface
{
    public class ChessBoardInterface
    {
        private readonly object interfaceLock = new object();

        private ChessBoard chessBoard;

        private IChessBoardHandler chessBoardHandler;

        public ChessBoardInterface(IChessBoardHandler chessBoardHandler)
        {
            this.chessBoard = null;

            this.chessBoardHandler = chessBoardHandler;
            this.chessBoardHandler.ParentInterface = this;
        }

        public void RegisterChessBoard(ChessBoard chessBoardToRegister)
        {
            this.UnregisterChessBoard();

            this.chessBoard = chessBoardToRegister;

            this.chessBoard.ChessGameStarting += this.OnChessGameStarting;
            this.chessBoard.ChessGameStarted += this.OnChessGameStarted;

            this.chessBoard.PlayerAddedToBoard += this.OnPlayerAddedToBoard;
            this.chessBoard.PieceAddedToBoard += OnPieceAddedToBoard;

            this.chessBoard.NextTurnStarted += this.OnNextTurnStarted;
            this.chessBoard.PreviousTurnStarted += this.OnPreviousTurnStarted;

            this.chessBoard.MoveApplied += this.OnMoveApplied;
            this.chessBoard.MoveReverted += this.OnMoveReverted;
        }

        public void UnregisterChessBoard()
        {
            if(this.chessBoard != null)
            {
                this.chessBoard.ChessGameStarting -= this.OnChessGameStarting;
                this.chessBoard.ChessGameStarted -= this.OnChessGameStarted;

                this.chessBoard.PlayerAddedToBoard -= this.OnPlayerAddedToBoard;
                this.chessBoard.PieceAddedToBoard -= OnPieceAddedToBoard;

                this.chessBoard.NextTurnStarted -= this.OnNextTurnStarted;
                this.chessBoard.PreviousTurnStarted -= this.OnPreviousTurnStarted;

                this.chessBoard.MoveApplied -= this.OnMoveApplied;
                this.chessBoard.MoveReverted -= this.OnMoveReverted;
            }
        }

        private void OnChessGameStarting()
        {
            this.chessBoardHandler.OnChessGameStarting();
        }

        private void OnChessGameStarted()
        {
            this.chessBoardHandler.OnChessGameStarted();
        }

        private void OnPlayerAddedToBoard(IPlayer playerAdded)
        {
            this.chessBoardHandler.OnPlayerAddedToBoard(playerAdded);
        }

        private void OnPieceAddedToBoard(ChessPiece chessPieceAdded)
        {
            this.chessBoardHandler.OnPieceAddedToBoard(chessPieceAdded);
        }

        private void OnMoveApplied(ChessPieceMovesContainer moveApplied)
        {
            this.chessBoardHandler.EnqueueChessEvent(new ChessEvent(ChessEventType.MOVE_APPLIED, null, moveApplied));
        }

        private void OnMoveReverted(ChessPieceMovesContainer moveReverted)
        {
            this.chessBoardHandler.EnqueueChessEvent(new ChessEvent(ChessEventType.MOVE_REVERTED, null, moveReverted));
        }

        private void OnNextTurnStarted(ChessTurn nextTurnStarted)
        {
            this.chessBoardHandler.EnqueueChessEvent(new ChessEvent(ChessEventType.NEXT_TURN, nextTurnStarted, null));
        }

        private void OnPreviousTurnStarted(ChessTurn previousTurnStarted)
        {
            this.chessBoardHandler.EnqueueChessEvent(new ChessEvent(ChessEventType.PREVIOUS_TURN, previousTurnStarted, null));
        }

        //internal bool DequeueChessEvent(out ChessEvent chessEvent)
        //{
        //    chessEvent = new ChessEvent();
        //    bool result = false;

        //    if (this.inputEvents.Any())
        //    {
        //        lock (this.interfaceLock)
        //        {
        //            chessEvent = this.inputEvents.Dequeue();
        //            result = true;
        //        }
        //    }

        //    return result;
        //}
    }
}
