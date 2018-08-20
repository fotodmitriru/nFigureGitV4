using System;
using System.Drawing;
using System.Diagnostics;

namespace nFigure
{
    [Serializable]
    internal sealed class Rectangle : BasePolygon
    {
        public int IdRect { get; private set; }
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

        public Point D
        {
            get { return PointsOfPolygon[3]; }
            set { PointsOfPolygon[3] = value; }
        }

        public Rectangle(Point a, Point b, Point c, Point d, int idRect = -1) : base(new Point[4])
        {
            IdRect = idRect;
            A = a;
            B = b;
            C = c;
            D = d;
            Debug.Assert(CalculateSides() == 1);
            CalculatePerimeter();
            CalculateSquare();
        }

        public Rectangle(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4, int idRect = -1) :
            this(new Point(x1, y1), new Point(x2, y2), new Point(x3, y3), new Point(x4, y4), idRect)
        { }
    }
}
