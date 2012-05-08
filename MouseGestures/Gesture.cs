using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGestures
{
    public struct GesturePoint
    {
        public float X;
        public float Y;
        public float threshold;
    }

    public struct GestureQuad
    {
        public PointF A;
        public PointF B;
        public PointF C;
        public PointF D;

        public GestureQuad(GesturePoint p1, GesturePoint p2)
        {
            var dx = p2.X - p1.X;
            var dy = p2.Y - p1.Y;

            var p1Mag = p1.threshold * p1.threshold;
            var p2Mag = p2.threshold * p2.threshold;
            var dMag = dx * dx + dy * dy;

            var magScale1 = p1Mag / dMag;
            var magScale2 = p2Mag / dMag;

            var n1 = new PointF(-dy * magScale1, dx * magScale1);
            var n2 = new PointF(-dy * magScale2, dx * magScale2);

            // if I did this right they should go counterclockwise
            A = new PointF(p1.X + n1.X, p1.Y + n1.Y);
            B = new PointF(p1.X - n1.X, p1.Y - n1.Y);
            C = new PointF(p2.X - n2.X, p2.Y - n2.Y);
            D = new PointF(p2.X + n2.X, p2.Y + n2.Y);
        }

        public bool Test(PointF testPoint)
        {
            return
                (TestEdge(A, B, testPoint) >= 0) &&
                (TestEdge(B, C, testPoint) >= 0) &&
                (TestEdge(C, D, testPoint) >= 0) &&
                (TestEdge(D, A, testPoint) >= 0);
        }

        private float TestEdge(PointF p1, PointF p2, PointF testPoint)
        {
            var a = p1.Y - p2.Y;
            var b = p2.X - p1.X;
            var c = -1 * (a * p1.X + b * p1.Y);

            return a * testPoint.X + b * testPoint.Y + c;
        }
    }

    public enum GestureState : int
    {
        OK,
        Advance,
        Complete,
        Fail
    }

    public class Gesture
    {
        private List<GesturePoint> points = new List<GesturePoint>();

        public Gesture(List<GesturePoint> points)
        {
            this.points = points;
        }

        public GestureState TestMotion(int index, PointF start, PointF end)
        {
            if (index >= points.Count - 1)
            {
                return GestureState.Complete;
            }

            var previousPoint = points[index];
            var nextPoint = points[index + 1];

            var previousRadiusSq = previousPoint.threshold * previousPoint.threshold;
            var endDX = end.X - previousPoint.X;
            var endDY = end.Y - previousPoint.Y;
            var endRadiusSq = (endDX * endDX) + (endDY * endDY);

            var quad = new GestureQuad(previousPoint, nextPoint);

            // first check for point completion
            if (TestCrossing(quad, start, end))
            {
                if (index == points.Count - 2) return GestureState.Complete;
                else return GestureState.Advance;
            }

            // must be inside the quad or the starting circle
            if (!(endRadiusSq <= previousRadiusSq || quad.Test(end))) return GestureState.Fail;

            return GestureState.OK;
        }

        private bool TestCrossing(GestureQuad quad, PointF p, PointF p2)
        {
            var q = quad.C;
            var q2 = quad.D;

            var r = Subtract(p2, p);
            var s = Subtract(q2, q);

            var qp = Subtract(q, p);
            var rs = Cross(r, s);

            // parallel case
            if (rs == 0) return false;

            var t = Cross(qp, s) / rs;
            var u = Cross(qp, r) / rs;

            return (t >= 0 && t <= 1 && u >= 0 && u <= 1);
        }

        private PointF Subtract(PointF a, PointF b)
        {
            return new PointF(a.X - b.X, a.Y - b.Y);
        }

        private float Cross(PointF a, PointF b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }
}
