using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseGestures
{
    public partial class Form1 : Form
    {
        private bool _recording;
        private PointF _startPoint;
        private PointF _currentPoint;
        private int _mouseTick = 0;
        private List<GesturePoint> points = new List<GesturePoint>();
        private GraphicsPath _path = new GraphicsPath();
        private Gesture _gesture = null;
        private Recognition _recognition = null;

        private int _thresh = 30;
        private Pen _pen;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _pen = new Pen(Color.Black, 30);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            _recording = !_recording;

            if (_recording)
            {
                _startPoint = _currentPoint;
                points.Clear();
                points.Add(new GesturePoint { X = 0, Y = 0, threshold = _thresh });
                _path = new GraphicsPath();
                _path.AddLine(_currentPoint, _currentPoint);
                BackColor = Color.OrangeRed;
            }
            else
            {
                _path.AddLine(_currentPoint, _currentPoint);
                points.Add(new GesturePoint { X = _currentPoint.X - _startPoint.X, Y = _currentPoint.Y - _startPoint.Y, threshold = _thresh });
                _gesture = new Gesture(points);
                _recognition = new Recognition(_gesture);
                BackColor = Color.White;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_recording)
            {
                _mouseTick++;
                if (_mouseTick >= 50)
                {
                    _path.AddLine(e.Location, e.Location);
                    points.Add(new GesturePoint { X = e.X - _startPoint.X, Y = e.Y - _startPoint.Y, threshold = _thresh });
                    _mouseTick = 0;
                }
            }
            else if (_recognition != null)
            {
                if (_mouseTick == 0)
                {
                    if (_recognition.Step(e.Location))
                    {
                        BackColor = Color.LightGreen;
                        _mouseTick++;
                    }
                    else if (_recognition.OK)
                    {
                        BackColor = Color.Cyan;
                    }
                    else
                    {
                        BackColor = Color.White;
                    }
                }
                else if (_mouseTick > 0)
                {
                    _mouseTick++;
                    if (_mouseTick >= 20)
                    {
                        BackColor = Color.White;
                        _mouseTick = 0;
                    }
                }
            }

            _currentPoint = e.Location;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawPath(_pen, _path);
        }
    }
}
