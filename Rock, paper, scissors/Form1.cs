using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rock__paper__scissors
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                Bitmap OgImage = new Bitmap(openFileDialog1.FileName);
                Bitmap GImage = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = OgImage;

               Grayscale(GImage);
            }
        }

        private void Grayscale(Bitmap pic)
        {
            for (int i = 0; i < pic.Width; i++)
            {
                for (int j = 0; j < pic.Height; j++)
                {
                    Color sel = pic.GetPixel(i, j);

                    int a = sel.A;
                    int r = sel.R;
                    int g = sel.G;
                    int b = sel.B;

                    int CValue = (r + g + b) / 3;

                    pic.SetPixel(i, j, Color.FromArgb(a, CValue, CValue, CValue));

                    pictureBox2.Image = pic;
                }
            }
        }

    }
}
