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
    public partial class modelprops : Form
    {
        public void synch()
        {
            this.textBox1.Text = "dims=" + model.width.ToString() + "," + model.height.ToString() + ";";
        }
        public modelprops()
        {
            InitializeComponent();
            this.textBox1.TextChanged += TextBox1_TextChanged;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            var settings = this.textBox1.Text.Split(';');
            foreach(var s in settings)
            {

                var p = s.Split('=');
                if(p.Length==2)
                {
                    var key = p[0];
                    var val = p[1];
                   
                    if(key.Equals("dims"))
                    {
                        var nval = val.Split(',');
                        if(nval.Length==2)
                        {
                            double w, h;
                            w = 1;
                            h = 1;
                            double.TryParse(nval[0], out w);
                            double.TryParse(nval[1], out h);

                            model.width = w;
                            model.height = h;
                        }
                    }
                }
            }
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private const int WS_EX_TOPMOST = 0x00000008;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_TOPMOST;
                return createParams;
            }
        }
    }
}
