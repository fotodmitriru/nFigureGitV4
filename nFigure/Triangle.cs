using System;
using System.Drawing;
using System.Diagnostics;

namespace nFigure
{
    [Serializable]
    internal sealed class Triangle : BasePolygon
    {
        public int IdTrian { get; private set; }
        public Point A
        {
            get { return PointsOfPolygon[0]; }
            set { PointsOfPolygon[0] = value; }
        }

        public Point B
        {
            get { return PointsOfPolygon[1]; }
            set { PointsOfPolygon[1] = value; }
        }

        public Point C
        {
            get { return PointsOfPolygon[2]; }
            set { PointsOfPolygon[2] = value; }
        }

        public Triangle(Point a, Point b, Point c, int idTrian = -1) : base(new Point[3])
        {
            IdTrian = idTrian;
            A = a;
            B = b;
            C = c;
            Debug.Assert(CalculateSides() == 1);
            CalculatePerimeter();
            CalculateSquare();
        }

        public Triangle(int x1, int y1, int x2, int y2, int x3, int y3, int idTrian = -1) :
            this(new Point(x1, y1), new Point(x2, y2), new Point(x3, y3), idTrian)
        { }
    }
}
