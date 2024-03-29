﻿using ChessEngine;
using ChessEngine.Moves;
using ChessEngine.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.BruteForceAI.MoveTree
{
    public abstract class AMoveComponent
    {
        protected int currentIndexPlayer;

        private MoveComposite mParent;

        private ChessPieceMovesContainer mCommand;

        //protected float mFitnessPlayer1;
        //protected float mFitnessPlayer2;

        //protected List<float> preFitnessesByPlayers;
        //protected List<float> postFitnessesByPlayers;
        //protected float preTotalFistnesses;
        //protected float postTotalFistnesses;

        protected float fitnessForFirstPlayer;

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

        public AMoveComponent(int currentIndexPlayer, MoveComposite pParent, ChessPieceMovesContainer pCommand)
        {
            this.currentIndexPlayer = currentIndexPlayer;

            this.mParent = pParent;

            //this.mGameAreaData = null;

            this.mCommand = pCommand;

            //this.preFitnessesByPlayers = new List<float>();
            //this.postFitnessesByPlayers = new List<float>();

            this.PostFitnessComputed = false;

            if (pParent != null)
            {
                this.Depth = pParent.Depth + 1;
            }
            else
            {
                this.Depth = 0;
            }
        }

        //public float FitnessPlayer1 { get => mFitnessPlayer1; set => mFitnessPlayer1 = value; }
        //public float FitnessPlayer2 { get => mFitnessPlayer2; set => mFitnessPlayer2 = value; }
        //public float PreFitnessForCurrentPlayer
        //{
        //    get
        //    {
        //        float fitnessCurrentPlayer = this.preFitnessesByPlayers[this.currentIndexPlayer];
        //        float fitnessOtherPlayers = (this.preTotalFistnesses - fitnessCurrentPlayer) / (this.preFitnessesByPlayers.Count - 1);

        //        return fitnessCurrentPlayer - fitnessOtherPlayers;
        //    }
        //}

        public float PostFitnessForFirstPlayer
        {
            get
            {
                return this.fitnessForFirstPlayer;
            }
        }


        public float PostFitnessForCurrentPlayer
        {
            get
            {
                //float fitnessCurrentPlayer = this.postFitnessesByPlayers[this.currentIndexPlayer];
                //float fitnessOtherPlayers = (this.postTotalFistnesses - fitnessCurrentPlayer) / (this.postFitnessesByPlayers.Count - 1);

                //return fitnessCurrentPlayer - fitnessOtherPlayers;
                return this.GetFitnessForCurrentPlayer(this.fitnessForFirstPlayer);
            }
        }

        public ChessPieceMovesContainer Command { get => mCommand; }

        //public GameAreaData GameAreaData { get => mGameAreaData; set => mGameAreaData = value; }

        public MoveComposite Parent { get => mParent; }

        public virtual AMoveComponent BestChild
        {
            get
            {
                return null;
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

        public virtual void CreateChildren(ChessBoard chessBoard)
        {
            // Nothing to do
        }

        public abstract AMoveComponent GetNextChild();

        public abstract void ComputeFitnessAndClean(ChessBoard chessBoard);

        //public virtual GameArea LoadGameAreaData(GameArea pGameArea)
        //{
        //    pGameArea.SetFromSnapshot(this.mGameAreaData);

        //    return pGameArea;
        //}

        public void AlphaBetaPruning()
        {
            AMoveComponent parentNode = this.mParent;
            AMoveComponent grandPaNode = null;
            if(parentNode != null)
            {
                grandPaNode = parentNode.mParent;
            }

            if(grandPaNode != null
                && parentNode.currentIndexPlayer != grandPaNode.currentIndexPlayer)
            {
                if (grandPaNode.IsPruningAvailableFor(this.fitnessForFirstPlayer))
                {
                    parentNode.PruneAllChildrenExcept(this);
                }
            }
        }

        public virtual void ComputeEnteringFitness(ChessBoard chessBoard)
        {
            //this.preTotalFistnesses = 0;
            //for (int i = 0; i < chessBoard.Players.Count; i++)
            //{
            //    float fitness = this.ComputeFitnessOfPlayer(chessBoard, i);

            //    this.preFitnessesByPlayers.Add(fitness);

            //    this.preTotalFistnesses += fitness;
            //}

            this.fitnessForFirstPlayer = this.ComputeFitnessOfPlayer(chessBoard, 0) - this.ComputeFitnessOfPlayer(chessBoard, 1);
        }

        //public List<float> CopyPostFitnesses(out float postTotalFitnesses)
        //{
        //    postTotalFitnesses = this.postTotalFistnesses;
        //    //return new List<float>(this.postFitnessesByPlayers);
        //    return this.postFitnessesByPlayers;
        //}

        //public List<float> CopyPreFitnesses(out float preTotalFitnesses)
        //{
        //    preTotalFitnesses = this.preTotalFistnesses;
        //    //return new List<float>(this.preFitnessesByPlayers);
        //    return this.preFitnessesByPlayers;
        //}

        protected virtual float ComputeFitnessOfPlayer(ChessBoard chessBoard, int playerIndex)
        {
            ChessTurn currentChessTurn = chessBoard.CurrentChessTurn;
            if (chessBoard.CanPlayerMoveChessPieces() == false)
            {
                if (chessBoard.IsKingChecked(chessBoard.Players[playerIndex].KingChessPiece)
                    && currentChessTurn.IndexPlayer == playerIndex)
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
