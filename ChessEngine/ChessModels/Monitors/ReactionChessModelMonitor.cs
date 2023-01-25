using ChessEngine.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.ChessModels.Monitors
{
    public class ReactionChessModelMonitor : IChessPieceMoveVisitor
    {
        public List<IReactionChessModel> ReactionChessModels
        {
            get;
            private set;
        }

        public ReactionChessModelMonitor()
        {
            this.ReactionChessModels = new List<IReactionChessModel>();
        }

        public void AddReactionChessModel(IReactionChessModel reactionChessModelToAdd)
        {
            this.ReactionChessModels.Add(reactionChessModelToAdd);
        }

        public void RemoveReactionChessModel(IReactionChessModel reactionChessModelToRemove)
        {
            this.ReactionChessModels.Remove(reactionChessModelToRemove);
        }

        public bool Visit(ChessBoard chessBoard, ChessPieceMovesContainer chessPieceMoveContainer)
        {
            List<IReactionChessModel>.Enumerator reactionChessModelsEnum = this.ReactionChessModels.GetEnumerator();
            bool isMoveContainerValid = false;

            while (isMoveContainerValid == false && reactionChessModelsEnum.MoveNext())
            {
                isMoveContainerValid = reactionChessModelsEnum.Current.Visit(chessBoard, chessPieceMoveContainer);
            }

            return isMoveContainerValid;
        }
    }
}
