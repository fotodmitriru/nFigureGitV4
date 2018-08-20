using System;

namespace nFigure
{
    [Serializable]
    internal sealed class Circle
    {
        public int IdCir { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int R { get; set; }
        public double S { get; private set; }
        public void CalculateSquare() => S = Math.Pow(3.1415 * R, 2);

        public Circle(int x, int y, int r, int idCir = -1)
        {
            IdCir = idCir;
            X = x;
            Y = y;
            R = r;
            CalculateSquare();
        }
    }
}
