using ChessEngine.ChessModels.Monitors;
using ChessEngine.Maths;
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

        public ChessPlayerHandler(ChessBoard2D parentChessBoard2D)
        {
            this.parentChessBoard2D = parentChessBoard2D;

            this.selectedChessPiece = null;
        }

        protected override void InternalHandleChessEvent(ChessEvent chessEvent)
        {

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
