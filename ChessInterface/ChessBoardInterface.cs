using ChessEngine;
using ChessEngine.ChessModels.Monitors;
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

        public IPlayer SupportedPlayer
        {
            get;
            set;
        }

        private Queue<Tuple<ChessPiece, IChessMoveInfluence>> influencesEmitted;

        private float influenceMinTimerSec;
        private float influenceCurrentTimerSec;

        public ChessBoardInterface(IChessBoardHandler chessBoardHandler, float influenceMinTimerSec = 0)
        {
            this.chessBoard = null;

            this.chessBoardHandler = chessBoardHandler;
            this.SupportedPlayer = null;
            this.chessBoardHandler.ParentInterface = this;

            this.influencesEmitted = new Queue<Tuple<ChessPiece, IChessMoveInfluence>>();
            this.influenceMinTimerSec = influenceMinTimerSec;
            this.influenceCurrentTimerSec = 0;
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
            lock (this.interfaceLock)
            {
                this.influencesEmitted.Clear();
            }

            this.chessBoardHandler.OnChessGameStarting();
        }

        private void OnChessGameStarted()
        {
            this.influenceCurrentTimerSec = 0;

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
            this.InitNewTurn();

            this.chessBoardHandler.EnqueueChessEvent(new ChessEvent(ChessEventType.NEXT_TURN, nextTurnStarted, null));
        }

        private void OnPreviousTurnStarted(ChessTurn previousTurnStarted)
        {
            this.InitNewTurn();

            this.chessBoardHandler.EnqueueChessEvent(new ChessEvent(ChessEventType.PREVIOUS_TURN, previousTurnStarted, null));
        }

        private void InitNewTurn()
        {
            lock (this.interfaceLock)
            {
                this.influencesEmitted.Clear();
            }

            this.influenceCurrentTimerSec = 0;
        }

        public void UpdateInterface(float deltaSec)
        {
            if (this.influenceMinTimerSec > 0)
            {
                this.influenceCurrentTimerSec += deltaSec;
            }

            if (this.influenceMinTimerSec <= 0
                || this.influenceCurrentTimerSec > this.influenceMinTimerSec)
            {
                if (this.DequeueChessEvent(out ChessPiece concernedChessPiece, out IChessMoveInfluence chessMoveInfluence))
                {
                    this.chessBoard.ComputeChessPieceInfluence(concernedChessPiece, chessMoveInfluence);
                }
            }
        }

        private bool DequeueChessEvent(out ChessPiece concernedChessPiece, out IChessMoveInfluence chessMoveInfluence)
        {
            chessMoveInfluence = null;
            concernedChessPiece = null;
            bool result = false;
            lock (this.interfaceLock)
            {
                if (this.influencesEmitted.Any())
                {
                    Tuple<ChessPiece, IChessMoveInfluence> influence = this.influencesEmitted.Dequeue();
                    concernedChessPiece = influence.Item1;
                    chessMoveInfluence = influence.Item2;
                    result = true;
                }
            }

            return result;
        }

        public virtual void EnqueueChessMoveInfluence(ChessPiece concernedChessPiece, IChessMoveInfluence chessMoveInfluence)
        {
            lock (this.interfaceLock)
            {
                this.influencesEmitted.Enqueue(new Tuple<ChessPiece, IChessMoveInfluence>(concernedChessPiece, chessMoveInfluence));
            }
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
