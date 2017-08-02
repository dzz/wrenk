using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wrenk
{


    class line
    {
        public double x1, y1, x2, y2;
        public bool physics_occluder;
        public bool light_occluder;
        public bool decoration;
    }

    class model
    {
        static public double width = 200;
        static public double height = 200;
        static public List<line> lines = new List<line>();
    }

    class state
    {
        static public double mx = 0.0;
        static public double my = 0.0;
        static public double cx = 0.0;
        static public double cy = 0.0;
        static public double zoom = 2.0;
        static public int layer_index = 0;

        static public int LAYER_LINES = 0;
        static public int LAYER_OBJECTS = 1;
        static public int LAYER_PROPS = 2;
        static public int LAYER_PHOTONS = 3;
        static public int LAYER_LIGHTS = 4;

        static public int leftclick_state = 0;

        static public int LS_STATE_OPEN = 0;
        static public int LS_STATE_DEFINING_LINE = 1;

        static public double open_line_x = 0.0;
        static public double open_line_y = 0.0;

    }
   

    class viewForm : Form
    {
        private Bitmap bm;
        private System.Timers.Timer refTimer;
        private System.Drawing.Pen area_pen;
        private System.Drawing.Pen pending_line_pen;

        private List<string> layernames = new List<string>();
        public viewForm()
        {
            this.DoubleBuffered = true;
            this.refTimer = new System.Timers.Timer(30);
            this.refTimer.Elapsed += RefTimer_Elapsed;
            this.refTimer.Start();
            this.FormClosed += ViewForm_FormClosed;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Width = (int)(960 * 1.5);
            this.Height = (int)(540 * 1.5);


            this.area_pen = new System.Drawing.Pen( Color.Black, 1.5f );
            this.area_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.pending_line_pen = new System.Drawing.Pen(Color.OliveDrab, 1.5f);
            this.pending_line_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;


            this.MouseClick += ViewForm_MouseClick;
            this.MouseMove += ViewForm_MouseMove;
            this.MouseWheel += ViewForm_MouseWheel;
            this.KeyPress += ViewForm_KeyPress;
            

            this.Cursor = Cursors.Cross;

            layernames.Add("line");
            layernames.Add("object");
            layernames.Add("prop");
            layernames.Add("photon");
            layernames.Add("light");

        }

        private void ViewForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            int offs = 0;
            if (e.KeyChar == ']') offs = 1;
            if (e.KeyChar == '[') offs = -1;
                state.layer_index = (state.layer_index+offs) % layernames.Count;
            if (state.layer_index < 0) state.layer_index = layernames.Count - 1;
            state.leftclick_state = state.LS_STATE_OPEN;

        }

        private void ViewForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) { state.zoom *= 1.1; }
            if(e.Delta<0) { state.zoom *= 0.9; }
            
        }

        private void ViewForm_MouseMove(object sender, MouseEventArgs e)
        {
            PointF npt = itpt(e.X,e.Y);

            state.mx = npt.X;
            state.my = npt.Y;
            
        }

        private void ViewForm_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if (state.leftclick_state == state.LS_STATE_OPEN)
                {
                    state.cx = state.mx;
                    state.cy = state.my;
                } else
                {
                    state.leftclick_state = state.LS_STATE_OPEN;
                }
            }
            if(e.Button == MouseButtons.Left)
            {
                if (state.leftclick_state == state.LS_STATE_OPEN && state.layer_index == 0)
                {
                    state.open_line_x = state.mx;
                    state.open_line_y = state.my;

                    state.leftclick_state = state.LS_STATE_DEFINING_LINE;
                }

                if(state.leftclick_state == state.LS_STATE_DEFINING_LINE)
                {
                    var l = new line();
                    l.x1 = state.open_line_x;
                    l.y1 = state.open_line_y;
                    l.x2 = state.mx;
                    l.y2 = state.my;
                    model.lines.Add(l);

                    state.open_line_x = state.mx;
                    state.open_line_y = state.my;
                }
            }
        }

        private void ViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.refTimer.Elapsed -= RefTimer_Elapsed;
       
        }

        public PointF tpt(double x, double y)
        {
            double cx = this.Width / 2.0;
            double cy = this.Height / 2.0;

            x *= state.zoom;
            y *= state.zoom;

            x += cx;
            y += cy;

            x -= state.cx;
            y -= state.cy;

            return new PointF((float)x, (float)y);

        }

        public PointF itpt(double x, double y)
        {
            double cx = this.Width / 2.0;
            double cy = this.Height / 2.0;

            x -= cx;
            y -= cy;
            
            x += state.cx;
            y += state.cy;

            x /= state.zoom;
            y /= state.zoom;


            return new PointF((float)x, (float)y);
        }
        public SizeF tsz(double w, double h)
        {
            return new SizeF((float)(w * state.zoom), (float)(h * state.zoom));
        }
        private void RefTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                {
                    bm = new Bitmap(this.Width, this.Height);
                    var g = Graphics.FromImage(bm);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    var bg_color = Color.AntiqueWhite;
                    {
                        g.Clear(bg_color);

                        {
                            //draw bounds
                            PointF p = this.tpt(0.0 - model.width, 0.0 - model.height);
                            SizeF s = this.tsz(model.width * 2, model.height * 2);
                            g.DrawRectangle(this.area_pen, p.X, p.Y, s.Width, s.Height);
                        }
                        {
                            //draw lines
                            foreach( var line in model.lines )
                            {
                                PointF p1 = this.tpt(line.x1, line.y1);
                                PointF p2 = this.tpt(line.x2, line.y2);

                                g.DrawLine(this.pending_line_pen, p1, p2);
                            }
                        }

                        {
                            //draw active line
                            if (state.leftclick_state == state.LS_STATE_DEFINING_LINE)
                            {
                                PointF p1 = this.tpt(state.open_line_x, state.open_line_y);
                                PointF p2 = this.tpt(state.mx,state.my);

                                g.DrawLine(this.pending_line_pen, p1, p2);
                            }
                        }
                        {
                            //draw cursor
                            // state.mx = 0;
                            //  state.my = 0;
                            PointF p = this.tpt(state.mx, state.my);
                            g.DrawRectangle(System.Drawing.Pens.Red, p.X, p.Y, 5, 5);

                        }

                        {
                            //draw layer names
                            int i = 0;
                            Font f = new Font(FontFamily.GenericMonospace, 10);
                            foreach(var layername in layernames)
                            {
                                String s = layername;
                                if (i == state.layer_index) s = " *" + s;
                                g.DrawString(s, f, Brushes.Black, 10, 10 + (i*10));
                                i++;
                            }
                        }
                    }
                }
                this.BackgroundImage = bm;
            }
            catch (Exception ex) { }
        }

    }
    public partial class wrenk : Form
    {
        public wrenk()
        {
            this.DoubleBuffered = true;
            InitializeComponent();

            var vf = new viewForm();
            vf.Show();
            vf.TopMost = true;
            vf.Width = 1000;
            vf.Height = 800;
            
        }

    }
}
