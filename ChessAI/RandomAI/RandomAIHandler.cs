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

namespace ChessAI.RandomAI
{
    public class RandomAIHandler: AChessBoardAsyncHandler
    {
        private Random random;

        public RandomAIHandler()
        {
            this.random = new Random((int)DateTime.Now.Ticks);
        }

        protected override void InternalHandleChessEvent(ChessEvent chessEvent)
        {
            if (this.CanEmitInfluence(chessEvent))
            {
                ChessPieceMovesContainer randomMove = this.GetCurrentRandomMove();

                if (randomMove != null)
                {
                    this.ParentInterface.EnqueueChessMoveInfluence(this.chessBoardsReferenceManager.GetOriginFromDestination(randomMove.ConcernedChessPiece), randomMove.CreateInfluence());
                }
            }
        }

        private ChessPieceMovesContainer GetCurrentRandomMove()
        {
            List<ChessPieceMovesContainer> possibleMovesList = new List<ChessPieceMovesContainer>();

            IPlayer currentInternalPlayer = this.internalChessBoard.Players[this.internalChessBoard.CurrentChessTurn.IndexPlayer];

            foreach (ChessPiece chessPiece in currentInternalPlayer.ChessPiecesOwned)
            {
                if (chessPiece.IsAlive)
                {
                    possibleMovesList.AddRange(chessPiece.GetAllPossibleMoves(this.internalChessBoard));
                }
            }

            if (possibleMovesList.Any())
            {
                int randomIndex = this.random.Next(0, possibleMovesList.Count);

                return possibleMovesList[randomIndex];
            }
            return null;
        }
    }
}
