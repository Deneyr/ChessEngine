using ChessEngine;
using ChessEngine.Moves;
using ChessEngine.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.BruteForceAI.MoveTree
{
    public class MoveComposite : AMoveComponent
    {
        protected List<AMoveComponent> mChildren;

        protected int mCrossIndex;

        protected AMoveComponent mBestChild;

        public MoveComposite(int currentIndexPlayer, MoveComposite pParent, ChessPieceMovesContainer pCommand) 
            : base(currentIndexPlayer, pParent, pCommand)
        {
            this.mChildren = null;

            this.mCrossIndex = 0;
        }

        public List<AMoveComponent> Children { get => mChildren; }
        public int CrossIndex { get => mCrossIndex; }
        public override AMoveComponent BestChild { get => mBestChild; }

        public override void CreateChildren(ChessBoard chessBoard)
        {
            if(this.Children != null)
            {
                return;
            }

            this.mChildren = new List<AMoveComponent>();

            int childrenIndexPlayer = chessBoard.CurrentChessTurn.IndexPlayer;
            IPlayer childrenInternalPlayer = chessBoard.Players[childrenIndexPlayer];

            foreach(ChessPiece chessPiece in childrenInternalPlayer.ChessPiecesOwned)
            {
                if (chessPiece.IsAlive)
                {
                    List<ChessPieceMovesContainer> allPossibleMoves = chessPiece.GetAllPossibleMoves(chessBoard);
                    foreach(ChessPieceMovesContainer move in allPossibleMoves)
                    {
                        //chessBoard.ComputeChessPieceMoves(move);

                        AMoveComponent child = this.CreateChildMoveComponent(childrenIndexPlayer, move, chessBoard);

                        //child.ComputeEnteringFitness(chessBoard);

                        //chessBoard.RevertLastChessMove();

                        this.mChildren.Add(child);
                    }
                }
            }

            this.SortChildren();
        }

        public override AMoveComponent GetNextChild()
        {
            if(this.mCrossIndex < this.mChildren.Count)
            {
                return this.mChildren.ElementAt(this.mCrossIndex++);
            }
            return null;
        }

        public override void ComputeFitnessAndClean(ChessBoard chessBoard)
        {
            if (this.mChildren.Count == 0)
            {
                this.ComputeEnteringFitness(chessBoard);
                this.PostFitnessComputed = true;
                return;
            }

            float lTotalFitnessChildrenPlayer = 0;
            bool isFirstChild = true;
            AMoveComponent lBestComponent = null;

            foreach (AMoveComponent lChild in this.mChildren)
            {
                float fitness = lChild.PostFitnessForCurrentPlayer;
                if (isFirstChild || fitness > lTotalFitnessChildrenPlayer)
                {
                    lTotalFitnessChildrenPlayer = fitness;
                    lBestComponent = lChild;
                    isFirstChild = false;
                }
            }

            //this.postFitnessesByPlayers = lBestComponent.CopyPostFitnesses(out float postTotalFitnesses);
            //this.postTotalFistnesses = postTotalFitnesses;

            this.fitnessForFirstPlayer = lBestComponent.PostFitnessForFirstPlayer;

            this.mChildren.Clear();

            this.mBestChild = lBestComponent;
            this.PostFitnessComputed = true;
        }

        protected virtual void SortChildren()
        {
            this.mChildren.Sort(new MoveComponentComparer());
        }

        protected override void PruneAllChildrenExcept(AMoveComponent moveComponent)
        {
            this.mChildren.Clear();
            this.mChildren.Add(moveComponent);
            this.mCrossIndex = 1;
        }

        protected override bool IsPruningAvailableFor(float fitnessFirstPlayer)
        {
            IEnumerable<AMoveComponent> childrenComputed = this.Children.Where(pElem => pElem.PostFitnessComputed);
            bool isFirstChild = true;
            float trueFitness = fitnessFirstPlayer;

            foreach(AMoveComponent childComputed in childrenComputed)
            {
                if (isFirstChild)
                {
                    trueFitness = childComputed.GetFitnessForCurrentPlayer(fitnessFirstPlayer);
                    isFirstChild = false;
                }

                if(childComputed.PostFitnessForCurrentPlayer >= trueFitness)
                {
                    return true;
                }
            }
            return false;
        }

        private AMoveComponent CreateChildMoveComponent(int childrenIndexPlayer, ChessPieceMovesContainer move, ChessBoard chessBoard)
        {
            AMoveComponent child = null;
            bool isGameFinished = false;//chessBoard.CurrentChessTurn.CanPlayerMoveChessPieces == false;
            if (isGameFinished
                || this.Depth > 2)
            {
                child = new MoveLeaf(childrenIndexPlayer, this, move);
            }
            else
            {
                child = new MoveComposite(childrenIndexPlayer, this, move);
            }

            return child;
        }

        private class MoveComponentComparer : IComparer<AMoveComponent>
        {
            public int Compare(AMoveComponent x, AMoveComponent y)
            {
                if(x.Command.ChessPieceMoves.Count > y.Command.ChessPieceMoves.Count)
                {
                    return -1;
                }
                if(y.Command.ChessPieceMoves.Count > x.Command.ChessPieceMoves.Count)
                {
                    return 1;
                }
                return 0;
            }
        }
    }
}
