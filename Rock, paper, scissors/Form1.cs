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
        Bitmap OgImage;
        Bitmap GImage;
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                OgImage = new Bitmap(openFileDialog1.FileName);
                GImage = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = OgImage;

                Frame(GImage);

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

        private void Frame(Bitmap pic)
        {
            for (int i = 0; i < pic.Width; i++)
            {
                for (int j = 0; j < pic.Height; j++)
                {
                    if (i==0 || j==0 || i==pic.Width-1 || j==pic.Height-1) {
                        pic.SetPixel(i, j, Color.Black);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void Threshold(Bitmap pic)
        {
            int x, y;

            int T = Convert.ToInt32(threshold1.Value);
            Bitmap temp = (Bitmap)pic.Clone();

            for (x = 0; x < temp.Width; x++)
            {
                //Console.WriteLine();
                for (y = 0; y < temp.Height; y++)
                {
                    Color pixelColor = temp.GetPixel(x, y);
                    Color black = Color.FromArgb(0, 0, 0);
                    Color white = Color.FromArgb(255, 255, 255);
                    //Console.Write(((pixelColor.R + pixelColor.G + pixelColor.B))/ 3 + " ");
                    if (((pixelColor.R + pixelColor.G + pixelColor.B)) / 3 < T)
                    {
                        temp.SetPixel(x, y, black);
                    }
                    else
                    {
                        temp.SetPixel(x, y, white);
                    }
                }
            }
            pictureBox2.Image = temp;
        }

        private void threshold1_Scroll(object sender, EventArgs e)
        {
            Threshold(GImage);
        }
    }
}
