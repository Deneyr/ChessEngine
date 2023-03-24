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
    public abstract class AMoveComponent
    {
        protected int currentIndexPlayer;

        protected float preFitnessForFirstPlayer;

        protected float postFitnessForFirstPlayer;

        public bool PostFitnessComputed
        {
            get;
            protected set;
        }

        public virtual int Depth
        {
            get;
            private set;
        }

        protected virtual MoveRoot Root
        {
            get;
            set;
        }

        public MoveComposite Parent
        {
            get;
            private set;
        }

        public ChessPieceMovesContainer Command
        {
            get;
            private set;
        }

        public virtual AMoveComponent BestChild
        {
            get
            {
                return null;
            }
        }

        public float PreFitnessForFirstPlayer
        {
            get
            {
                return this.preFitnessForFirstPlayer;
            }
        }


        public float PreFitnessForCurrentPlayer
        {
            get
            {
                return this.GetFitnessForCurrentPlayer(this.preFitnessForFirstPlayer);
            }
        }

        public float PostFitnessForFirstPlayer
        {
            get
            {
                return this.postFitnessForFirstPlayer;
            }
        }


        public float PostFitnessForCurrentPlayer
        {
            get
            {
                return this.GetFitnessForCurrentPlayer(this.postFitnessForFirstPlayer);
            }
        }

        public AMoveComponent(int currentIndexPlayer, MoveRoot root, MoveComposite parent, ChessPieceMovesContainer command)
        {
            this.currentIndexPlayer = currentIndexPlayer;

            this.Root = root;
            this.Parent = parent;

            this.Command = command;

            this.PostFitnessComputed = false;

            if (parent != null)
            {
                this.Depth = parent.Depth + 1;
            }
            else
            {
                this.Depth = 0;
            }
        }

        public float GetFitnessForCurrentPlayer(float fitnessForFirstPlayer)
        {
            if (this.currentIndexPlayer != 0)
            {
                return -fitnessForFirstPlayer;
            }
            return fitnessForFirstPlayer;
        }

        //public virtual void CreateChildren()
        //{
        //    // Nothing to do
        //}

        public abstract void StudyMove();

        public void AlphaBetaPruning()
        {
            AMoveComponent parentNode = this.Parent;
            AMoveComponent grandPaNode = null;
            if(parentNode != null)
            {
                grandPaNode = parentNode.Parent;
            }

            if(grandPaNode != null
                && parentNode.currentIndexPlayer != grandPaNode.currentIndexPlayer)
            {
                if (grandPaNode.IsPruningAvailableFor(this.preFitnessForFirstPlayer))
                {
                    parentNode.PruneAllChildrenExcept(this);
                }
            }
        }

        public virtual void ComputeEnteringFitness()
        {
            ChessBoard chessBoard = this.Root.MainChessBoard;
            ChessTurn currentChessTurn = chessBoard.CurrentChessTurn;

            int nextPlayerIndex = currentChessTurn.IndexPlayer;

            bool canPlayerMoveCheckPiece = chessBoard.CanPlayerMoveChessPieces();
            bool isKingChecked = canPlayerMoveCheckPiece == false && chessBoard.IsKingChecked(chessBoard.Players[nextPlayerIndex].KingChessPiece);

            this.preFitnessForFirstPlayer = this.ComputeFitnessOfPlayer(0, canPlayerMoveCheckPiece, nextPlayerIndex == 0 ? isKingChecked : false) - this.ComputeFitnessOfPlayer(1, canPlayerMoveCheckPiece, nextPlayerIndex == 1 ? isKingChecked : false);
        }

        public virtual void ComputeClosingFitness()
        {
            this.postFitnessForFirstPlayer = this.preFitnessForFirstPlayer;

            this.PostFitnessComputed = true;
        }

        protected void EndStudy()
        {
            MoveComposite currentParent = this.Parent;
            AMoveComponent currentChild = this;
            while(currentParent != null
                && currentParent.ForwardChild(currentChild))
            {
                currentChild = currentParent;
                currentParent = currentParent.Parent;
            }
        }

        protected virtual float ComputeFitnessOfPlayer(int playerIndex, bool canPlayerMoveCheckPiece, bool isKingChecked)
        {
            ChessBoard chessBoard = this.Root.MainChessBoard;

            if (canPlayerMoveCheckPiece == false)
            {
                if (isKingChecked)
                {
                    return -1000000f;
                }
                else
                {
                    return 0;
                }
            }

            float resultFitness = 0;
            IPlayer player = chessBoard.Players[playerIndex];
            IEnumerable<ChessPiece> chessPiecesAlive = player.ChessPiecesOwned.Where(pElem => pElem.IsAlive);
            foreach (ChessPiece chessPiece in chessPiecesAlive)
            {
                resultFitness += this.ChessPieceTypeToValue(chessPiece.ChessPieceType);

                //int nbPossibleMoves = chessPiece.GetAllPossibleMoves(chessBoard).Count;
                //resultFitness += nbPossibleMoves / 10f;
            }

            return resultFitness;
        }

        protected virtual void PruneAllChildrenExcept(AMoveComponent moveComponent)
        {

        }

        protected virtual bool IsPruningAvailableFor(float fitnessFirstPlayer)
        {
            return false;
        }

        private float ChessPieceTypeToValue(ChessPieceType chessPieceType)
        {
            switch (chessPieceType)
            {
                case ChessPieceType.PAWN:
                    return 1;
                case ChessPieceType.BISHOP:
                    return 3;
                case ChessPieceType.KNIGHT:
                    return 3;
                case ChessPieceType.ROOK:
                    return 5;
                case ChessPieceType.QUEEN:
                    return 10;
                case ChessPieceType.KING:
                    return 20;
            }
            return 1;
        }
    }
}
