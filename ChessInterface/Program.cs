using ChessEngine;
using ChessEngine.ChessModels.Monitors;
using ChessEngine.Maths;
using ChessEngine.Moves;
using ChessEngine.Players;
using ChessInterface.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            TestAsyncHandler handler = new TestAsyncHandler();
            ChessBoardInterface chessBoardInterface = new ChessBoardInterface(handler);

            ChessBoard chessBoard = new ChessBoard();
            chessBoardInterface.RegisterChessBoard(chessBoard);

            InitChessBoardGame(chessBoard);
            chessBoardInterface.SupportedPlayer = chessBoard.Players[0];

            ChessPiece chessPiece = chessBoard.Players[0].ChessPiecesOwned[0];
            ChessPiece chessPiece2 = chessBoard.Players[1].ChessPiecesOwned[1];

            IChessMoveInfluence moveInfluence = new ShiftChessMoveInfluence(new ChessPiecePosition(3, 7));
            bool result = chessPiece.CreateMove(chessBoard, moveInfluence, out ChessPieceMovesContainer move);
            List<ChessPieceMovesContainer> moves = chessPiece.GetAllPossibleMoves(chessBoard);

            chessBoard.ComputeChessPieceInfluence(chessPiece, moveInfluence);

            Console.WriteLine(chessBoardInterface);
            Console.Read();
        }

        static private void InitChessBoardGame(ChessBoard chessBoard)
        {
            chessBoard.InitGame();

            // Players
            Player player1 = new Player("White", 0, -1);
            Player player2 = new Player("Black", 0, 1);

            chessBoard.AddPlayer(player1);
            chessBoard.AddPlayer(player2);

            ChessPiece chessPiece;

            // Chess pieces player 1
            // Position is compute from the top left corner of the board, starting from 0
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.BISHOP, new ChessPiecePosition(5, 5));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.KNIGHT, new ChessPiecePosition(3, 3));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.ROOK, new ChessPiecePosition(7, 4));
            chessBoard.AddChessPiece(chessPiece);

            // Chess pieces player 2
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.ROOK, new ChessPiecePosition(7, 0));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.KING, new ChessPiecePosition(4, 0));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.ROOK, new ChessPiecePosition(0, 0));
            chessBoard.AddChessPiece(chessPiece);

            chessBoard.InitFirstTurn();
        }
    }
}
