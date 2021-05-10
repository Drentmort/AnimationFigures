using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using System;

namespace Animation1
{
    enum CurveType
    {
        POLYGON,
        BEZIE,
        SPLAIN,
        ELLIPSE
    }
    public enum DotPaintType
    {
        FILLED_CIRCLE,
        EMPTY_CIRCLE,
        FILLED_RECTANGLE,
        EMPTY_RECTANGLE,
    }

    interface IFigure
    {
        List<Point> Points { get; }
        Matrix basisMatrix { get; set; }
        Rectangle boundingBox { get; set; }
        PointF center { get; set; }
        CurveType curveType { get; set; }
        DotPaintType dotPaintType { get; set; }
        float angularSpeed { get; set; }
        bool isPointsShown { get; set; }
        bool isLinessShown { get; set; }
        bool isAxisShown { get; set; }
        void SetBounds(int x, int y, int weight, int height);
        void SetAngleAndRotate(float angle);

    }

    class Figure : IFigure, IDisposable
    {
        public List<Point> Points { get; } = new List<Point>();
        public Matrix basisMatrix { get; set; }
        public Rectangle boundingBox { get; set; }
        public PointF center { get; set; }
        public CurveType curveType { get; set; }
        public DotPaintType dotPaintType { get; set; }
        public float angularSpeed { get; set; }
        public bool isPointsShown { get; set; }
        public bool isLinessShown { get; set; }
        public bool isAxisShown { get; set; }
        
        public Figure()
        {
            basisMatrix = new Matrix();
            angularSpeed = 0;
            curveType = CurveType.BEZIE;
            dotPaintType = DotPaintType.FILLED_CIRCLE;
        }

        public void SetBounds(int x, int y, int widht, int height)
        {
            boundingBox = new Rectangle(x, y, widht, height);
            center = new PointF(x + widht / 2, y + height / 2);
        }

        public void SetBounds(Rectangle bounds)
        {
            boundingBox = bounds;
            center = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
        }

        public void SetAngleAndRotate(float angle)
        {
            this.angularSpeed = angle;
            basisMatrix.RotateAt(this.angularSpeed, center);
        }

        public void AddPoint(Point point)
        {
            if (Points.Count == 0)
                SetBounds(point.X, point.Y, 2, 2);
            else
            {
                if (point.X < boundingBox.X || point.Y < boundingBox.Y)
                    SetBounds(point.X, point.Y, 
                        boundingBox.Width + boundingBox.X - point.X, 
                        boundingBox.Height + boundingBox.Y - point.Y);
                if (point.X > boundingBox.X || point.Y > boundingBox.Y)
                    SetBounds(boundingBox.X, boundingBox.Y,
                        point.X - boundingBox.X,
                        point.Y - boundingBox.Y);
            }
            Points.Add(point);
            CenterMass();
        }
        public byte[] GetPathPointsTypes()
        {
            byte[] pathTypePoints = new byte[Points.Count];

            pathTypePoints[0] = 0;

            for (int i = 1; i < pathTypePoints.Length - 1; i++)
                pathTypePoints[i] = 1;

            pathTypePoints[pathTypePoints.Length - 1] = 1;

            return pathTypePoints;
        
        }
        public Bitmap GetPointImage()
        {
            Bitmap image = new Bitmap(6,6);
            Graphics graphTemp = Graphics.FromImage(image);
            switch (dotPaintType)
            {
                case DotPaintType.FILLED_CIRCLE:
                    {
                        graphTemp.DrawEllipse(Pens.Black, 1, 1, 5, 5);
                        graphTemp.FillEllipse(Brushes.Red, 1, 1, 5, 5);
                    }
                    break;
            }
            graphTemp.Dispose();

            return image;

        }
        public Bitmap GetNodesImage()
        {
            if (boundingBox.Width == 0 || boundingBox.Height == 0)
                return null;

            Bitmap image = new Bitmap(boundingBox.Width+1, boundingBox.Width+1);
            Graphics graphTemp = Graphics.FromImage(image);
            foreach(var node in Points)
            {
                graphTemp.DrawRectangle(Pens.Black, node.X- boundingBox.Width, node.Y- boundingBox.Height, 2, 2);
            }
            graphTemp.Dispose();

            return image;
        }
        private void CenterMass()
        {
            double X = 0.0;
            double Y = 0.0;

            foreach(var point in Points)
            {
                X += point.X;
                Y += point.Y;
            }
            X /= Points.Count;
            Y /= Points.Count;
            center = new PointF((float)X, (float)Y);
        }
        public void Dispose()
        {
            if (basisMatrix != null)
                basisMatrix.Dispose();
        }
    }
}
