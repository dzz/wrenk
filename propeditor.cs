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
            this.selected = p;
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

    }
}
