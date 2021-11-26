using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Rock__paper__scissors
{

    public partial class Form1 : Form
    {
        Bitmap OgImage;
        Bitmap GImage;
        Bitmap TImage;
        Bitmap Check;
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                label1.Text = "";
                OgImage = new Bitmap(openFileDialog1.FileName);
                GImage = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = OgImage;

                Crop(OgImage);
                Crop(GImage);

                Frame(GImage);

                Grayscale(GImage);
                Threshold(GImage, 100);
                AutomaticThreshold(GImage,TImage,threshold1.Value);

                Check = new Bitmap(pictureBox2.Image);
                CheckImage(Check);
                Evaluate();

            }
        }

        private void Evaluate()
        {
            if (blackpercent > 33)
            {
                label1.Text = "Olló";
            }
        }

        private double whitepercent;
        private double blackpercent;
        private void CheckImage(Bitmap G)
        {
            Color R = Color.FromArgb(255, 0, 0);
            Color B = Color.FromArgb(0, 0, 0);
            Color W = Color.FromArgb(255, 255, 255);
            int countblack = 0;
            int countwhite = 0;

            try
            {
                for (int i = 0; i < G.Width; i++)
                {
                    for (int j = 0; j < G.Height; j++)
                    {
                        if (G.GetPixel(i, j) == R)
                        {
                            for (int z = i + 2; z < G.Height; z++)
                            {
                                //countblack++;
                                if (G.GetPixel(z, j) == W)
                                {
                                    countwhite++;
                                }

                                else
                                {
                                    countblack++;
                                }

                                if (G.GetPixel(z, j) == R)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            whitepercent = (double)countwhite / (countblack + countwhite) * 100;
            blackpercent = (double)countblack / (countblack + countwhite) * 100;
            Console.WriteLine("Black percent: " + blackpercent);
            Console.WriteLine("White percent: " + whitepercent);
        }
        private void Crop(Bitmap b)
        {
            Bitmap nb = new Bitmap(b.Width, b.Height);
            using (Graphics g = Graphics.FromImage(nb))
            {
                g.DrawImage(b, -10, 0);
                GImage = nb;
            }
        }

        private void AutomaticThreshold(Bitmap pic, Bitmap tpic, int fixval)
        {
            //Scan image for white pixel percentage
            int x, y;
            Bitmap temp = (Bitmap)tpic.Clone();
            float whitePixels = 0;
            float blackPixels = 0;
            int fixValue = fixval;
            for (x = 0; x < temp.Width; x++)
            {
                for (y = 0; y < temp.Height; y++)
                {
                    Color pixelColor = temp.GetPixel(x, y);
                    Color black = Color.FromArgb(0, 0, 0);
                    Color white = Color.FromArgb(255, 255, 255);
                    if (pixelColor == white)
                    {
                        whitePixels++;
                    }
                    else if (pixelColor == black)
                    {
                        blackPixels++;
                    }
                }
            }
            //Console.WriteLine("White pixels: " + whitePixels);
            //Console.WriteLine("Black pixels: " + blackPixels);
            Threshold(GImage, fixValue);
            Console.WriteLine("Fehér pixelek százaléka: " +  whitePixels / (blackPixels + whitePixels) * 100);
            if (whitePixels/(blackPixels+whitePixels)*100 > 35)
            {
                fixValue++;
                AutomaticThreshold(OgImage, TImage, fixValue);
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

        private void Threshold(Bitmap pic, int value)
        {
            int x, y;

            int T = value;
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
            threshold1.Value = value;
            TImage = temp;
            convexHull(temp);
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
            TImage = temp;
            convexHull(temp);
        }

        private void threshold1_Scroll(object sender, EventArgs e)
        {
            Threshold(GImage);
        }

        public static int orientation(Point p, Point q, Point r)
        {
            int val = (q.y - p.y) * (r.x - q.x) -
                    (q.x - p.x) * (r.y - q.y);

            if (val == 0) return 0; // collinear
            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }
            public void convexHull(Bitmap pic)
            {

            int x, y;

            int n = 0;

            Color black = Color.FromArgb(0, 0, 0);
            Color white = Color.FromArgb(255, 255, 255);

            Bitmap temp = (Bitmap)pic.Clone();

            Point[] points = new Point[temp.Width * temp.Height];

            for (x = 0; x < temp.Width; x++)
            {
                //Console.WriteLine();
                for (y = 0; y < temp.Height; y++)
                {
                    Color pixelColor = temp.GetPixel(x, y);

                    if (pixelColor == white)
                    {
                        points[n] = new Point(x, y);
                        n++;
                    }
                }
            }
            int hullpoints = 0;
            // There must be at least 3 points
            if (n < 3) return;

                // Initialize Result
                List<Point> hull = new List<Point>();

                // Find the leftmost point
                int l = 0;
            for (int i = 1; i < n; i++)
            {
                if (points[i].x < points[l].x)
                    l = i;
            }

                // Start from leftmost point, keep moving
                // counterclockwise until reach the start point
                // again. This loop runs O(h) times where h is
                // number of points in result or output.
                int p = l, q;
                do
                {
                    // Add current point to result
                    hull.Add(points[p]);
                hullpoints++;

                    // Search for a point 'q' such that
                    // orientation(p, q, x) is counterclockwise
                    // for all points 'x'. The idea is to keep
                    // track of last visited most counterclock-
                    // wise point in q. If any point 'i' is more
                    // counterclock-wise than q, then update q.
                    q = (p + 1) % n;

                    for (int i = 0; i < n; i++)
                    {
                        // If i is more counterclockwise than
                        // current q, then update q
                        if (orientation(points[p], points[i], points[q])
                                                            == 2)
                            q = i;
                    }

                    // Now q is the most counterclockwise with
                    // respect to p. Set p as q for next iteration,
                    // so that q is added to result 'hull'
                    p = q;

                } while (p != l); // While we don't come to first
                                  // point

                // Print Result
                /*foreach (Point tempp in hull)
                    Console.WriteLine("(" + tempp.x + ", " +
                                        tempp.y + ")");

            Console.WriteLine();
            Console.WriteLine("-----------------");
            Console.WriteLine();*/

            Pen redPen = new Pen(Color.Red, 3);

            for (int i = 1; i < hullpoints; i++)
            {
                using (var graphics = Graphics.FromImage(temp))
                {
                    graphics.DrawLine(redPen, hull[i].x, hull[i].y, hull[i-1].x, hull[i-1].y);
                }
            }
            using (var graphics = Graphics.FromImage(temp))
            {
                graphics.DrawLine(redPen, hull[0].x, hull[0].y, hull[hullpoints - 1].x, hull[hullpoints-1].y);
            }

            pictureBox2.Image = temp;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    public class Point
    {
        public int x, y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
