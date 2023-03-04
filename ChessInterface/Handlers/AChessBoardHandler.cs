using ChessEngine;
using ChessEngine.ChessModels.Monitors;
using ChessEngine.Players;
using ChessInterface.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInterface.Handlers
{
    public abstract class AChessBoardHandler : IChessBoardHandler
    {
        protected readonly object handlerLock = new object();

        public ChessBoardInterface ParentInterface
        {
            get;
            private set;
        }

        protected ChessBoard internalChessBoard;
        protected ChessBoardsReferenceManager chessBoardsReferenceManager;

        private Queue<ChessEvent> inputEvents;

        public AChessBoardHandler()
        {
            this.internalChessBoard = new ChessBoard();
            this.chessBoardsReferenceManager = new ChessBoardsReferenceManager();

            this.inputEvents = new Queue<ChessEvent>();
        }

        protected void UpdateHandler()
        {
            while(this.DequeueChessEvent(out ChessEvent chessEvent))
            {
                this.UpdateInternalChessBoard(chessEvent);

                this.InternalHandleChessEvent(chessEvent);
            }
        }

        protected abstract void InternalHandleChessEvent(ChessEvent chessEvent);

        private void UpdateInternalChessBoard(ChessEvent chessEvent)
        {
            if(chessEvent.EventType == ChessEventType.NEXT_TURN
                && chessEvent.EventTurn == null)
            {
                this.internalChessBoard.InitFirstTurn();
            }
            else if(chessEvent.EventType == ChessEventType.MOVE_APPLIED)
            {
                IChessMoveInfluence moveInfluence = chessEvent.EventMove.CreateInfluence();

                if(this.internalChessBoard.ComputeChessPieceInfluence(this.chessBoardsReferenceManager.GetDestinationFromOrigin(chessEvent.EventMove.ConcernedChessPiece), moveInfluence) == false)
                {
                    throw new Exception("Desync between chess board and internal chess board");
                }
            }
            else if(chessEvent.EventType == ChessEventType.MOVE_REVERTED)
            {
                this.internalChessBoard.RevertLastChessMove();
            }
        }

        private bool DequeueChessEvent(out ChessEvent chessEvent)
        {
            chessEvent = null;
            bool result = false;
            lock (this.handlerLock)
            {
                if (this.inputEvents.Any())
                {
                    chessEvent = this.inputEvents.Dequeue();
                    result = true;
                }
            }

            return result;
        }

        public void OnChessGameStarting()
        {
            lock (this.handlerLock)
            {
                this.inputEvents.Clear();

                this.internalChessBoard.InitGame();
            }
        }

        public void OnChessGameStarted()
        {
            //lock (this.handlerLock)
            //{
            //    this.internalChessBoard.InitFirstTurn();
            //}
        }

        public void OnPlayerAddedToBoard(IPlayer playerAdded)
        {
            lock (this.handlerLock)
            {
                IPlayer newPlayer = playerAdded.Clone() as IPlayer;
                newPlayer.ClearChessPieces();

                this.chessBoardsReferenceManager.RegisterPlayers(playerAdded, newPlayer);
                this.internalChessBoard.AddPlayer(newPlayer);
            }
        }

        public void OnPieceAddedToBoard(ChessPiece chessPieceAdded)
        {
            lock (this.handlerLock)
            {
                ChessPiece newChessPiece = this.internalChessBoard.CreateChessPiece(
                    this.chessBoardsReferenceManager.GetDestinationFromOrigin(chessPieceAdded.Owner), 
                    chessPieceAdded.ChessPieceType, 
                    chessPieceAdded.ChessPiecePosition);

                this.chessBoardsReferenceManager.RegisterChessPieces(chessPieceAdded, newChessPiece);
                this.internalChessBoard.AddChessPiece(newChessPiece);
            }
        }

        public virtual void EnqueueChessEvent(ChessEvent chessEvent)
        {
            lock (this.handlerLock)
            {
                this.inputEvents.Enqueue(chessEvent);
            }
        }

        public virtual void Dispose()
        {

        }

        public virtual void OnInterfaceAttached(ChessBoardInterface parentInterface)
        {
            this.ParentInterface = parentInterface;
        }

        public virtual void OnInterfaceDetached(ChessBoardInterface parentInterface)
        {
            this.ParentInterface = null;
        }
    }
}
