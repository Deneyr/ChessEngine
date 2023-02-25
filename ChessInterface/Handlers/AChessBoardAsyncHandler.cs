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
        private bool receivedChessEvent;

        private Thread mainThread;
        private bool isThreadActive;

        public AChessBoardAsyncHandler()
        {
            this.mainThread = new Thread(new ThreadStart(this.RunThread));
            this.isThreadActive = true;

            this.mainThread.Start();
        }

        private void RunThread()
        {
            while (this.isThreadActive)
            {
                bool currentReceivedChessEvent = false;

                lock (this.handlerLock)
                {
                    if(this.receivedChessEvent
                        || Monitor.Wait(this.handlerLock))
                    {
                        currentReceivedChessEvent = true;

                        this.receivedChessEvent = false;
                    }
                }

                if (currentReceivedChessEvent)
                {
                    this.UpdateHandler();
                }
            }
        }

        public override void EnqueueChessEvent(ChessEvent chessEvent)
        {
            base.EnqueueChessEvent(chessEvent);

            lock(this.handlerLock)
            {
                this.receivedChessEvent = true;

                Monitor.Pulse(this.handlerLock);
            }
        }

        public override void Dispose()
        {
            this.isThreadActive = false;

            base.Dispose();
        }
    }
}
