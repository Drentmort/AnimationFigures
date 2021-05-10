using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Animation1
{
    abstract class Drawer
    {
        public abstract void Draw(Graphics g);
    }

    interface IFigureDrawer
    {
        Figure figure { get; set; }
        PaintResourses tools { get; set; }
        void Draw(Graphics g);
    }

    class FigureDrawer : Drawer, IFigureDrawer
    {
        public PaintResourses tools { get; set;}
        public Figure figure { get; set; }

        public override void Draw(Graphics g)
        {
            tools = new PaintResourses();
            tools.AddGraphics(g);
            tools.AddPen(Color.Black);
            tools.AddSolidBrush(Color.Gray);

            g.FillRectangle(Brushes.White, g.VisibleClipBounds);

            if (figure == null)
            {
                tools.Dispose();
                return;
            }
      
            tools.graphics.Transform = figure.basisMatrix;

            //foreach (var point in figure.Points)
            //{
            //    tools.graphics.DrawRectangle(tools.pen, point.X, point.Y, 2, 2);
            //}

            Image nodesImage = figure.GetNodesImage();
            if (nodesImage != null)
                tools.graphics.DrawImage(nodesImage, figure.boundingBox.Location);

            Image centerImage = figure.GetPointImage();
            tools.graphics.DrawImage(centerImage, figure.center.X - centerImage.Width / 2, figure.center.Y - centerImage.Height / 2);

            //if (figure.Points.Count > 1) 
            //{
            //    Point[] pathPoints = new Point[figure.Points.Count];
            //    figure.Points.CopyTo(pathPoints);
            //    GraphicsPath path = new GraphicsPath(pathPoints, figure.GetPathPointsTypes());

            //    tools.graphics.DrawPath(Pens.Black, path); 
            //}
           

            tools.Dispose();
        }

        public override string ToString()
        {
            return "Эллипс";
        }
    }
}
