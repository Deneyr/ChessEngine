using ChessEngine;
using ChessEngine.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInterface.Events
{
    public class ChessEvent
    {
        public ChessEventType EventType
        {
            get;
            private set;
        }

        public ChessTurn EventTurn
        {
            get;
            private set;
        }

        public ChessPieceMovesContainer EventMove
        {
            get;
            private set;
        }

        public ChessEvent(ChessEventType eventType, ChessTurn eventTurn, ChessPieceMovesContainer eventMove)
        {
            this.EventType = eventType;
            this.EventTurn = eventTurn;
            this.EventMove = eventMove;
        }
    }
}
