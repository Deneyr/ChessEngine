using ChessEngine;
using ChessEngine.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.AStarAI.MoveTree
{
    public class MoveRoot : MoveComposite
    {
        private AMoveComponent lastStudiedMove;

        // TEST
        //private int counter;

        protected override MoveRoot Root
        {
            get
            {
                return this;
            }
            set
            {
                // Nothing to do
            }
        }

        public ChessBoard MainChessBoard
        {
            get;
            private set;
        }

        public int CurrentCounter
        {
            get;
            set;
        }

        public int MaxCounter
        {
            get;
            private set;
        }

        public int MaxDepth
        {
            get;
            private set;
        }

        public bool IsMoveStudying
        {
            get
            {
                return this.moveComponentStudied.Any();
            }
        }

        private SortedList<float, AMoveComponent> moveComponentStudied;

        public MoveRoot(ChessBoard chessBoard, int maxCounter, int maxDepth)
            : base(-1, null, null, null)
        {
            this.MainChessBoard = chessBoard;

            this.MaxCounter = maxCounter;
            this.MaxDepth = maxDepth;

            this.CurrentCounter = 0;
            this.lastStudiedMove = null;

            this.moveComponentStudied = new SortedList<float, AMoveComponent>(new DecendingComparer());
        }

        internal void InitRoot()
        {
            this.CurrentCounter = this.MaxCounter;
            this.lastStudiedMove = null;

            this.moveComponentStudied.Clear();
            this.moveComponentStudied.Add(0, this);

            //this.counter = 0;
        }

        internal void cleanRoot()
        {
            this.TravelChessBoardTo(this);
        }

        internal void AddMoveComponentToStudied(AMoveComponent child)
        {
            this.MainChessBoard.ComputeChessPieceMoves(child.Command);

            child.ComputeEnteringFitness();

            this.moveComponentStudied.Add(this.GetPotentialFitnessFrom(child), child);

            this.MainChessBoard.RevertLastChessMove();
        }

        public bool StudyMostPotentialMove()
        {
            if (this.moveComponentStudied.Any())
            {
                AMoveComponent moveToStudy = this.moveComponentStudied.First().Value;
                this.moveComponentStudied.RemoveAt(0);

                this.TravelChessBoardTo(moveToStudy);

                moveToStudy.StudyMove();

                return true;
            }
            return false;
        }

        private void TravelChessBoardTo(AMoveComponent toMove)
        {
            if(this.lastStudiedMove != null)
            {
                ChessBoard chessBoard = this.MainChessBoard;

                Stack<ChessPieceMovesContainer> movesToApply = new Stack<ChessPieceMovesContainer>();
                int lastStudiedDepthDiff = this.lastStudiedMove.Depth - toMove.Depth;
                int toMoveDepthDiff = toMove.Depth - this.lastStudiedMove.Depth;

                AMoveComponent currentLastStudiedMove = this.lastStudiedMove;
                for (int i = 0; i < lastStudiedDepthDiff; i++)
                {
                    // this.counter--;
                    chessBoard.RevertLastChessMove();
                    currentLastStudiedMove = currentLastStudiedMove.Parent;
                }
                AMoveComponent currentToMove = toMove;
                for (int i = 0; i < toMoveDepthDiff; i++)
                {
                    movesToApply.Push(currentToMove.Command);
                    currentToMove = currentToMove.Parent;
                }

                while(currentToMove != currentLastStudiedMove)
                {
                    // this.counter--;
                    chessBoard.RevertLastChessMove();
                    movesToApply.Push(currentToMove.Command);

                    currentLastStudiedMove = currentLastStudiedMove.Parent;
                    currentToMove = currentToMove.Parent;
                }

                while(movesToApply.Any())
                {
                    // this.counter++;
                    if (chessBoard.ComputeChessPieceMoves(movesToApply.Pop()) == false)
                    {
                        throw new Exception("Anticipated move computing failure");
                    }
                }

            }
            this.lastStudiedMove = toMove;
        }

        private float GetPotentialFitnessFrom(AMoveComponent moveComponent)
        {
            return moveComponent.PreFitnessForCurrentPlayer - moveComponent.Depth * 0.3f;
        }

        private class DecendingComparer : IComparer<float>
        {
            public int Compare(float x, float y)
            {
                int result = y.CompareTo(x);

                return result == 0 ? 1 : result;
            }
        }
    }
}
