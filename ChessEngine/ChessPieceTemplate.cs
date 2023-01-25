using ChessEngine.ChessModels;
using ChessEngine.ChessModels.Monitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class ChessPieceTemplate: IChessPiece
    {
        public virtual ChessPieceType ChessPieceType
        {
            get;
            set;
        }

        internal virtual ActionChessModelMonitor ActionModelMonitor
        {
            get;
            set;
        }

        internal virtual ReactionChessModelMonitor ReactionModelMonitor
        {
            get;
            set;
        }

        public ChessPieceTemplate()
        {
            this.ActionModelMonitor = new ActionChessModelMonitor();
            this.ReactionModelMonitor = new ReactionChessModelMonitor();
        }
    }
}
