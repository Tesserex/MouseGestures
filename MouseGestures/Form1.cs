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
        private GestureFactory _factory;
        private GraphicsPath _path = new GraphicsPath();
        private Gesture _gesture = null;
        private Recognition _recognition = null;
        private PointF _mousePos;

        private Pen _pen;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _pen = new Pen(Color.Black, 2);
        }

        private void gestureSurface_Click(object sender, EventArgs e)
        {
            _recording = !_recording;

            if (_recording)
            {
                _factory = new GestureFactory();
                _factory.Start(_mousePos);
                _path = new GraphicsPath();
                gestureSurface.BackColor = Color.OrangeRed;
                eventList.Items.Clear();
            }
            else
            {
                _gesture = _factory.Finish();
                _recognition = new Recognition(_gesture);
                gestureSurface.BackColor = Color.White;

                _path.ClearMarkers();

                var minX = _gesture.Points.Min(g => g.X);
                var maxX = _gesture.Points.Max(g => g.X);
                var minY = _gesture.Points.Min(g => g.Y);
                var maxY = _gesture.Points.Max(g => g.Y);

                var centerX = (gestureSurface.Width - (minX + maxX)) / 2;
                var centerY = (gestureSurface.Height - (minY + maxY)) / 2;

                GesturePoint? prev = null;

                foreach (var point in _gesture.Points)
                {
                    _path.AddEllipse(point.X + centerX - point.threshold, point.Y + centerY - point.threshold, point.threshold * 2, point.threshold * 2);
                    
                    if (prev != null)
                    {
                        _path.AddLine(prev.Value.X + centerX, prev.Value.Y + centerY, point.X + centerX, point.Y + centerY);
                    }
                    prev = point;
                }
                gestureSurface.Refresh();
            }
        }

        private void gestureSurface_MouseMove(object sender, MouseEventArgs e)
        {
            _mousePos = e.Location;

            if (_recording)
            {
                _factory.ApplyPoint(e.Location);
            }
            else
            {
                if (_recognition != null)
                {
                    if (_recognition.Step(e.Location))
                    {
                        gestureSurface.BackColor = Color.White;
                        eventList.Items.Add(String.Format("Gesture detected: {0}", _gesture.Name));
                    }
                    else if (_recognition.OK)
                    {
                        gestureSurface.BackColor = Color.Cyan;
                    }
                    else
                    {
                        gestureSurface.BackColor = Color.White;
                    }
                }
            }
        }

        private void gestureSurface_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawPath(_pen, _path);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (_gesture != null)
            {
                _gesture.Save("Gesture.xml");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _gesture = Gesture.Load("Gesture.xml");
        }
    }
}
