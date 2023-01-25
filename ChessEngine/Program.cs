using ChessEngine.ChessModels.Monitors;
using ChessEngine.Maths;
using ChessEngine.Moves;
using ChessEngine.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            ChessBoard chessBoard = new ChessBoard();
            InitChessBoardGame(chessBoard);

            ChessPiece chessPiece = chessBoard.Players.First().ChessPiecesOwned[0];

            IChessMoveInfluence moveInfluence = new ShiftChessMoveInfluence(new ChessPiecePosition(6, 1));
            bool result = chessPiece.CreateMove(chessBoard, moveInfluence, out ChessPieceMovesContainer move);
            List<ChessPieceMovesContainer> moves = chessPiece.GetAllPossibleMoves(chessBoard);

            chessBoard.ComputeChessPieceInfluence(chessPiece, moveInfluence);

            chessBoard.RevertLastChessMove();

            Console.WriteLine(moves);
        }

        static private void InitChessBoardGame(ChessBoard chessBoard)
        {
            chessBoard.InitGame();

            // Players
            Player player1 = new Player("White");
            Player player2 = new Player("Black");

            chessBoard.AddPlayer(player1);
            chessBoard.AddPlayer(player2);

            ChessPiece chessPiece;

            // Chess pieces player 1
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.BISHOP, new ChessPiecePosition(4, 3));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.KNIGHT, new ChessPiecePosition(4, 4));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.ROOK, new ChessPiecePosition(6, 6));
            chessBoard.AddChessPiece(chessPiece);

            // Chess pieces player 2
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.BISHOP, new ChessPiecePosition(6, 1));
            chessBoard.AddChessPiece(chessPiece);
        }
    }
}
