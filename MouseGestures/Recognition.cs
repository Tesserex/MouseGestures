using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGestures
{
    public class Recognition
    {
        private class Attempt
        {
            public PointF _startLocation;
            public PointF _previousOffset;
            public int _step;
            public bool _ok = true;
        }

        private Gesture _gesture;

        private List<Attempt> attempts = new List<Attempt>();

        public bool OK { get { return attempts.Any(a => a._step > 0); } }

        public Recognition(Gesture gesture)
        {
            this._gesture = gesture;
        }

        public bool Step(PointF location)
        {
            bool success = false;

            for (var i = 0; i < attempts.Count; i++)
            {
                var attempt = attempts[i];

                PointF currentOffset = new PointF(location.X - attempt._startLocation.X, location.Y - attempt._startLocation.Y);

                switch (_gesture.TestMotion(attempt._step, attempt._previousOffset, currentOffset))
                {
                    case GestureState.Fail:
                        attempt._ok = false;
                        break;

                    case GestureState.Complete:
                        attempt._ok = false;
                        success = true;
                        break;

                    case GestureState.Advance:
                        attempt._step++;
                        attempt._previousOffset = currentOffset;
                        break;

                    case GestureState.OK:
                        attempt._previousOffset = currentOffset;
                        break;
                }
            }

            if (success)
            {
                attempts.Clear();
            }
            else
            {
                attempts = attempts.Where(a => a._ok).ToList();
                attempts.Add(new Attempt() { _ok = true, _step = 0, _previousOffset = Point.Empty, _startLocation = location });
            }

            return success;
        }
    }
}
