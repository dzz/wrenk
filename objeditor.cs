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
                this.selected.key = (String)this.keySelector.SelectedItem;
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
