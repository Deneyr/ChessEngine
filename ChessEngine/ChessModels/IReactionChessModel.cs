using ChessEngine.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.ChessModels
{
    public interface IReactionChessModel: IChessModel, IChessPieceMoveVisitor
    {
        
    }
}
