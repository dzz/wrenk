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
    public partial class propeditor : Form
    {
        public PictureBox PictureBox;
        public ComboBox KeyBox;

        public prop selected = null;

        public void synch(prop p)
        {
            this.KeyBox.SelectedIndex = state.prop_images.IndexOf(p.image);
            this.pictureBox1.Image = p.image;

            this.listBox1.SelectedIndex = p.type;
            this.selected = p;

            this.specBox.Text = p.x.ToString() + ";" + p.y.ToString() + ";" + p.w.ToString() + ";" + p.h.ToString() + ";" + p.r.ToString();

            //if (this.selected.type == 0) radioButton1.Checked = true;
            //if (this.selected.type == 1) radioButton2.Checked = true;
            //if (this.selected.type == 2) radioButton3.Checked = true;
        }

        public propeditor()
        {
            InitializeComponent();
            PictureBox = this.pictureBox1;
            KeyBox = this.comboBox1;
            this.comboBox1.Items.AddRange(state.prop_names.ToArray());
            this.comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            //this.radioButton1.CheckedChanged += RadioButton1_CheckedChanged;
            //this.radioButton2.CheckedChanged += RadioButton2_CheckedChanged;
            //this.radioButton3.CheckedChanged += RadioButton3_CheckedChanged;

            this.listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.selected != null)
            {
                this.selected.type = this.listBox1.SelectedIndex;

            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pictureBox1.Image = state.prop_images[this.comboBox1.SelectedIndex];
            this.pictureBox1.Size = pictureBox1.Image.Size;

            if(this.selected != null)
            {
                this.selected.image = this.pictureBox1.Image;
                this.selected.image_key = (String)this.comboBox1.SelectedItem;
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void specBox_TextChanged(object sender, EventArgs e)
        {
            if (this.selected == null) return;

            var spec=  specBox.Text.Split(';');
            if(spec.Length==5)
            {
                double x, y, w, h, r;

                if( double.TryParse(spec[0], out x) )
                {
                    this.selected.x = x;
                }
                if (double.TryParse(spec[1], out y))
                {
                    this.selected.y = y;
                }
                if (double.TryParse(spec[2], out w))
                {
                    this.selected.w = w;
                }
                if (double.TryParse(spec[3], out h))
                {
                    this.selected.h = h;
                }
                if (double.TryParse(spec[4], out r))
                {
                    this.selected.r = r;
                }

            }

        }
    }
}
