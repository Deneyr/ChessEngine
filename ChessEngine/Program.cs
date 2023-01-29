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

            ChessPiece chessPiece = chessBoard.Players[0].ChessPiecesOwned[2];
            ChessPiece chessPiece2 = chessBoard.Players[1].ChessPiecesOwned[0];

            IChessMoveInfluence moveInfluence = new ShiftChessMoveInfluence(new ChessPiecePosition(3, 0));
            bool result = chessPiece.CreateMove(chessBoard, moveInfluence, out ChessPieceMovesContainer move);
            List<ChessPieceMovesContainer> moves = chessPiece.GetAllPossibleMoves(chessBoard);

            chessBoard.ComputeChessPieceInfluence(chessPiece, moveInfluence);

            moves = chessPiece.GetAllPossibleMoves(chessBoard);

            chessBoard.RevertLastChessMove();
            chessBoard.ComputeChessPieceInfluence(chessPiece, moveInfluence);

            moves = chessPiece2.GetAllPossibleMoves(chessBoard);

            Console.WriteLine(moves);
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
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.BISHOP, new ChessPiecePosition(5, 5));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.KNIGHT, new ChessPiecePosition(4, 4));
            chessBoard.AddChessPiece(chessPiece);
            chessPiece = chessBoard.CreateChessPiece(player1, ChessPieceType.PAWN, new ChessPiecePosition(3, 2));
            chessBoard.AddChessPiece(chessPiece);

            // Chess pieces player 2
            chessPiece = chessBoard.CreateChessPiece(player2, ChessPieceType.PAWN, new ChessPiecePosition(2, 4));
            chessBoard.AddChessPiece(chessPiece);
        }
    }
}
