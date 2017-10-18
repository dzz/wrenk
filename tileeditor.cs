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
    public partial class tileeditor : Form
    {
        static Random r = new Random();
        public Image tileset;
        public Image selected;
        public int selected_index;
        public List<int> selected_indices = new List<int>();

        public int get_print_tile()
        {
            if(selected_indices.Count>0)
            {
                return selected_indices[r.Next(selected_indices.Count)];
            } else
            {
                return selected_index;
            }

        }

        public tileeditor()
        {
            try
            {
                tileset = Image.FromFile("c:\\users\\dzz\\kthuune\\resources\\forest\\floor_tiles.png");
            } catch
            {
                MessageBox.Show("Default tileset missing. Please locate tileset image.");
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    tileset = Image.FromFile(ofd.FileName);
                } else
                {
                    Environment.Exit(0);
                }
            }
            InitializeComponent();
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.pictureBox1.Image = tileset;
            this.MouseClick += Tileeditor_MouseClick;
            this.pictureBox1.MouseClick += PictureBox1_MouseClick;
            selected = new Bitmap(32, 32);
            selected_index = 0;
        }

        public Rectangle get_source(int index)
        {
            int base_pixel_left = index * 32;
            int pixel_left = base_pixel_left % tileset.Width;
            int pixel_top = (base_pixel_left / (tileset.Width))*32;

            return new Rectangle(pixel_left, pixel_top, 32, 32);
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int xpin = (e.X / 32)*32;
            int ypin = (e.Y / 32)*32;
            Bitmap tile = new Bitmap(32, 32);

            Graphics g = Graphics.FromImage(tile);

            var sr = new Rectangle(xpin, ypin, 32, 32);
            var dr = new Rectangle(0, 0, 32, 32);
            g.DrawImage(this.tileset, dr, sr, GraphicsUnit.Pixel);

            this.selected = tile;
            pictureBox2.Image = tile;
            this.selected_index = ((xpin / 32) + ((ypin / 32) * (this.tileset.Width / 32)));
            if (e.Button == MouseButtons.Left)
            {
                this.selected_indices.RemoveAll(x => true);
                
            } else
            {
                this.selected_indices.Add(this.selected_index);
            }
           // MessageBox.Show(this.selected_index.ToString());
        }

        private void Tileeditor_MouseClick(object sender, MouseEventArgs e)
        {
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
    }

   

}
