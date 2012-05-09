using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MouseGestures
{
    public static class PointExtensions
    {
        public static double AngleTo(this PointF a, PointF b)
        {
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            return Math.Atan2(dy, dx);
        }

        public static double DistanceTo(this PointF a, PointF b)
        {
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            return Math.Pow(dx * dx + dy * dy, 0.5);
        }

        public static PointF Subtract(this PointF a, PointF b)
        {
            return new PointF(a.X - b.X, a.Y - b.Y);
        }

        public static float Cross(this PointF a, PointF b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }
}
