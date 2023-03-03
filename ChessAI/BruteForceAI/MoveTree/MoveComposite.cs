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
                        chessBoard.ComputeChessPieceMoves(move);

                        AMoveComponent child = this.CreateChildMoveComponent(childrenIndexPlayer, move, chessBoard);

                        child.ComputeEnteringFitness(chessBoard);

                        chessBoard.RevertLastChessMove();

                        this.mChildren.Add(child);
                    }
                }
            }

            this.ComputeEnteringFitness(chessBoard);

            this.SortChildren();

            //Player lPlayerTurn = this.mGameAreaData.PlayerTurn;

            //IEnumerable<int> lAvailableCards = lPlayerTurn.CardDeck.GetAvailableCards();
            //IEnumerable<Tuple<int, int>> lAvailableFrames = pGameArea.GetAvailableFrames();

            //Player lPlayer = pGameArea.Player2;
            //if (this.mGameAreaData.PlayerTurn == pGameArea.Player1)
            //{
            //    lPlayer = pGameArea.Player1;
            //}

            //if (lAvailableFrames.Count() == 1)
            //{
            //    foreach (int lCardIndex in lAvailableCards)
            //    {
            //        ACommand lCommand = new PlayCardCommand(lPlayer, lCardIndex, lAvailableFrames.ElementAt(0).Item1, lAvailableFrames.ElementAt(0).Item2);

            //        MoveLeaf lMoveLeaf = new MoveLeaf(this, lCommand);

            //        this.mChildren.Add(lMoveLeaf);
            //    }
            //}
            //else
            //{
            //    foreach (int lCardIndex in lAvailableCards)
            //    {
            //        foreach (Tuple<int, int> lFrame in lAvailableFrames)
            //        {
            //            ACommand lCommand = new PlayCardCommand(lPlayer, lCardIndex, lFrame.Item1, lFrame.Item2);

            //            MoveComposite lMoveComposite = new MoveComposite(this, lCommand);

            //            this.mChildren.Add(lMoveComposite);
            //        }
            //    }
            //}
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

            this.postFitnessesByPlayers = lBestComponent.CopyPostFitnesses(out float postTotalFitnesses);
            this.postTotalFistnesses = postTotalFitnesses;

            foreach (AMoveComponent lChild in this.mChildren)
            {
                if (lChild != lBestComponent)
                {
                    lChild.Dispose();
                }
            }

            this.mChildren.Clear();

            this.mBestChild = lBestComponent;
        }

        private AMoveComponent CreateChildMoveComponent(int childrenIndexPlayer, ChessPieceMovesContainer move, ChessBoard chessBoard)
        {
            AMoveComponent child = null;
            bool isGameFinished = chessBoard.CurrentChessTurn.CanPlayerMoveChessPieces == false;
            if (isGameFinished
                || this.Depth > 1)
            {
                child = new MoveLeaf(childrenIndexPlayer, this, move);
            }
            else
            {
                child = new MoveComposite(childrenIndexPlayer, this, move);
            }

            return child;
        }
    }
}
