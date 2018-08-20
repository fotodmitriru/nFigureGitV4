using System;
using System.Drawing;

namespace nFigure
{
    [Serializable]
    internal class BasePolygon
    {
        public Point[] PointsOfPolygon { get; private set; }
        public double[] LenghtOfSides { get; private set; }
        public double P { get; private set; }
        public double S { get; private set; }
        private double CalculateSide(Point a, Point b) => Math.Sqrt(Math.Pow((b.X - a.X), 2) +
                                                Math.Pow((b.Y - a.Y), 2));
        protected int CalculateSides()
        {
            if (PointsOfPolygon == null)
                return -1;
            LenghtOfSides = new double[PointsOfPolygon.Length];
            for (int i = 0; i < PointsOfPolygon.Length; i++)
                LenghtOfSides[i] = (i + 1 != PointsOfPolygon.Length) ?
                CalculateSide(PointsOfPolygon[i], PointsOfPolygon[i + 1]) :
                CalculateSide(PointsOfPolygon[i], PointsOfPolygon[0]);
            return 1;
        }

        protected void CalculatePerimeter()
        {
            foreach (double i in LenghtOfSides)
                P += i;
        }

        protected void CalculateSquare()
        {
            double xy = 0;
            double yx = 0;
            for (int i = 0; i < PointsOfPolygon.Length; i++)
            {
                xy += PointsOfPolygon[i].X * ((i + 1 != PointsOfPolygon.Length) ?
                                                PointsOfPolygon[i + 1].Y :
                                                PointsOfPolygon[0].Y);
                yx += PointsOfPolygon[i].Y * ((i + 1 != PointsOfPolygon.Length) ?
                                                PointsOfPolygon[i + 1].X :
                                                PointsOfPolygon[0].X);
            }
            S = ((xy - yx) / 2);
            S = (S < 0) ? S * -1 : S;//(абсолютное значение)строку можно закомментировать,
                                     //тогда присутствие знака минус будет показывать в какой последовательности
                                     //происходит выборка вершин в системе координат
                                     //+ против часовой, - по часовой
        }

        public BasePolygon(Point[] pointsOfPolygon)
        {
            PointsOfPolygon = pointsOfPolygon;
            //CalculateSquare();
        }
    }
}