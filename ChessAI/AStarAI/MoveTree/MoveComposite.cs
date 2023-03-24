using ChessEngine;
using ChessEngine.Moves;
using ChessEngine.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.AStarAI.MoveTree
{
    public class MoveComposite : AMoveComponent
    {
        private AMoveComponent bestChild;

        public List<AMoveComponent> Children
        {
            get;
            private set;
        }

        public override AMoveComponent BestChild
        {
            get
            {
                //AMoveComponent bestComponent = this.Children.FirstOrDefault();

                //if(bestComponent != null && bestComponent.PostFitnessComputed)
                //{
                //    return bestComponent;
                //}
                //return null;

                return this.bestChild;
            }
        }

        public MoveComposite(int currentIndexPlayer, MoveRoot root, MoveComposite pParent, ChessPieceMovesContainer pCommand) 
            : base(currentIndexPlayer, root, pParent, pCommand)
        {
            this.Children = null;
            this.bestChild = null;
        }

        public void CreateChildren()
        {
            if(this.Children != null)
            {
                return;
            }

            this.Children = new List<AMoveComponent>();

            int childrenIndexPlayer = this.Root.MainChessBoard.CurrentChessTurn.IndexPlayer;
            IPlayer childrenInternalPlayer = this.Root.MainChessBoard.Players[childrenIndexPlayer];

            ChessBoard chessBoard = this.Root.MainChessBoard;

            //this.Root.CurrentCounter--;

            foreach (ChessPiece chessPiece in childrenInternalPlayer.ChessPiecesOwned)
            {
                if (chessPiece.IsAlive)
                {
                    List<ChessPieceMovesContainer> allPossibleMoves = chessPiece.GetAllPossibleMoves(chessBoard);
                    foreach(ChessPieceMovesContainer move in allPossibleMoves)
                    {
                        AMoveComponent child = this.CreateChildMoveComponent(childrenIndexPlayer, move);

                        this.AddChild(child);
                    }
                }
            }

            //this.SortChildren();
        }

        private void AddChild(AMoveComponent child)
        {
            this.Children.Add(child);

            this.Root.AddMoveComponentToStudied(child);
        }

        //public override AMoveComponent GetNextChild()
        //{
        //    if(this.mCrossIndex < this.mChildren.Count)
        //    {
        //        return this.mChildren.ElementAt(this.mCrossIndex++);
        //    }
        //    return null;
        //}

        //public override void ComputeClosingFitness()
        //{
        //    //if (this.mChildren.Count == 0)
        //    //{
        //    //    this.ComputeEnteringFitness();
        //    //    this.PostFitnessComputed = true;
        //    //    return;
        //    //}

        //    //float lTotalFitnessChildrenPlayer = 0;
        //    //bool isFirstChild = true;
        //    //AMoveComponent lBestComponent = null;

        //    //foreach (AMoveComponent lChild in this.mChildren)
        //    //{
        //    //    float fitness = lChild.PreFitnessForCurrentPlayer;
        //    //    if (isFirstChild || fitness > lTotalFitnessChildrenPlayer)
        //    //    {
        //    //        lTotalFitnessChildrenPlayer = fitness;
        //    //        lBestComponent = lChild;
        //    //        isFirstChild = false;
        //    //    }
        //    //}

        //    //this.fitnessForFirstPlayer = lBestComponent.PreFitnessForFirstPlayer;

        //    //this.mChildren.Clear();

        //    //this.mBestChild = lBestComponent;
        //    this.PostFitnessComputed = true;
        //}

        //protected virtual void SortChildren()
        //{
        //    this.mChildren.Sort(new MoveComponentComparer());
        //}

        internal bool ForwardChild(AMoveComponent child)
        {
            //this.Children.Sort(new MoveComponentComparer());

            //AMoveComponent currentBestComponent = this.Children.First();
            //float currentBestPostFitness = this.GetFitnessForCurrentPlayer(currentBestComponent.PostFitnessForFirstPlayer);

            //if (this.PostFitnessComputed == false
            //    || currentBestPostFitness != this.PostFitnessForCurrentPlayer)
            //{
            //    this.postFitnessForFirstPlayer = currentBestComponent.PostFitnessForFirstPlayer;
            //    this.PostFitnessComputed = true;

            //    return true;
            //}
            //return false;

            if (this.BestChild == null
                || child.PostFitnessForCurrentPlayer > this.BestChild.GetFitnessForCurrentPlayer(this.postFitnessForFirstPlayer))
            {
                this.bestChild = child;
                this.postFitnessForFirstPlayer = child.PostFitnessForFirstPlayer;
                return true;
            }
            return false;
        }

        protected override void PruneAllChildrenExcept(AMoveComponent moveComponent)
        {
            this.Children.Clear();
            this.Children.Add(moveComponent);
            //this.mCrossIndex = 1;
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

                if(childComputed.PreFitnessForCurrentPlayer >= trueFitness)
                {
                    return true;
                }
            }
            return false;
        }

        public override void StudyMove()
        {
            this.CreateChildren();

            if (this.Children.Any() == false)
            {
                this.ComputeClosingFitness();

                this.EndStudy();
            }
        }

        private AMoveComponent CreateChildMoveComponent(int childrenIndexPlayer, ChessPieceMovesContainer move)
        {
            AMoveComponent child = null;
            //bool isGameFinished = false;//chessBoard.CurrentChessTurn.CanPlayerMoveChessPieces == false;

            if (this.Root.CurrentCounter <= 0
                ||this.Depth > this.Root.MaxDepth)
            {
                child = new MoveLeaf(childrenIndexPlayer, this.Root, this, move);
            }
            else
            {
                child = new MoveComposite(childrenIndexPlayer, this.Root, this, move);
                this.Root.CurrentCounter--;
            }

            return child;
        }

        //private class MoveComponentComparer : IComparer<AMoveComponent>
        //{
        //    public int Compare(AMoveComponent x, AMoveComponent y)
        //    {
        //        if(x.PostFitnessComputed == false || y.PostFitnessComputed == false)
        //        {
        //            return 0;
        //        }
        //        if(x.PostFitnessComputed && y.PostFitnessComputed == false)
        //        {
        //            return -1;
        //        }
        //        if(x.PostFitnessComputed == false && y.PostFitnessComputed)
        //        {
        //            return 1;
        //        }

        //        if(x.PostFitnessForCurrentPlayer > y.PostFitnessForCurrentPlayer)
        //        {
        //            return -1;
        //        }
        //        if(x.PostFitnessForCurrentPlayer < y.PostFitnessForCurrentPlayer)
        //        {
        //            return 1;
        //        }
        //        return 0;
        //    }
        //}
    }
}
