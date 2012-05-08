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
        private Gesture _gesture;
        private PointF _startLocation;
        private PointF _previousOffset;
        private int _step;
        private bool _hasReset = false;

        public bool OK { get; private set; }

        public Recognition(Gesture gesture)
        {
            this._gesture = gesture;
        }

        public void Reset(PointF location)
        {
            this._step = 0;
            this._startLocation = location;
            this._previousOffset = PointF.Empty;
            this._hasReset = true;
            this.OK = false;
        }

        public bool Step(PointF location)
        {
            if (!this._hasReset)
            {
                Reset(location);
                return false;
            }

            PointF currentOffset = new PointF(location.X - _startLocation.X, location.Y - _startLocation.Y);

            switch (_gesture.TestMotion(this._step, this._previousOffset, currentOffset))
            {
                case GestureState.Fail:
                    this.Reset(location);
                    return false;

                case GestureState.Complete:
                    this.Reset(location);
                    return true;

                case GestureState.Advance:
                    this._step++;
                    this.OK = true;
                    break;
            }

            this._previousOffset = currentOffset;

            return false;
        }
    }
}
