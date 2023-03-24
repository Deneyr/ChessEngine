using ChessAI.AStarAI;
using ChessAI.RandomAI;
using ChessEngine;
using ChessEngine.ChessModels.Monitors;
using ChessEngine.Maths;
using ChessEngine.Players;
using ChessInterface;
using ChessInterface.Handlers;
using ChessView.View;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessView
{
    public class MainWindow
    {
        private FloatRect boundsView;

        private ChessBoard chessBoard;

        private ChessBoard2D chessBoard2D;

        private ChessBoardInterface playerInterface1;
        private ChessBoardInterface playerInterface2;

        public MainWindow()
        {
            this.chessBoard = new ChessBoard();

            this.chessBoard2D = new ChessBoard2D(this.chessBoard);
        }

        public void Run()
        {
            Vector2u chessBoardSize = this.chessBoard2D.ChessBoardSize;

            var mode = new VideoMode(chessBoardSize.X, chessBoardSize.Y);
            var window = new RenderWindow(mode, "Chess view");
            window.SetVerticalSyncEnabled(false);

            this.InitChessBoardGame(window, this.chessBoard);
            this.chessBoard.InitFirstTurn();

            window.KeyPressed += Window_KeyPressed;

            //window.MouseButtonPressed += OnMouseButtonPressed;
            //window.MouseButtonReleased += OnMouseButtonReleased;
            //window.MouseMoved += OnMouseMoved;

            Clock clock = new Clock();

            // Start the game loop
            while (window.IsOpen)
            {
                Time deltaTime = clock.Restart();
                // Game logic update

                // Draw window
                AObject2D.UpdateAnimationManager(deltaTime);

                this.playerInterface1.UpdateInterface(deltaTime.AsSeconds());
                this.playerInterface2.UpdateInterface(deltaTime.AsSeconds());

                window.Clear();

                this.chessBoard2D.DrawIn(window, deltaTime);

                // Process events
                window.DispatchEvents();

                // Finally, display the rendered frame on screen
                window.Display();
            }

            this.chessBoard2D.Dispose(this.chessBoard);
        }       

        private void SetView(RenderWindow window, SFML.Graphics.View view)
        {
            this.boundsView = new FloatRect(view.Center.X - view.Size.X / 2, view.Center.Y - view.Size.Y / 2, view.Size.X, view.Size.Y);

            window.SetView(view);
        }

        private void InitChessBoardGame(RenderWindow window, ChessBoard chessBoard)
        {
            AStarAI handler = new AStarAI();
            //RandomAIHandler handler = new RandomAIHandler();
            ChessPlayerHandler chessPlayerHandler = new ChessPlayerHandler(this.chessBoard2D);

            this.playerInterface2 = new ChessBoardInterface(handler, 1f);
            this.playerInterface1 = new ChessBoardInterface(chessPlayerHandler, 1f);

            window.MouseButtonPressed += chessPlayerHandler.OnMouseButtonPressed;
            window.MouseButtonReleased += chessPlayerHandler.OnMouseButtonReleased;
            window.MouseMoved += chessPlayerHandler.OnMouseMoved;

            this.playerInterface1.RegisterChessBoard(chessBoard);
            this.playerInterface2.RegisterChessBoard(chessBoard);

            chessBoard.InitGame();

            // Players
            Player player1 = new Player("White", 0, -1);
            Player player2 = new Player("Black", 0, 1);

            chessBoard.AddPlayer(player1);
            chessBoard.AddPlayer(player2);

            ChessPiece chessPiece;

            // Chess pieces player 1
            // Position is compute from the top left corner of the board, starting from 0
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.PAWN, new ChessPiecePosition(0, 6));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.PAWN, new ChessPiecePosition(1, 6));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.PAWN, new ChessPiecePosition(2, 6));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.PAWN, new ChessPiecePosition(3, 6));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.PAWN, new ChessPiecePosition(4, 6));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.PAWN, new ChessPiecePosition(5, 6));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.PAWN, new ChessPiecePosition(6, 6));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.PAWN, new ChessPiecePosition(7, 6));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.ROOK, new ChessPiecePosition(0, 7));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.KNIGHT, new ChessPiecePosition(1, 7));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.BISHOP, new ChessPiecePosition(2, 7));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.QUEEN, new ChessPiecePosition(3, 7));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.KING, new ChessPiecePosition(4, 7));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.BISHOP, new ChessPiecePosition(5, 7));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.KNIGHT, new ChessPiecePosition(6, 7));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.ROOK, new ChessPiecePosition(7, 7));
            chessBoard.AddChessPiece(chessPiece);

            // Chess pieces player 2
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.PAWN, new ChessPiecePosition(0, 1));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.PAWN, new ChessPiecePosition(1, 1));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.PAWN, new ChessPiecePosition(2, 1));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.PAWN, new ChessPiecePosition(3, 1));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.PAWN, new ChessPiecePosition(4, 1));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.PAWN, new ChessPiecePosition(5, 1));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.PAWN, new ChessPiecePosition(6, 1));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.PAWN, new ChessPiecePosition(7, 1));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.ROOK, new ChessPiecePosition(0, 0));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.KNIGHT, new ChessPiecePosition(1, 0));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.BISHOP, new ChessPiecePosition(2, 0));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.QUEEN, new ChessPiecePosition(3, 0));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.KING, new ChessPiecePosition(4, 0));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.BISHOP, new ChessPiecePosition(5, 0));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.KNIGHT, new ChessPiecePosition(6, 0));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.ROOK, new ChessPiecePosition(7, 0));
            chessBoard.AddChessPiece(chessPiece);

            this.playerInterface1.SupportedPlayer = chessBoard.Players[0];
            this.playerInterface2.SupportedPlayer = chessBoard.Players[1];
        }

        private void TestInitChessBoardGame(RenderWindow window, ChessBoard chessBoard)
        {
            AStarAI handler = new AStarAI();
            //RandomAIHandler handler = new RandomAIHandler();
            ChessPlayerHandler chessPlayerHandler = new ChessPlayerHandler(this.chessBoard2D);

            this.playerInterface2 = new ChessBoardInterface(handler, 1f);
            this.playerInterface1 = new ChessBoardInterface(chessPlayerHandler, 1f);

            window.MouseButtonPressed += chessPlayerHandler.OnMouseButtonPressed;
            window.MouseButtonReleased += chessPlayerHandler.OnMouseButtonReleased;
            window.MouseMoved += chessPlayerHandler.OnMouseMoved;

            this.playerInterface1.RegisterChessBoard(chessBoard);
            this.playerInterface2.RegisterChessBoard(chessBoard);

            chessBoard.InitGame();

            // Players
            Player player1 = new Player("White", 0, -1);
            Player player2 = new Player("Black", 0, 1);

            chessBoard.AddPlayer(player1);
            chessBoard.AddPlayer(player2);

            ChessPiece chessPiece;

            // Chess pieces player 1
            // Position is compute from the top left corner of the board, starting from 0
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.KNIGHT, new ChessPiecePosition(3, 4));
            chessBoard.AddChessPiece(chessPiece);

            // Chess pieces player 2
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.BISHOP, new ChessPiecePosition(2, 2));
            chessBoard.AddChessPiece(chessPiece);

            //chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.BISHOP, new ChessPiecePosition(1, 5));
            //chessBoard.AddChessPiece(chessPiece);

            this.playerInterface1.SupportedPlayer = chessBoard.Players[0];
            this.playerInterface2.SupportedPlayer = chessBoard.Players[1];
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                window.Close();
            }
            //else if(e.Code == SFML.Window.Keyboard.Key.Right)
            //{
            //    ChessPiece chessPiece = this.chessBoard.Players[0].ChessPiecesOwned[0];
            //    ChessPiece chessPiece2 = this.chessBoard.Players[1].ChessPiecesOwned[1];

            //    IChessMoveInfluence moveInfluence = new ShiftChessMoveInfluence(new ChessPiecePosition(3, 7));

            //    chessBoard.ComputeChessPieceInfluence(chessPiece, moveInfluence);
            //}
        }
    }
}
