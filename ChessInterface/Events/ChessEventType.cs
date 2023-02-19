using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInterface.Events
{
    public enum ChessEventType
    {
        NEXT_TURN,
        PREVIOUS_TURN,

        MOVE_APPLIED,
        MOVE_REVERTED
    }
}
