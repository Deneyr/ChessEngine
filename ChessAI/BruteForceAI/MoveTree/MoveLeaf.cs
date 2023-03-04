using ChessEngine;
using ChessEngine.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.BruteForceAI.MoveTree
{
    public class MoveLeaf : AMoveComponent
    {
        public MoveLeaf(int currentIndexPlayer, MoveComposite pParent, ChessPieceMovesContainer pCommand) 
            : base(currentIndexPlayer, pParent, pCommand)
        {
        }

        public override void ComputeFitnessAndClean(ChessBoard chessBoard)
        {
            //int lPlointsPlayer = pGameArea.GetPointsPlayer(pGameArea.Player1);

            //int lPlointsOtherPlayer = pGameArea.GetPointsPlayer(pGameArea.Player2);

            //int lPointsDifference = lPlointsPlayer - lPlointsOtherPlayer;

            //if (lPointsDifference > 0)
            //{
            //    this.mFitnessPlayer1 = 3;
            //    this.mFitnessPlayer2 = 0;
            //}
            //else if (lPointsDifference < 0)
            //{
            //    this.mFitnessPlayer1 = 0;
            //    this.mFitnessPlayer2 = 3;
            //}
            //else
            //{
            //    this.mFitnessPlayer1 = 1;
            //    this.mFitnessPlayer2 = 1;
            //}

            //this.postFitnessesByPlayers = this.CopyPreFitnesses(out float preTotalFitnesses);
            //this.postTotalFistnesses = preTotalFitnesses;

            this.ComputeEnteringFitness(chessBoard);
            this.PostFitnessComputed = true;
        }

        public override AMoveComponent GetNextChild()
        {
            return null;
        }
    }
}
