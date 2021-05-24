using System.Drawing;
using System.Drawing.Drawing2D;

namespace Animation1
{
  
    class FigureDrawer
    {
        public PaintResourses tools { get; set;}
        public Figure figure { get; set; }

        public void Draw(Graphics g)
        {
            tools = new PaintResourses();
            tools.AddPen(Color.Black);
            tools.AddSolidBrush(Color.Gray);

            g.FillRectangle(Brushes.White, g.VisibleClipBounds);

            if (figure == null)
            {
                tools.Dispose();
                return;
            }
      
            g.Transform = figure.basisMatrix;

            foreach (var point in figure.Points)
            {
                g.DrawRectangle(tools.pen, point.X, point.Y, 2, 2);
            }

            Image centerImage = figure.GetPointImage();
            g.DrawImage(centerImage, figure.center.X - centerImage.Width / 2, figure.center.Y - centerImage.Height / 2);

            if (figure.Points.Count > 1)
            {
                Point[] pathPoints = new Point[figure.Points.Count];
                figure.Points.CopyTo(pathPoints);
                GraphicsPath path = new GraphicsPath(pathPoints, figure.GetPathPointsTypes());

                g.DrawPath(Pens.Black, path);
            }


            tools.Dispose();
        }
    }

   
}
