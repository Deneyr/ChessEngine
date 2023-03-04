using ChessAI.BruteForceAI.MoveTree;
using ChessEngine.Moves;
using ChessEngine.Players;
using ChessInterface.Events;
using ChessInterface.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.BruteForceAI
{
    public class BruteForceAI: AChessBoardAsyncHandler
    {
        private Random random;

        public BruteForceAI()
        {
            this.random = new Random((int)DateTime.Now.Ticks);
        }

        protected override void InternalHandleChessEvent(ChessEvent chessEvent)
        {
            int currentIndexPlayer = this.internalChessBoard.CurrentChessTurn.IndexPlayer;
            IPlayer currentInternalPlayer = this.internalChessBoard.Players[currentIndexPlayer];
            IPlayer currentPlayer = this.chessBoardsReferenceManager.GetOriginFromDestination(currentInternalPlayer);

            if (currentPlayer == this.ParentInterface.SupportedPlayer)
            {
                List<ChessPieceMovesContainer> bestMoves = this.GetBestMoves();

                ChessPieceMovesContainer chosenMove = bestMoves.First();

                this.ParentInterface.EnqueueChessMoveInfluence(this.chessBoardsReferenceManager.GetOriginFromDestination(chosenMove.ConcernedChessPiece), chosenMove.CreateInfluence());
            }
        }

        //public void InitializeSimulation(CardDeck pCardDeck1, CardDeck pCardDeck2, int pPlayerTurn)
        //{
        //    Player lPlayer1 = new Player("Player1", pCardDeck1);
        //    Player lPlayer2 = new Player("Player2", pCardDeck2);

        //    switch (pPlayerTurn)
        //    {
        //        case 0:
        //            this.mGameArea.InitializeGame(lPlayer1, lPlayer2, lPlayer1, false);
        //            break;
        //        case 1:
        //            this.mGameArea.InitializeGame(lPlayer1, lPlayer2, lPlayer2, false);
        //            break;
        //    }
        //}

        public List<ChessPieceMovesContainer> GetBestMoves() 
        {
            MoveRoot lMoveRoot = new MoveRoot();

            AMoveComponent lCurrentComponent = lMoveRoot;
            AMoveComponent lNextComponent;

            float turnState = this.CurrentBoardTurnState(this.internalChessBoard);

            //System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            //stopWatch.Start();
            lCurrentComponent.CreateChildren(this.internalChessBoard);
            //stopWatch.Stop();
            //TimeSpan ts = stopWatch.Elapsed;
            //Console.WriteLine("RunTime " + ts);

            while ((lNextComponent = lCurrentComponent.GetNextChild()) != null || lCurrentComponent.Parent != null)
            {
                if(lNextComponent != null)
                {
                    //lNextComponent.Command.RunCommand();
                    this.internalChessBoard.ComputeChessPieceMoves(lNextComponent.Command);

                    lNextComponent.CreateChildren(this.internalChessBoard);

                    lCurrentComponent = lNextComponent;
                }
                else
                {
                    lCurrentComponent.ComputeFitnessAndClean(this.internalChessBoard);
                    lCurrentComponent.AlphaBetaPruning();

                    //this.mGameArea.SetFromSnapshot(lCurrentComponent.Parent.GameAreaData);
                    this.internalChessBoard.RevertLastChessMove();

                    lCurrentComponent = lCurrentComponent.Parent;
                }
            }

            lMoveRoot.ComputeFitnessAndClean(this.internalChessBoard);

            List<ChessPieceMovesContainer> lResult = new List<ChessPieceMovesContainer>();
            
            lCurrentComponent = lMoveRoot.BestChild;
            while(lCurrentComponent != null)
            {
                lResult.Add(lCurrentComponent.Command);
                lCurrentComponent = lCurrentComponent.BestChild;
            }

            float turnStateAfter = this.CurrentBoardTurnState(this.internalChessBoard);
            if(turnState != turnStateAfter)
            {
                throw new Exception("Alteration of the internal chessBoard during bruteForce forbidden");
            }

            return lResult;
        }

        // TEST TO REMOVE
        private float CurrentBoardTurnState(ChessEngine.ChessBoard chessBoard)
        {
            return chessBoard.ChessTurns.Count + chessBoard.CurrentChessTurn.TurnMoves.Count / 10f;
        }
    }
}
