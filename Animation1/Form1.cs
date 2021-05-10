using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Animation1
{
    public partial class Form1 : Form
    {
        private FigureDrawer curveDrawer;
        private float speed = 1;

        public Form1()
        {
            InitializeComponent();
            curveDrawer = new FigureDrawer();
            curveDrawer.figure = new Figure();
        }
        
        private void PaintArea_Paint(object sender, PaintEventArgs e)
        {
            curveDrawer.Draw(e.Graphics);
        }

        private void Start_Click(object sender, EventArgs e)
        {
            PaintArea.Invalidate();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            curveDrawer.figure.SetAngleAndRotate(speed);
            PaintArea.Invalidate();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                speed = float.Parse(textBox1.Text);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void PaintArea_Click(object sender, EventArgs e)
        {
            Point position = Control.MousePosition;
            position.X -= ( PaintArea.Bounds.X + this.Location.X);
            position.Y -= ( PaintArea.Bounds.Y + this.Location.Y);

            Matrix tempMatrix = curveDrawer.figure.basisMatrix.Clone();
            tempMatrix.Invert();

            Point[] point = new Point[1];
            point[0] = position;
            tempMatrix.TransformPoints(point);

            curveDrawer.figure.AddPoint(point[0]);
            PaintArea.Invalidate();
        }
    }

}
