using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab3Form
{
    public partial class Form1 : Form
    {
        private PointF point0; // ����� (0, 0)
        private PointF point1; // ����������� �����
        private PointF point2; // ����� �����
        private bool isDragging = false; // ���� ��� ��������������, �� ������������ �����
        private float scaleX = 50; // ������� �� �� X (���������, 1 ������� = 50 ������)
        private float scaleY = 50; // ������� �� �� Y (���������, 1 ������� = 50 ������)

        public Form1()
        {
            InitializeComponent();
            point0 = new PointF(this.ClientSize.Width / 2f, this.ClientSize.Height / 2f); // ����� ������
            point1 = new PointF(2, 1); // ��������� ����� � ���� ������ ���������
            point2 = CalculateThirdPoint(point0, point1);
        }

        public static float CalculateTriangleArea(PointF _point0, PointF _point1, PointF _point2) =>
             Math.Abs(_point0.X * (_point1.Y - _point2.Y) + _point1.X * (_point2.Y - _point0.Y) + _point2.X * (_point0.Y - _point1.Y)) / 2;

        // ����� ��� ���������� ������ �����
        private PointF CalculateThirdPoint(PointF point0, PointF point1)
        {
            int dist = (int)Math.Sqrt(point0.X * point0.X + point0.Y * point0.Y) * 2;
            Point startSearch = new((int)Math.Min(point0.X, point1.X) - dist, (int)Math.Min(point0.Y, point1.Y) - dist);
            Point endSearch = new((int)Math.Max(point0.X, point1.X) + dist, (int)Math.Max(point0.Y, point1.Y) + dist);
            Point bestPoint = new Point(0, 0);
            float minArea = float.MaxValue;
            for (int x = startSearch.X; x <= endSearch.X; x += 2)
            {
                for (int y = startSearch.Y; y <= endSearch.Y; y += 2)
                {
                    if ((x == point0.X && y == point0.Y) || (x == point1.X && y == point1.Y))
                        continue;
                    Point point2 = new(x, y);
                    float area = CalculateTriangleArea(point0, point1, point2);
                    if (area > 0 && area < minArea)
                    {
                        minArea = area;
                        bestPoint = point2;
                    }
                }
            }
            float a = 1.3f;
            return new PointF(bestPoint.X * a, bestPoint.Y * a);
        }

        // ������� �� ������� ��������� �� ������
        private PointF ConvertToPixelCoordinates(PointF point)
        {
            return new PointF(point0.X + point.X * scaleX, point0.Y - point.Y * scaleY);
        }

        // ������� ��䳿 ��������� �� ����
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // ������� �����
            g.FillEllipse(Brushes.Red, ConvertToPixelCoordinates(point0).X - 5, ConvertToPixelCoordinates(point0).Y - 5, 10, 10); // �����
            g.FillEllipse(Brushes.Blue, ConvertToPixelCoordinates(point1).X - 5, ConvertToPixelCoordinates(point1).Y - 5, 10, 10); // ����������� �����
            g.FillEllipse(Brushes.Green, ConvertToPixelCoordinates(point2).X - 5, ConvertToPixelCoordinates(point2).Y - 5, 10, 10); // ����� �����

            // ������� �� �� �������
            g.DrawLine(Pens.Black, ConvertToPixelCoordinates(point0), ConvertToPixelCoordinates(point1));
            g.DrawLine(Pens.Black, ConvertToPixelCoordinates(point1), ConvertToPixelCoordinates(point2));
            g.DrawLine(Pens.Black, ConvertToPixelCoordinates(point2), ConvertToPixelCoordinates(point0));

            // ����������� ����� ����������
            float area = CalculateTriangleArea(point0, point1, point2);

            // ������� ����� ��� ���������
            string text = $"����� 1: ({point0.X:F2}, {point0.Y:F2})\n" +
                          $"����� 2: ({point1.X:F2}, {point1.Y:F2})\n" +
                          $"����� 3: ({point2.X:F2}, {point2.Y:F2})\n" +
                          $"����� ����������: {area:F2}";

            // �������� ����� � ������ ���� ���
            g.DrawString(text, new Font("Arial", 10), Brushes.Black, new PointF(10, 10));
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsPointClicked(e.Location, point1))
                isDragging = true;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                PointF newPoint1 = new PointF((e.X - point0.X) / scaleX, (point0.Y - e.Y) / scaleY);
                point1 = newPoint1;
                point2 = CalculateThirdPoint(point0, point1);
                Invalidate();
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e) =>
            isDragging = false;

        private bool IsPointClicked(Point mousePos, PointF point)
        {
            PointF pointPixel = ConvertToPixelCoordinates(point);
            return Math.Sqrt(Math.Pow(mousePos.X - pointPixel.X, 2) + Math.Pow(mousePos.Y - pointPixel.Y, 2)) < 10;
        }

        private void MainForm_Load(object sender, EventArgs e) =>
            this.DoubleBuffered = true;
    }
}
