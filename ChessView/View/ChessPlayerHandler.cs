using ChessEngine;
using ChessEngine.ChessModels.Monitors;
using ChessEngine.Maths;
using ChessEngine.Moves;
using ChessEngine.Players;
using ChessInterface.Events;
using ChessInterface.Handlers;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessView.View
{
    public class ChessPlayerHandler : AChessBoardSyncHandler
    {
        private ChessBoard2D parentChessBoard2D;

        private ChessPiece2D selectedChessPiece;

        private ChessPiece promotingChessPiece;

        public ChessPlayerHandler(ChessBoard2D parentChessBoard2D)
        {
            this.parentChessBoard2D = parentChessBoard2D;

            this.selectedChessPiece = null;
            this.promotingChessPiece = null;
        }

        protected override void InternalHandleChessEvent(ChessEvent chessEvent)
        {
            if(chessEvent.EventType == ChessEventType.MOVE_APPLIED
                || chessEvent.EventType == ChessEventType.MOVE_REVERTED)
            {
                int currentIndexPlayer = this.internalChessBoard.CurrentChessTurn.IndexPlayer;
                IPlayer currentInternalPlayer = this.internalChessBoard.Players[currentIndexPlayer];
                IPlayer currentPlayer = this.chessBoardsReferenceManager.GetOriginFromDestination(currentInternalPlayer);

                if (currentPlayer == this.ParentInterface.SupportedPlayer)
                {
                    ChessPiece concernedChessPiece = this.chessBoardsReferenceManager.GetDestinationFromOrigin(chessEvent.EventMove.ConcernedChessPiece);
                    if (concernedChessPiece.ChessPieceType == ChessPieceType.PAWN)
                    {
                        List<ChessPieceMovesContainer> possibleMoves = concernedChessPiece.GetAllPossibleMoves(this.internalChessBoard);

                        if (possibleMoves != null)
                        {
                            List<IChessPieceMove> promoteChessMoves = possibleMoves
                                .Select(pElem => pElem.ChessPieceMoves.FirstOrDefault())
                                .Where(pElem => pElem is PromoteChessPieceMove).ToList();

                            if (promoteChessMoves.Any())
                            {
                                ShiftChessPieceMove shiftChessPieceMove = (chessEvent.EventMove as ChessPieceMovesContainer).ChessPieceMoves.FirstOrDefault() as ShiftChessPieceMove;
                                this.createPossiblePromoteChessPiece2D(concernedChessPiece, shiftChessPieceMove.ToPosition, promoteChessMoves);
                            }
                        }
                    }
                }
            }
        }

        private void createPossiblePromoteChessPiece2D(ChessPiece promotingChessPiece, ChessPiecePosition toPosition, List<IChessPieceMove> promoteChessMoves)
        {
            this.parentChessBoard2D.ClearPossiblePromoteChessPiece2Ds();

            toPosition = ClampPositionToBoard(toPosition, promoteChessMoves.Count);
            Vector2f position = this.parentChessBoard2D.ConvertChessPositionTo2D(toPosition);

            this.promotingChessPiece = promotingChessPiece;

            int i = 0;
            float halfPromoteMoveCount = promoteChessMoves.Count / 2f;
            foreach (PromoteChessPieceMove promoteChessMove in promoteChessMoves)
            {
                this.parentChessBoard2D.AddPossiblePromoteChessPiece2Ds(promoteChessMove.ToChessType, new Vector2f(position.X + ChessBoard2D.MODEL_2_VIEW_X, position.Y + ChessBoard2D.MODEL_2_VIEW_Y * (i + 1)));
                i++;
            }
        }

        private ChessPiecePosition ClampPositionToBoard(ChessPiecePosition toPosition, int nbPromoteMoveCount)
        {
            ChessPiecePosition resultPosition = toPosition;

            if (resultPosition.X < 0)
            {
                resultPosition.X = 0;
            }
            else if(resultPosition.X > this.internalChessBoard.Width - 2)
            {
                resultPosition.X = this.internalChessBoard.Width - 2;
            }

            if(resultPosition.Y < 0)
            {
                resultPosition.Y = 0;
            }
            else if(resultPosition.Y + nbPromoteMoveCount > this.internalChessBoard.Height)
            {
                resultPosition.Y = 7 - nbPromoteMoveCount;
            }

            return resultPosition;
        }

        internal void OnMouseMoved(object sender, SFML.Window.MouseMoveEventArgs e)
        {
            if(this.selectedChessPiece != null)
            {
                Vector2f mousePosition = new Vector2f(e.X, e.Y);

                this.selectedChessPiece.Position = mousePosition;
            }
        }

        internal void OnMouseButtonReleased(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            if (e.Button == SFML.Window.Mouse.Button.Left)
            {
                if (this.selectedChessPiece != null)
                {
                    this.ReinitConcernedChessPiecePosition();

                    ChessPiecePosition toPosition = this.parentChessBoard2D.Convert2DToChessPosition(new Vector2f(e.X, e.Y));
                    this.ParentInterface.EnqueueChessMoveInfluence(this.selectedChessPiece.ChessPiece, new ShiftChessMoveInfluence(toPosition));

                    this.selectedChessPiece = null;
                }
                else if (this.parentChessBoard2D.AnyPossiblePromoteChessPiece2Ds())
                {
                    ChessPiece2D chessPiece2DSelected = this.parentChessBoard2D.GetPossiblePromoteChessPiece2DAt(new Vector2f(e.X, e.Y));

                    if(chessPiece2DSelected != null)
                    {
                        this.ParentInterface.EnqueueChessMoveInfluence(this.chessBoardsReferenceManager.GetOriginFromDestination(this.promotingChessPiece), new PromoteChessMoveInfluence(chessPiece2DSelected.ChessType));

                        this.promotingChessPiece = null;
                        this.parentChessBoard2D.ClearPossiblePromoteChessPiece2Ds();
                    }
                }
            }
        }

        internal void OnMouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            if (e.Button == SFML.Window.Mouse.Button.Left)
            {
                if (this.selectedChessPiece == null)
                {
                    Vector2f mousePosition = new Vector2f(e.X, e.Y);
                    ChessPiece2D chessPiece2D = this.parentChessBoard2D.GetChessPiece2DAt(new Vector2f(e.X, e.Y));

                    if (chessPiece2D != null
                        && chessPiece2D.ChessPiece.Owner == this.ParentInterface.SupportedPlayer)
                    {
                        chessPiece2D.Position = mousePosition;
                        this.selectedChessPiece = chessPiece2D;
                    }
                }
            }
            else if(e.Button == SFML.Window.Mouse.Button.Right)
            {
                this.ReinitConcernedChessPiecePosition();
                this.selectedChessPiece = null;
            }
        }

        private void ReinitConcernedChessPiecePosition()
        {
            this.selectedChessPiece.Position = this.parentChessBoard2D.GetChessPiece2DPositionFrom(this.selectedChessPiece.ChessPiece.ChessPiecePosition);
        }
    }
}
