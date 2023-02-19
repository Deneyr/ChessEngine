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

        public event Action ChessGameStarting;
        public event Action ChessGameStarted;

        public event Action<IPlayer> PlayerAddedToBoard;
        public event Action<ChessPiece> PieceAddedToBoard;

        /// <summary>
        /// Events raising order : MoveApplied + (NextTurnStarted)
        /// </summary>
        public event Action<ChessTurn> NextTurnStarted;
        /// <summary>
        /// Events raising order : (PreviousTurnStarted) + MoveReverted
        /// </summary>
        public event Action<ChessTurn> PreviousTurnStarted;

        /// <summary>
        /// Events raising order : MoveApplied + (NextTurnStarted)
        /// </summary>
        public event Action<ChessPieceMovesContainer> MoveApplied;
        /// <summary>
        /// Events raising order : (PreviousTurnStarted) + MoveReverted
        /// </summary>
        public event Action<ChessPieceMovesContainer> MoveReverted;

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public int PromoteBorderDistance
        {
            get;
            private set;
        }

        public List<ChessTurn> ChessTurns
        {
            get;
            private set;
        }

        public ChessTurn CurrentChessTurn
        {
            get
            {
                return this.ChessTurns.LastOrDefault();
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

        public bool IsTurnFirstMove
        {
            get
            {
                return this.CurrentChessTurn.TurnMoves.Any() == false;
            }
        }

        public HashSet<ChessPiece> ChessPiecesCemetery
        {
            get;
            private set;
        }

        public ChessBoard(int width = 8, int height = 8, int promoteBorderDistance = 4)
        {
            this.Players = new List<IPlayer>();
            this.ChessTurns = new List<ChessTurn>();

            this.PromoteBorderDistance = promoteBorderDistance;

            this.Width = width;
            this.Height = height;

            this.InitBoard();
        }

        public void InitGame()
        {
            this.Players.Clear();

            this.ChessTurns.Clear();

            this.ChessPiecesOnBoard.Clear();
            this.ChessPiecesCemetery.Clear();

            this.ChessGameStarting?.Invoke();
        }

        public void InitFirstTurn()
        {
            this.GoToNextTurn();

            this.ChessGameStarted?.Invoke();
        }

        public void AddPlayer(IPlayer playerToAdd)
        {
            this.Players.Add(playerToAdd);
            playerToAdd.ClearChessPieces();

            this.PlayerAddedToBoard?.Invoke(playerToAdd);
        }

        //public void RemovePlayer(IPlayer playerToRemove)
        //{
        //    this.Players.Add(playerToRemove);
        //    playerToRemove.ClearChessPieces();
        //}

        public void AddChessPiece(ChessPiece chessPieceToAdd)
        {
            chessPieceToAdd.HasAlreadyMoved = false;
            this.AddChessPieceTo(chessPieceToAdd, chessPieceToAdd.ChessPiecePosition);

            chessPieceToAdd.Owner.AddChessPieceToPlayer(chessPieceToAdd);

            this.PieceAddedToBoard?.Invoke(chessPieceToAdd);
        }

        //public void RemoveChessPiece(ChessPiece chessPieceToRemove)
        //{
        //    this.RemoveChessPieceFrom(chessPieceToRemove, chessPieceToRemove.ChessPiecePosition);

        //    chessPieceToRemove.Owner.RemoveChessPieceOfPlayer(chessPieceToRemove);
        //}

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
            return x >= 0 && x < this.Width
                && y >= 0 && y < this.Height;
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

        private bool ComputeSimulatedChessPieceMoves(ChessPieceMovesContainer chessPieceMove)
        {
            ChessTurn currentChessTurn = this.CurrentChessTurn;

            IPlayer currentPlayer = this.Players[currentChessTurn.IndexPlayer];

            if (chessPieceMove.ApplyMove(this))
            {
                currentChessTurn.TurnMoves.Add(chessPieceMove);

                if (currentChessTurn.IsTurnFinished)
                {
                    this.GoToNextSimulatedTurn();
                }

                return true;
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

                if (chessPieceMove.ApplyReverseMove(this))
                {
                    currentChessTurn.TurnMoves.RemoveAt(currentChessTurn.TurnMoves.Count - 1);

                    this.MoveReverted?.Invoke(chessPieceMove);

                    return true;
                }
            }

            return false;
        }

        private bool RevertLastSimulatedChessMove()
        {
            ChessTurn currentChessTurn = this.CurrentChessTurn;

            if (currentChessTurn.TurnMoves.Any() == false)
            {
                this.GoToPreviousSimulatedTurn();

                currentChessTurn = this.CurrentChessTurn;
            }

            if (currentChessTurn.TurnMoves.Any())
            {
                ChessPieceMovesContainer chessPieceMove = currentChessTurn.TurnMoves.Last();
                currentChessTurn.TurnMoves.RemoveAt(currentChessTurn.TurnMoves.Count - 1);

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

                    this.MoveApplied?.Invoke(chessPieceMove);

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
            int newTurnIndex = 0;

            ChessTurn currentChessTurn = this.CurrentChessTurn;

            if (currentChessTurn != null)
            {
                newTurnIndex = currentChessTurn.IndexPlayer + 1;
                if (newTurnIndex >= this.Players.Count)
                {
                    newTurnIndex = 0;
                }
            }

            ChessTurn newTurn = new ChessTurn(newTurnIndex);
            this.ChessTurns.Add(newTurn);

            IPlayer currentPlayer = this.Players[this.CurrentChessTurn.IndexPlayer];

            newTurn.IsCurrentKingChecked = this.IsKingChecked(currentPlayer.KingChessPiece);
            newTurn.CanPlayerMoveChessPieces = this.CanPlayerMoveChessPieces();

            this.NextTurnStarted?.Invoke(currentChessTurn);
        }

        private void GoToNextSimulatedTurn()
        {
            int newTurnIndex = 0;

            ChessTurn currentChessTurn = this.CurrentChessTurn;

            if (currentChessTurn != null)
            {
                newTurnIndex = currentChessTurn.IndexPlayer + 1;
                if (newTurnIndex >= this.Players.Count)
                {
                    newTurnIndex = 0;
                }
            }

            ChessTurn newTurn = new ChessTurn(newTurnIndex);
            this.ChessTurns.Add(newTurn);
        }

        private void GoToPreviousTurn()
        {
            if (this.ChessTurns.Count > 1)
            {
                ChessTurn currentChessTurn = this.CurrentChessTurn;

                this.ChessTurns.RemoveAt(this.ChessTurns.Count - 1);

                this.PreviousTurnStarted?.Invoke(currentChessTurn);
            }
        }

        private void GoToPreviousSimulatedTurn()
        {
            if (this.ChessTurns.Count > 1)
            {
                ChessTurn currentChessTurn = this.CurrentChessTurn;

                this.ChessTurns.RemoveAt(this.ChessTurns.Count - 1);
            }
        }

        private bool IsKingChecked(ChessPiece king)
        {
            if (king != null)
            {
                ChessTurn currentChessTurn = this.CurrentChessTurn;

                IEnumerable<ChessPiece> otherPlayersChessPieces = this.ChessPiecesOnBoard.Where(pElem => pElem.Owner != king.Owner);
                foreach (ChessPiece chessPiece in otherPlayersChessPieces)
                {
                    // Simulate a "fake" turn to get the all possible moves of the chessPiece inspected 
                    this.ChessTurns.Add(new ChessTurn(this.Players.IndexOf(chessPiece.Owner)));

                    List<ChessPieceMovesContainer> possibleMoves = chessPiece.GetAllPossibleMovesWithoutCheckRestriction(this);

                    foreach (ChessPieceMovesContainer move in possibleMoves)
                    {
                        IChessPieceMove takeKingMove = move.ChessPieceMoves.FirstOrDefault(
                            pElem => pElem is KillChessPieceMove
                            && pElem.ConcernedChessPiece == king);

                        if (takeKingMove != null)
                        {
                            // Remove the simulated turn
                            this.ChessTurns.RemoveAt(this.ChessTurns.Count - 1);

                            return true;
                        }
                    }

                    // Remove the simulated turn
                    this.ChessTurns.RemoveAt(this.ChessTurns.Count - 1);
                }
            }
            return false;
        }

        private bool CanPlayerMoveChessPieces()
        {
            IPlayer currentPlayer = this.Players[this.CurrentChessTurn.IndexPlayer];

            foreach (ChessPiece chessPiece in currentPlayer.ChessPiecesOwned)
            {
                List<ChessPieceMovesContainer> possibleMoves = chessPiece.GetAllPossibleMoves(this);

                if (possibleMoves.Any())
                {
                    return true;
                }
            }

            return false;
        }

        internal bool IsGivenMovesGetChecked(ChessPieceMovesContainer move)
        {
            bool lResult = false;
            ChessPiece king = this.Players[this.CurrentChessTurn.IndexPlayer].KingChessPiece;

            bool isMoveEndTurn = move.IsEndTurn;
            move.IsEndTurn = true;

            if (king != null && this.ComputeSimulatedChessPieceMoves(move))
            {
                lResult = this.IsKingChecked(king);

                this.RevertLastSimulatedChessMove();
            }

            move.IsEndTurn = isMoveEndTurn;

            return lResult;
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
            this.chessBoard = new ChessPieceCell[this.Height, this.Width];
            for(int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    this.chessBoard[i, j] = new ChessPieceCell();
                }
            }

            this.ChessPiecesOnBoard = new HashSet<ChessPiece>();
            this.ChessPiecesCemetery = new HashSet<ChessPiece>();
        }
    }
}
