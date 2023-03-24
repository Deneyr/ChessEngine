using ChessAI.AStarAI.MoveTree;
using ChessEngine.Moves;
using ChessEngine.Players;
using ChessInterface.Events;
using ChessInterface.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.AStarAI
{
    public class AStarAI: AChessBoardAsyncHandler
    {
        private Random random;

        public AStarAI()
        {
            this.random = new Random((int)DateTime.Now.Ticks);
        }

        protected override void InternalHandleChessEvent(ChessEvent chessEvent)
        {
            if (this.CanEmitInfluence(chessEvent))
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
            MoveRoot lMoveRoot = new MoveRoot(this.internalChessBoard, 500, 5);

            float turnState = this.CurrentBoardTurnState(this.internalChessBoard);

            lMoveRoot.InitRoot();

            while (lMoveRoot.StudyMostPotentialMove());

            lMoveRoot.cleanRoot();

            float turnStateAfter = this.CurrentBoardTurnState(this.internalChessBoard);
            if (turnState != turnStateAfter)
            {
                throw new Exception("Alteration of the internal chessBoard during bruteForce forbidden");
            }

            List<ChessPieceMovesContainer> lResult = new List<ChessPieceMovesContainer>();            
            AMoveComponent lCurrentComponent = lMoveRoot.BestChild;
            while(lCurrentComponent != null)
            {
                lResult.Add(lCurrentComponent.Command);
                lCurrentComponent = lCurrentComponent.BestChild;
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
