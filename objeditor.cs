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
    public partial class objeditor : Form
    {
        public obj selected;
        public void synch(obj o)
        {
            this.jsonEditor.Text = o.json;
            this.keySelector.Text = o.key;

            this.regionCheckbox.Checked = o.region;
            this.widthBox.Text = o.w.ToString();
            this.heightBox.Text = o.h.ToString();
            this.selected = o;
        }


        public ComboBox getKeySelector()
        {
            return this.keySelector;
        }

        public TextBox getJsonEditor()
        {
            return this.jsonEditor;
        }

        public objeditor()
        {
            InitializeComponent();
            this.keySelector.SelectedIndex = 0;
            this.selected = null;
            this.keySelector.TextChanged += KeySelector_TextChanged;
            this.jsonEditor.TextChanged += JsonEditor_TextChanged;
            this.widthBox.TextChanged += WidthBox_TextChanged;
            this.heightBox.TextChanged += HeightBox_TextChanged;
            this.regionCheckbox.CheckedChanged += RegionCheckbox_CheckedChanged;
        }

        private void RegionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.selected != null)
            {
                this.selected.region = this.regionCheckbox.Checked;
            }
        }

        private void HeightBox_TextChanged(object sender, EventArgs e)
        {
            if (this.selected != null)
            {
                double d = 0.0;
                double.TryParse(this.heightBox.Text, out d);
                this.selected.h = d;
            }
        }

        private void WidthBox_TextChanged(object sender, EventArgs e)
        {

            if (this.selected != null)
            {
                double d = 0.0;
                double.TryParse(this.widthBox.Text, out d);
                this.selected.w = d;
            }
        }

        private void JsonEditor_TextChanged(object sender, EventArgs e)
        {
            if(this.selected != null)
            {
                this.selected.json = this.jsonEditor.Text;
            }
        }

        private void KeySelector_TextChanged(object sender, EventArgs e)
        {
            if(this.selected != null)
            {
                this.selected.key = (String)this.keySelector.Text;
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
