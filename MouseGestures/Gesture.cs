using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

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
        public static int ID = 0;

        public string Name { get; set; }

        public List<GesturePoint> Points { get; set; }

        public static Gesture Load(string filename)
        {
            XmlSerializer s = new XmlSerializer(typeof(Gesture));
            var reader = new XmlTextReader(filename);
            return (Gesture)s.Deserialize(reader);
        }

        public Gesture()
        {
            Name = ID.ToString();
            ID++;
            Points = new List<GesturePoint>();
        }

        public Gesture(List<GesturePoint> points) : this()
        {
            this.Points = points;
        }

        public GestureState TestMotion(int index, PointF start, PointF end)
        {
            if (index >= Points.Count - 1)
            {
                return GestureState.Complete;
            }

            var previousPoint = Points[index];
            var nextPoint = Points[index + 1];

            var previousRadiusSq = previousPoint.threshold * previousPoint.threshold;
            var endDX = end.X - previousPoint.X;
            var endDY = end.Y - previousPoint.Y;
            var endRadiusSq = (endDX * endDX) + (endDY * endDY);

            var quad = new GestureQuad(previousPoint, nextPoint);

            // first check for point completion
            if (TestCrossing(quad, start, end))
            {
                if (index == Points.Count - 2) return GestureState.Complete;
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

            var r = p2.Subtract(p);
            var s = q2.Subtract(q);

            var qp = q.Subtract(p);
            var rs = r.Cross(s);

            // parallel case
            if (rs == 0) return false;

            var t = qp.Cross(s) / rs;
            var u = qp.Cross(r) / rs;

            return (t >= 0 && t <= 1 && u >= 0 && u <= 1);
        }

        public void Save(string name)
        {
            XmlSerializer s = new XmlSerializer(typeof(Gesture));
            var writer = new XmlTextWriter(name, Encoding.UTF8);
            s.Serialize(writer, this);
        }
    }
}
