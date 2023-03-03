using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI.BruteForceAI.MoveTree
{
    public class MoveRoot : MoveComposite
    {
        public MoveRoot() 
            : base(-1, null, null)
        {
        }
    }
}
