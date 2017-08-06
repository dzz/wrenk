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
        public bool physics_occluder = true;
        public bool light_occluder = true;
        public bool decoration = false;
        public bool selected = false;
    }

    public class obj {
        public string key = "";
        public string json = "{}";
        public double x, y;
    }

    public class prop
    {
        public string image_key = "";
        public Image image = null;
        public double x, y;
        public double w, h;
        public double r;
    }

    class model
    {
        static public double width = 200;
        static public double height = 200;
        static public List<line> lines = new List<line>();
        static public List<obj> objs = new List<obj>();
        static public List<prop> props = new List<prop>();
    }

    public class state
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

        static public int input_state = 0;

        static public int LS_STATE_OPEN = 0;
        static public int LS_STATE_DEFINING_LINE = 1;
        static public int RS_STATE_SELECTING = 2;

        static public double open_sel_x = 0.0;
        static public double open_sel_y = 0.0;

        static public double open_line_x = 0.0;
        static public double open_line_y = 0.0;

        static public double snap = 4.0;
        static public bool modifier = false;
        static public bool accumulator = false;

        static public List<Image> prop_images = new List<Image>();
        static public List<String> prop_names = new List<String>();

    }
   

    class viewForm : Form
    {
        private Bitmap bm;
        private System.Windows.Forms.Timer refTimer;
        private System.Drawing.Pen area_pen;
        private System.Drawing.Pen pending_line_pen;
        private System.Drawing.Pen base_line_pen;
        private System.Drawing.Pen selected_line_pen;
        private objeditor OE = new objeditor();
        private propeditor PE; 

        private System.Drawing.Pen sel_pen;
        private List<string> layernames = new List<string>();
        public viewForm()
        {
            this.DoubleBuffered = true;
            this.refTimer = new System.Windows.Forms.Timer();
            this.refTimer.Interval = 30;
            this.refTimer.Tick += RefTimer_Tick;
            this.refTimer.Start();
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Width = (int)(960 * 1.5);
            this.Height = (int)(540 * 1.5);


            this.area_pen = new System.Drawing.Pen( Color.Black, 1.5f );
            this.area_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.pending_line_pen = new System.Drawing.Pen(Color.OliveDrab, 1.5f);
            this.pending_line_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.base_line_pen = new System.Drawing.Pen(Color.CadetBlue, 1.9f);
            this.selected_line_pen = new System.Drawing.Pen(Color.LightCoral, 8.0f);

            this.sel_pen = new System.Drawing.Pen(Color.CornflowerBlue, 0.5f);
            this.sel_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            this.MouseDoubleClick += ViewForm_MouseDoubleClick;
            this.MouseClick += ViewForm_MouseClick;
            this.MouseMove += ViewForm_MouseMove;
            this.MouseWheel += ViewForm_MouseWheel;
            this.KeyPress += ViewForm_KeyPress;
            this.KeyDown += ViewForm_KeyDown;
            this.KeyUp += ViewForm_KeyUp;
            

            this.Cursor = Cursors.Cross;

            layernames.Add("line");
            layernames.Add("object");
            layernames.Add("prop");
            layernames.Add("photon");
            layernames.Add("light");

            //var fbd = new FolderBrowserDialog();
            //fbd.ShowDialog();
            //var path = fbd.SelectedPath;

            var path = "c:\\users\\dzz\\kthuune\\resources\\forest\\";

            var pngs = System.IO.Directory.GetFiles(path, "*.png");
            foreach(var png in pngs)
            {
                var img = Image.FromFile(png);
                state.prop_images.Add(img);
                state.prop_names.Add(png.Split('\\').Last().Replace(".png",""));
            }

            //MessageBox.Show("Loaded " + state.prop_images.Count.ToString() + " images");
            PE = new propeditor();


        }

        private void ViewForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (state.layer_index == state.LAYER_OBJECTS)
                {
                    var o = new obj();
                    o.key = (String)OE.getKeySelector().SelectedItem;
                    o.json = "{}";
                    o.x = state.mx;
                    o.y = state.my;
                    model.objs.Add(o);
                    OE.synch(o);
                }

                if(state.layer_index == state.LAYER_PROPS)
                {
                    var p = new prop();
                    p.image_key = (String)PE.KeyBox.SelectedItem;
                    p.image = PE.PictureBox.Image;
                    p.x = state.mx;
                    p.y = state.my;
                    p.w = 100;
                    p.h = 100;

                    if(PE.selected != null)
                    {
                        p.w = PE.selected.w;
                        p.h = PE.selected.h;
                    }
                    model.props.Add(p);
                    PE.synch(p);
                }
            }
        }

        private void RefTimer_Tick(object sender, EventArgs e)
        {
            this.redraw();
        }

        private void ViewForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Alt) {
                state.modifier = true;
            }

            if(e.Shift)
            {
                state.accumulator = true;
            }
            
        }

        private void ViewForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Menu)
            {
                state.modifier = false;
            }
            if(! (e.KeyData == Keys.Shift))
            {
                state.accumulator = false;
            }
        }

        private void ViewForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar=='=')
            {
                state.snap += 2.0;
            }

            if(e.KeyChar=='-')
            {
                if(state.snap>2.0)
                state.snap -= 2.0;
            }
            int offs = 0;
            if (e.KeyChar == ']') offs = 1;
            if (e.KeyChar == '[') offs = -1;
                state.layer_index = (state.layer_index+offs) % layernames.Count;
            if (state.layer_index < 0) state.layer_index = layernames.Count - 1;
            state.input_state = state.LS_STATE_OPEN;

            if(state.layer_index==0)
            {
                if(e.KeyChar=='x')
                {
                    model.lines = model.lines.Where(x => x.selected == false).ToList();
                }
                var selected = model.lines.Where(x => x.selected == true).ToList();
                if (e.KeyChar=='P')
                {
                    foreach(var line in selected)
                    {
                        line.physics_occluder = true;
                    }
                }
                if (e.KeyChar == 'p')
                {
                    foreach (var line in selected)
                    {
                        line.physics_occluder = false;
                    }
                }
                if (e.KeyChar == 'L')
                {
                    foreach (var line in selected)
                    {
                        line.light_occluder = true;
                    }
                }
                if (e.KeyChar == 'l')
                {
                    foreach (var line in selected)
                    {
                        line.light_occluder= false;
                    }
                }
                if (e.KeyChar == 'D')
                {
                    foreach (var line in selected)
                    {
                        line.decoration = true;
                    }
                }
                if (e.KeyChar == 'd')
                {
                    foreach (var line in selected)
                    {
                        line.decoration = false;
                    }
                }
            }

            if(state.layer_index == state.LAYER_OBJECTS)
            {
                OE.Show();
            } else
            {
                OE.Hide();
            }

            if (state.layer_index == state.LAYER_PROPS)
            {
                PE.Show();
            }
            else
            {
                PE.Hide();
            }

        }

        private void ViewForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!state.modifier)
            {
                if (e.Delta > 0) { state.zoom *= 1.1; }
                if (e.Delta < 0) { state.zoom *= 0.9; }
            }
            else
            {

                if(state.layer_index == state.LAYER_PROPS)
                {
                    if(PE.selected !=null)
                    {
                        if (e.Delta > 0) { PE.selected.w *= 1.05; PE.selected.h *= 1.05; }
                        if (e.Delta < 0) { PE.selected.w *= 0.95; PE.selected.h *= 0.95; }
                    }
                }
            }
            
        }

        private void ViewForm_MouseMove(object sender, MouseEventArgs e)
        {
            PointF npt = itpt(e.X,e.Y);

            state.mx = npt.X;
            state.my = npt.Y;
            
        }

        private void commitSelection()
        {
            if(state.layer_index == state.LAYER_LINES)
            {
                double gx = Math.Max(state.open_sel_x, state.mx);
                double lx = Math.Min(state.open_sel_x, state.mx);
                double gy = Math.Max(state.open_sel_y, state.my);
                double ly = Math.Min(state.open_sel_y, state.my);

                foreach( var line in model.lines )
                {

                    if (state.modifier == false)
                    {
                        if (
                           (line.x1 > lx) && (line.x1 < gx) && (line.x2 > lx) && (line.x2 < gx) &&
                            (line.y1 > ly) && (line.y1 < gy) && (line.y2 > ly) && (line.y2 < gy)
                            )
                        {
                            line.selected = true;
                        }
                        else
                        {
                            if (!state.accumulator)
                            {
                                line.selected = false;
                            }
                        }
                    } else
                    {
                        if ((line.x1 > lx) && (line.x1 < gx) && (line.y1 > ly) && (line.y1 < gy))
                            line.selected = false;

                        if ((line.x2 > lx) && (line.x2 < gx) && (line.y2 > ly) && (line.y2 < gy))
                            line.selected = false;
                    }
                }
            }
        }

        private void ViewForm_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Middle)
            {
                state.cx = state.mx;
                state.cy = state.my;
            }

            if (e.Button == MouseButtons.Right)
            {
                if (state.layer_index == state.LAYER_LINES)
                {
                    if (state.input_state == state.LS_STATE_OPEN)
                    {

                        state.input_state = state.RS_STATE_SELECTING;
                        state.open_sel_x = state.mx;
                        state.open_sel_y = state.my;
                    }
                    else
                    if (state.input_state == state.RS_STATE_SELECTING)
                    {
                        this.commitSelection();
                        state.input_state = state.LS_STATE_OPEN;
                    }
                    else
                    if (state.input_state == state.LS_STATE_DEFINING_LINE)
                    {
                        state.input_state = state.LS_STATE_OPEN;
                    }
                }

                if( state.layer_index == state.LAYER_OBJECTS)
                {
                    if(this.OE.selected != null)
                    {
                        this.OE.selected.x = state.mx;
                        this.OE.selected.y = state.my;
                    }
                }

                if (state.layer_index == state.LAYER_PROPS)
                {
                    if (this.PE.selected != null)
                    {
                        this.PE.selected.x = state.mx;
                        this.PE.selected.y = state.my;
                    }
                }
            }

            if(e.Button == MouseButtons.Left)
            {
                if (state.layer_index == state.LAYER_LINES)
                {
                    if (state.input_state == state.LS_STATE_OPEN && state.layer_index == 0)
                    {
                        state.open_line_x = state.mx;
                        state.open_line_y = state.my;

                        foreach (var line in model.lines)
                        {
                            line.selected = false;
                        }
                        state.input_state = state.LS_STATE_DEFINING_LINE;
                    }

                    if (state.input_state == state.LS_STATE_DEFINING_LINE)
                    {
                        var l = new line();
                        l.x1 = state.open_line_x;
                        l.y1 = state.open_line_y;
                        l.x2 = state.mx;
                        l.y2 = state.my;
                        l.selected = true;
                        model.lines.Add(l);

                        state.open_line_x = state.mx;
                        state.open_line_y = state.my;
                    }
                }
                if(state.layer_index == state.LAYER_OBJECTS)
                {
                    foreach(var o in model.objs)
                    {
                        var dx = state.mx - o.x;
                        var dy = state.my - o.y;
                        if((dx*dx)+(dy*dy) < 20)
                        {
                            this.OE.selected = null;
                            this.OE.synch(o);
                            break;
                        }
                    }
                }

                if (state.layer_index == state.LAYER_PROPS)
                {
                    foreach (var p in model.props)
                    {
                        if( 
                             ( state.mx > (p.x-p.w) ) &&
                             ( state.mx < (p.x+p.w) ) &&
                             ( state.my > (p.y - p.h)) &&
                             ( state.my < (p.y + p.h)) ) 

                        {
                            this.PE.selected = null;
                            this.PE.synch(p);
                            break;
                        }
                    }
                }
            }
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

            double effective_snap = state.snap;

            if(state.input_state == state.RS_STATE_SELECTING) {
                effective_snap /= 2;
            }
            x = Math.Floor(x / effective_snap) * effective_snap;
            y = Math.Floor(y / effective_snap) * effective_snap;
            return new PointF((float)x, (float)y);
        }
        public SizeF tsz(double w, double h)
        {
            return new SizeF((float)(w * state.zoom), (float)(h * state.zoom));
        }

        float t = 0.0f;
        private void redraw()
        {
            t = t + 0.1f;
            Bitmap nbm = new Bitmap(this.Width, this.Height);
            try
            {
                {
                   
                    var g = Graphics.FromImage(nbm);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

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
                            //draw props
                            foreach(var p in model.props)
                            {
                                PointF pp = this.tpt(p.x, p.y);
                                SizeF s = this.tsz(p.w, p.h);

                                //g.RotateTransform(t);
                                
                                
                                //g.DrawImage(p.image, pp.X - s.Width, pp.Y - s.Height, s.Width * 2, s.Height * 2);
                                
                                g.TranslateTransform(pp.X, pp.Y);
                                g.RotateTransform(45.0f);
                                g.TranslateTransform(-s.Width, -s.Height);
                                g.DrawImage(p.image, 0.0f, 0.0f, s.Width * 2, s.Height * 2);

                               
                                g.ResetTransform();

                                

                                if(p == PE.selected)
                                {
                                    float w = s.Width * 1.03f;
                                    float h = s.Height * 1.03f;
                                    g.DrawRectangle(Pens.Red, pp.X - w, pp.Y - h, w * 2, h * 2);
                                }
                                
                            }
                        }
                        {
                            //draw lines
                            foreach( var line in model.lines )
                            {
                                PointF p1 = this.tpt(line.x1, line.y1);
                                PointF p2 = this.tpt(line.x2, line.y2);

                                if(line.selected)
                                {
                                    g.DrawLine(this.selected_line_pen, p1, p2);
                                }

                                Pen p = new System.Drawing.Pen(Color.LightGray, 2.5f);
                                p.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                                
                                if(line.light_occluder && !line.physics_occluder)
                                {
                                    p.Color = Color.Black;
                                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                                }

                                if(line.physics_occluder && !line.light_occluder)
                                {

                                    p.Color = Color.Black;
                                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                                }

                                if (line.physics_occluder && line.light_occluder)
                                {
                                    p.Color = Color.Black;
                                    p.Width = 4.0f;
                                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                                }

                                if(line.decoration)
                                {
                                    p.Color = Color.DarkGreen;
                                }
                                g.DrawLine(p, p1, p2);
                            }
                        }

                        {
                            //draw active line
                            if (state.input_state == state.LS_STATE_DEFINING_LINE)
                            {
                                PointF p1 = this.tpt(state.open_line_x, state.open_line_y);
                                PointF p2 = this.tpt(state.mx,state.my);

                                g.DrawLine(this.pending_line_pen, p1, p2);
                            }
                        }

                        {
                            //draw selection
                            if(state.input_state == state.RS_STATE_SELECTING)
                            {
                               
                                PointF p1 = this.tpt(state.open_sel_x, state.open_sel_y);
                                PointF p2 = this.tpt(state.mx, state.my);

                                
                                if(p1.X<p2.X)
                                {
                                    float t = p1.X;
                                    p1.X = p2.X;
                                    p2.X = t;
                                }
                                if(p1.Y < p2.Y)
                                {
                                    float t = p1.Y;
                                    p1.Y = p2.Y;
                                    p2.Y = t;
                                }

                                float w = p1.X - p2.X;
                                float h = p1.Y - p2.Y;
                                g.DrawRectangle(this.sel_pen,p2.X,p2.Y,w,h);

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

                            g.DrawString("input state:" + state.input_state.ToString(), f, Brushes.Black, 10, 80);
                            g.DrawString("snap:" + state.snap.ToString(), f, Brushes.Black, 10, 90);
                        }

                        {
                            //draw objects
                            foreach(var o in model.objs)
                            {
                                Font f = new Font(FontFamily.GenericMonospace, 6);
                                PointF p = this.tpt(o.x, o.y);
                                g.DrawRectangle(Pens.DarkCyan, p.X - 10, p.Y - 10, 20, 20);
                                g.DrawString(o.key, f, Brushes.DarkBlue, p.X -20 , p.Y + 23);

                                if(o == this.OE.selected)
                                {
                                    g.DrawRectangle(Pens.DarkMagenta, p.X - 13, p.Y - 13, 26, 26);
                                }
                            }
                        }

                        {
                            //draw reticle
                            PointF p = this.tpt(state.mx, state.my);
                
                            g.DrawLine( Pens.White, p.X, 0, p.X, (float)this.Height );
                            g.DrawLine( Pens.White, 0, p.Y, (float)this.Width, p.Y );
                        }
                    }
                    g.Dispose();
                }
               
                this.BackgroundImage = nbm;
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
