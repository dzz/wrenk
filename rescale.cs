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
    public partial class rescale : Form
    {
        public rescale()
        {
            InitializeComponent();
        }

        public double getVal()
        {
            double v = 1.0;
            double.TryParse(this.textBox1.Text, out v);


            return v;
        }
    }
}
