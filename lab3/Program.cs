using System;

namespace lab3
{
    struct Point
    {
        public decimal x { get; set; }
        public decimal y { get; set; }
        public Point(decimal X, decimal Y) => (x, y) = (X, Y);
        public Point(decimal[] coord) => (x, y) = (coord[0], coord[1]);
        public override string ToString() => $"({this.x}, {this.y})";
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Point point0 = new(0, 0);
            Point point1;
            Console.WriteLine("Введіть координати другої точки через пробіл");
            while (true)
                try
                {
                    var temp = Array.ConvertAll(Console.ReadLine().Split(), decimal.Parse);
                    if (temp.Length != 2) continue;
                    point1 = new(temp);
                    break;
                }
                catch { Console.WriteLine("Неправильний тип введення"); }
            Point point2 = FindThirdPoint(point0, point1);

            Console.WriteLine($"Координати третьої точки для мінімальної площі трикутника: {point2}");
            Console.WriteLine($"Площа в цьому випадку: {CalculateTriangleArea(point0, point1, point2)}");
        }
        static Point FindThirdPoint(Point _point0, Point _point1)
        {
            Point startSearch = new(Math.Min(_point0.x, _point1.x) - 2, Math.Min(_point0.y, _point1.y) - 2);
            Point endSearch = new(Math.Max(_point0.x, _point1.x) + 2, Math.Max(_point0.y, _point1.y) + 2);
            Point? bestPoint = null;
            decimal minArea = decimal.MaxValue;
            for (decimal x = startSearch.x; x <= endSearch.x; x++)
            {
                for (decimal y = startSearch.y; y <= endSearch.y; y++)
                {
                    if ((x == _point0.x && y == _point0.y) || (x == _point1.x && y == _point1.y)) continue;
                    Point _point2 = new Point(x, y);
                    decimal area = CalculateTriangleArea(_point0, _point1, _point2);
                    if (area > 0 && area < minArea)
                    {
                        minArea = area;
                        bestPoint = _point2;
                    }
                }
            }
            return bestPoint ?? new Point(0, 0);
        }
        static decimal CalculateTriangleArea(Point point0, Point point1, Point point2) =>
             decimal.Abs(point0.x * (point1.y - point2.y) + point1.x * (point2.y - point0.y) + point2.x * (point0.y - point1.y)) / 2;
    }
}
