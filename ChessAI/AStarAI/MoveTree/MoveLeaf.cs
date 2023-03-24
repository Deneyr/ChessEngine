using ChessEngine;
using ChessEngine.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.AStarAI.MoveTree
{
    public class MoveLeaf : AMoveComponent
    {
        public MoveLeaf(int currentIndexPlayer, MoveRoot root,MoveComposite pParent, ChessPieceMovesContainer pCommand) 
            : base(currentIndexPlayer, root, pParent, pCommand)
        {
        }

        //public override void ComputeClosingFitness()
        //{
        //    this.postFitnessForFirstPlayer = this.preFitnessForFirstPlayer;

        //    this.PostFitnessComputed = true;
        //}

        //public override AMoveComponent GetNextChild()
        //{
        //    return null;
        //}

        public override void StudyMove()
        {
            this.ComputeClosingFitness();

            this.EndStudy();
        }
    }
}
