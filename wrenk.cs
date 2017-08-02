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
    class model
    {
        static public double width = 200;
        static public double height = 200;
    }

    class state
    {
        static public double mx = 0.0;
        static public double my = 0.0;
        static public double cx = 0.0;
        static public double cy = 0.0;
        static public double zoom = 2.0;
    }



    class viewForm : Form
    {
        private Bitmap bm;
        private System.Timers.Timer refTimer;
        private System.Drawing.Pen area_pen;
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

            this.MouseClick += ViewForm_MouseClick;
            this.MouseMove += ViewForm_MouseMove;

            this.Cursor = Cursors.Cross;
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
                PointF npt = itpt(e.X, e.Y);
                state.cx = npt.X;
                state.cy = npt.Y;
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
                        g.DrawRectangle(this.area_pen, p.X,p.Y, s.Width,s.Height);
                    }

                    {
                        //draw cursor
                       // state.mx = 0;
                      //  state.my = 0;
                        PointF p = this.tpt(state.mx, state.my);
                        g.DrawRectangle(System.Drawing.Pens.Red, p.X, p.Y, 5, 5);

                    }
                }
            }
            this.BackgroundImage = bm;

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
