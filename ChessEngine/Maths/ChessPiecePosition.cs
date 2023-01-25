using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Maths
{
    public struct ChessPiecePosition
    {
        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public ChessPiecePosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static bool operator !=(ChessPiecePosition position1, ChessPiecePosition position2)
        {
            return (position1 == position2) == false;
        }

        public static bool operator ==(ChessPiecePosition position1, ChessPiecePosition position2)
        {
            return position1.Equals(position2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
