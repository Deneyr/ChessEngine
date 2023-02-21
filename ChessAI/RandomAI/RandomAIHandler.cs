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
        protected override void InternalHandleChessEvent(ChessEvent chessEvent)
        {
            int currentIndexPlayer = this.internalChessBoard.CurrentChessTurn.IndexPlayer;
            IPlayer currentInternalPlayer = this.internalChessBoard.Players[currentIndexPlayer];
            IPlayer currentPlayer = this.chessBoardsReferenceManager.GetOriginFromDestination(currentInternalPlayer);

            if (currentPlayer == this.ParentInterface.SupportedPlayer)
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

            // TO REMOVE
            //if (currentInternalPlayer.YDirection > 0)
            //{
            //    List<ChessPieceMovesContainer> tempMoves = new List<ChessPieceMovesContainer>(possibleMovesList);
            //    possibleMovesList = possibleMovesList.Where(pElem => pElem.ChessPieceMoves.Count > 1 && pElem.ConcernedChessPiece == currentInternalPlayer.KingChessPiece).ToList();

            //    if(possibleMovesList.Any() == false)
            //    {
            //        possibleMovesList = tempMoves;
            //    }
            //}

            if (possibleMovesList.Any())
            {
                Random random = new Random();
                int randomIndex = random.Next(0, possibleMovesList.Count);

                return possibleMovesList[randomIndex];
            }
            return null;
        }
    }
}
