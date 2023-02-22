using ChessInterface.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChessInterface.Handlers
{
    public abstract class AChessBoardAsyncHandler: AChessBoardHandler
    {
        private Semaphore semaphore;

        private Thread mainThread;
        private bool isThreadActive;

        public AChessBoardAsyncHandler()
        {
            this.semaphore = new Semaphore(0, 10);

            this.mainThread = new Thread(new ThreadStart(this.RunThread));
            this.isThreadActive = true;

            this.mainThread.Start();
        }

        private void RunThread()
        {
            while (this.isThreadActive)
            {
                this.semaphore.WaitOne();

                this.UpdateHandler();
            }
        }

        public override void EnqueueChessEvent(ChessEvent chessEvent)
        {
            base.EnqueueChessEvent(chessEvent);

            this.semaphore.Release();
        }

        public override void Dispose()
        {
            this.isThreadActive = false;

            base.Dispose();
        }
    }
}
