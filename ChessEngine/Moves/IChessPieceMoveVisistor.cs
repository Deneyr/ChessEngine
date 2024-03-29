﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Moves
{
    public interface IChessPieceMoveVisitor
    {
        bool Visit(ChessBoard chessBoard, ChessPieceMovesContainer chessPieceMoveContainer);
    }
}
