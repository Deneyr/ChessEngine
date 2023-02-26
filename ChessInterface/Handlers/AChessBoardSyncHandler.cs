using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInterface.Handlers
{
    public abstract class AChessBoardSyncHandler: AChessBoardHandler
    {
        public override void OnInterfaceAttached(ChessBoardInterface parentInterface)
        {
            base.OnInterfaceAttached(parentInterface);

            parentInterface.InterfaceUpdating += this.OnInterfaceUpdating;
        }

        public override void OnInterfaceDetached(ChessBoardInterface parentInterface)
        {
            base.OnInterfaceDetached(parentInterface);

            parentInterface.InterfaceUpdating -= this.OnInterfaceUpdating;
        }

        private void OnInterfaceUpdating(float deltaSec)
        {
            this.UpdateHandler();
        }
    }
}
