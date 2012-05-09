using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MouseGestures
{
    public class GestureFactory
    {
        private List<GesturePoint> points = new List<GesturePoint>();

        private PointF _previousOffset;
        private PointF _startPoint;

        public void Start(PointF point)
        {
            _previousOffset = PointF.Empty;
            _startPoint = point;
        }

        public void ApplyPoint(PointF point)
        {
            var offset = point.Subtract(_startPoint);

            if (points.Count == 0)
            {
                points.Add(new GesturePoint { X = offset.X, Y = offset.Y, threshold = 10 });
            }
            else
            {
                var last = points.Last();
                var lastPoint = new PointF(last.X, last.Y);

                // first check distance from last point
                if (lastPoint.DistanceTo(offset) > 10)
                {
                    var currentAngle = _previousOffset.AngleTo(offset);
                    var previousAngle = lastPoint.AngleTo(_previousOffset);

                    var da = currentAngle - previousAngle;
                    if (da < -Math.PI) da += 2 * Math.PI;
                    if (da > Math.PI) da -= 2 * Math.PI;

                    if (da > 0.1)
                    {
                        AddPoint(_previousOffset);
                    }
                }
            }

            _previousOffset = offset;
        }

        private void AddPoint(PointF p)
        {
            var last = points.Last();
            var lastPoint = new PointF(last.X, last.Y);
            var dist = p.DistanceTo(lastPoint);

            float thresh = (float)dist / 2;
            thresh = Math.Max(thresh, 30);

            points.Add(new GesturePoint { X = p.X, Y = p.Y, threshold = thresh });
        }

        public Gesture Finish()
        {
            return new Gesture(points);
        }
    }
}
