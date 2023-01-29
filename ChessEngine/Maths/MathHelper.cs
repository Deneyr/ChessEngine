using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Maths
{
    public static class MathHelper
    {
        public static void ChangeBaseVector(int xVector, int yVector, int xYBaseVector, int yYBaseVector, out int xResultVector, out int yResultVector)
        {
            GetXVectorFromY(xYBaseVector, yYBaseVector, out int xXBaseVector, out int yXBaseVector);

            xResultVector = xXBaseVector * xVector + xYBaseVector * yVector;
            yResultVector = yXBaseVector * xVector + yYBaseVector * yVector;
        }

        private static void GetXVectorFromY(int xYVector, int yYVector, out int xXVector, out int yXVector)
        {
            xXVector = -yYVector / (xYVector * xYVector + yYVector * yYVector);
            yXVector = -xYVector / (yYVector * yYVector - xYVector * xYVector);
        }
    }
}
