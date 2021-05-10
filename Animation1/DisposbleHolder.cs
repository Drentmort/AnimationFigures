using System;
using System.Drawing;
using System.Collections.Generic;

namespace Animation1
{
    abstract class DisposbleHolder : IDisposable
    {
        public abstract void AddResourse(IDisposable obj);
        public abstract void Dispose();
    }

    interface IResourseDisposbleHolder
    {
        List<IDisposable> Resourses { get; }
        void AddResourse(IDisposable obj);
        void Dispose();

    }
    class ResourseDisposbleHolder : DisposbleHolder, IResourseDisposbleHolder
    {
        public List<IDisposable> Resourses { get; }

        public override void Dispose()
        {
            foreach (var resourse in Resourses)
            {
                if (resourse != null)
                    resourse.Dispose();
            }

            if (Resourses.Count != 0)
                Resourses.Clear();
        }

        public override void AddResourse(IDisposable obj)
        {
            Resourses.Add(obj);
        }

        public ResourseDisposbleHolder()
        {
            Resourses = new List<IDisposable>();
        }
    }

    class PaintResourses: ResourseDisposbleHolder
    {
        public Graphics graphics;
        public Pen pen;
        public Brush brush;

        public void AddPen(Color color, float width = 1)
        {
            pen = new Pen(color, width);
            Resourses.Add(pen);
        }

        public void AddSolidBrush(Color color)
        {
            brush = new SolidBrush(color);
            Resourses.Add(brush);
        }

        public void AddGraphics(Graphics graphics)
        {
            this.graphics = graphics;
            Resourses.Add(this.graphics);
        }
    }
}
