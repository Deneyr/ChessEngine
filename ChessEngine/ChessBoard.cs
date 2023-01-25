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
    public class ChessBoard
    {
        private static ChessPieceFactory sChessPieceFactory = new ChessPieceFactory();

        private ChessPieceCell[,] chessBoard;

        private int width;
        private int height;

        public List<ChessTurn> ChessTurns
        {
            get;
            private set;
        }

        public ChessTurn CurrentChessTurn
        {
            get
            {
                return this.ChessTurns.Last();
            }
        }

        public List<IPlayer> Players
        {
            get;
            private set;
        }

        public HashSet<ChessPiece> ChessPiecesOnBoard
        {
            get;
            private set;
        }

        public HashSet<ChessPiece> ChessPiecesCemetery
        {
            get;
            private set;
        }

        public ChessBoard(int width = 8, int height = 8)
        {
            this.Players = new List<IPlayer>();
            this.ChessTurns = new List<ChessTurn>();

            this.width = width;
            this.height = height;

            this.InitBoard();
        }

        //public ChessPiecePosition ClampChessPiecePositionToBoard(ChessPiecePosition chessPiecePosition)
        //{
        //    if(chessPiecePosition.X < 0)
        //    {
        //        chessPiecePosition.X = 0;
        //    }
        //    else if(chessPiecePosition.X >= this.width)
        //    {
        //        chessPiecePosition.X = this.width - 1;
        //    }

        //    if (chessPiecePosition.Y < 0)
        //    {
        //        chessPiecePosition.Y = 0;
        //    }
        //    else if (chessPiecePosition.Y >= this.height)
        //    {
        //        chessPiecePosition.Y = this.height - 1;
        //    }

        //    return chessPiecePosition;
        //}

        public void InitGame()
        {
            this.Players.Clear();

            this.ChessTurns.Clear();
            this.ChessTurns.Add(new ChessTurn(0));

            this.ChessPiecesOnBoard.Clear();
            this.ChessPiecesCemetery.Clear();
        }

        public void AddPlayer(IPlayer playerToAdd)
        {
            this.Players.Add(playerToAdd);
            playerToAdd.ClearChessPieces();
        }

        public void RemovePlayer(IPlayer playerToRemove)
        {
            this.Players.Add(playerToRemove);
            playerToRemove.ClearChessPieces();
        }

        public void AddChessPiece(ChessPiece chessPieceToAdd)
        {
            chessPieceToAdd.HasAlreadyMoved = false;
            this.AddChessPieceTo(chessPieceToAdd, chessPieceToAdd.ChessPiecePosition);

            chessPieceToAdd.Owner.AddChessPieceToPlayer(chessPieceToAdd);
        }

        public void RemoveChessPiece(ChessPiece chessPieceToRemove)
        {
            this.RemoveChessPieceFrom(chessPieceToRemove, chessPieceToRemove.ChessPiecePosition);

            chessPieceToRemove.Owner.RemoveChessPieceOfPlayer(chessPieceToRemove);
        }

        public ChessPiece CreateChessPiece(IPlayer owner, ChessPieceType chessPieceType, ChessPiecePosition chessPiecePosition)
        {
            return sChessPieceFactory.CreateChessPiece(owner, chessPieceType, chessPiecePosition);
        }

        public bool IsPositionOnChessBoard(ChessPiecePosition chessPiecePosition)
        {
            return this.IsPositionOnChessBoard(chessPiecePosition.X, chessPiecePosition.Y);
        }

        public bool IsPositionOnChessBoard(int x, int y)
        {
            return x >= 0 && x < this.width
                && y >= 0 && y < this.height;
        }

        public ChessPiece GetChessPieceAt(ChessPiecePosition chessPiecePosition)
        {
            return this.GetChessPieceAt(chessPiecePosition.X, chessPiecePosition.Y);
        }

        public ChessPiece GetChessPieceAt(int x, int y)
        {
            if (this.IsPositionOnChessBoard(x, y))
            {
                return this.chessBoard[y, x].mainChessPiece;
            }
            return null;
        }

        public int RayTrace(ChessPiecePosition fromPosition, int shiftX, int shiftY, int maxStep, out ChessPiece chessPieceTouched)
        {
            chessPieceTouched = null;

            fromPosition.X += shiftX;
            fromPosition.Y += shiftY;

            int i = 0;
            while (i < maxStep
                && this.IsPositionOnChessBoard(fromPosition)
                && (chessPieceTouched = this.GetChessPieceAt(fromPosition)) == null)
            {
                fromPosition.X += shiftX;
                fromPosition.Y += shiftY;

                ++i;
            }

            return i;
        }

        public bool ComputeChessPieceInfluence(ChessPiece chessPiece, IChessMoveInfluence chessMoveInfluence)
        {
            ChessTurn currentChessTurn = this.CurrentChessTurn;

            IPlayer currentPlayer = this.Players[currentChessTurn.IndexPlayer];

            if(currentPlayer == chessPiece.Owner)
            {
                if(chessPiece.CreateMove(this, chessMoveInfluence, out ChessPieceMovesContainer chessPieceMove))
                {
                    return this.ApplyChessMove(currentChessTurn, chessPieceMove);
                }
            }

            return false;
        }

        public bool ComputeChessPieceMoves(ChessPieceMovesContainer chessPieceMove)
        {
            ChessTurn currentChessTurn = this.CurrentChessTurn;

            IPlayer currentPlayer = this.Players[currentChessTurn.IndexPlayer];

            if (currentPlayer == chessPieceMove.ConcernedChessPiece.Owner)
            {
                return this.ApplyChessMove(currentChessTurn, chessPieceMove);
            }

            return false;
        }

        public bool RevertLastChessMove()
        {
            ChessTurn currentChessTurn = this.CurrentChessTurn;

            if (currentChessTurn.TurnMoves.Any() == false)
            {
                this.GoToPreviousTurn();

                currentChessTurn = this.CurrentChessTurn;
            }

            if (currentChessTurn.TurnMoves.Any())
            {
                ChessPieceMovesContainer chessPieceMove = currentChessTurn.TurnMoves.Last();

                return chessPieceMove.ApplyReverseMove(this);
            }

            return false;
        }

        private bool ApplyChessMove(ChessTurn currentChessTurn, ChessPieceMovesContainer chessPieceMove)
        {
            if (currentChessTurn.IsTurnFinished == false)
            {
                if (chessPieceMove.ApplyMove(this))
                {
                    currentChessTurn.TurnMoves.Add(chessPieceMove);

                    if (currentChessTurn.IsTurnFinished)
                    {
                        this.GoToNextTurn();
                    }

                    return true;
                }
            }

            return false;
        }

        private void GoToNextTurn()
        {
            ChessTurn currentChessTurn = this.CurrentChessTurn;

            int newTurnIndex = currentChessTurn.IndexPlayer + 1;
            if(newTurnIndex >= this.Players.Count)
            {
                newTurnIndex = 0;
            }

            this.ChessTurns.Add(new ChessTurn(newTurnIndex));
        }

        private void GoToPreviousTurn()
        {
            ChessTurn currentChessTurn = this.CurrentChessTurn;

            int newTurnIndex = currentChessTurn.IndexPlayer - 1;
            if (newTurnIndex >= 0)
            {
                this.ChessTurns.RemoveAt(currentChessTurn.IndexPlayer);
            }
        }

        internal void MoveChessPieceTo(ChessPiece chessPiece, ChessPiecePosition chessPiecePosition)
        {
            this.RemoveChessPieceFrom(chessPiece, chessPiece.ChessPiecePosition);

            this.AddChessPieceTo(chessPiece, chessPiecePosition);
        }

        internal void KillChessPiece(ChessPiece chessPiece)
        {
            this.MoveChessPieceTo(chessPiece, new ChessPiecePosition(-1, -1));
        }

        internal void ResurrectChessPiece(ChessPiece chessPiece, ChessPiecePosition chessPiecePosition)
        {
            this.MoveChessPieceTo(chessPiece, chessPiecePosition);
        }

        internal void PromoteChessPiece(ChessPiece chessPiece, ChessPieceType promoteToChessType)
        {
            sChessPieceFactory.TransmuteChessPiece(promoteToChessType, chessPiece);
        }

        private void AddChessPieceTo(ChessPiece chessPiece, ChessPiecePosition chessPiecePosition)
        {
            this.AddChessPieceTo(chessPiece, chessPiecePosition.X, chessPiecePosition.Y);
        }

        private void AddChessPieceTo(ChessPiece chessPieceToAdd, int x, int y)
        {
            if (this.IsPositionOnChessBoard(x, y))
            {
                chessPieceToAdd.ChessPiecePosition = new ChessPiecePosition(x, y);

                this.ChessPiecesOnBoard.Add(chessPieceToAdd);

                this.chessBoard[y, x].AddChessPieceToCell(chessPieceToAdd);
            }
            else
            {
                chessPieceToAdd.ChessPiecePosition = new ChessPiecePosition(-1, -1);

                this.ChessPiecesCemetery.Add(chessPieceToAdd);
            }
        }

        private void RemoveChessPieceFrom(ChessPiece chessPiece, ChessPiecePosition chessPiecePosition)
        {
            this.RemoveChessPieceFrom(chessPiece, chessPiecePosition.X, chessPiecePosition.Y);
        }

        private void RemoveChessPieceFrom(ChessPiece chessPieceToRemove, int x, int y)
        {
            if (this.IsPositionOnChessBoard(x, y))
            {
                chessPieceToRemove.ChessPiecePosition = new ChessPiecePosition(-1, -1);

                this.ChessPiecesOnBoard.Remove(chessPieceToRemove);

                this.chessBoard[y, x].RemoveChessPieceOfCell(chessPieceToRemove);
            }
            else
            {
                chessPieceToRemove.ChessPiecePosition = new ChessPiecePosition(-1, -1);

                this.ChessPiecesCemetery.Remove(chessPieceToRemove);
            }
        }

        private void InitBoard()
        {
            this.chessBoard = new ChessPieceCell[this.height, this.width];
            for(int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    this.chessBoard[i, j] = new ChessPieceCell();
                }
            }

            this.ChessPiecesOnBoard = new HashSet<ChessPiece>();
            this.ChessPiecesCemetery = new HashSet<ChessPiece>();
        }
    }
}
