using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInterface.Handlers
{
    public abstract class AChessBoardSyncHandler: AChessBoardHandler
    {
        public void SyncUpdateHandler()
        {
            this.UpdateHandler();
        }
    }
}
