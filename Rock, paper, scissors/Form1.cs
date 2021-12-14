using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Rock__paper__scissors
{

    public partial class Form1 : Form
    {
        Bitmap OgImage;
        Bitmap GImage;
        Bitmap TImage;
        Bitmap Check;

        private int talaltOllo = 0;
        private int talaltKo = 0;
        private int talaltPapir = 0;
        private int HullCount = 0;
        //private int InnerLine=0;

        private int leghosszabb = 0;
        private int MaxBlackLenght = 0;






        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                leghosszabb = 0;
                MaxBlackLenght = 0;
                //InnerLine = 0;

                label1.Text = "";
                OgImage = new Bitmap(openFileDialog1.FileName);
                GImage = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = OgImage;

                Crop(OgImage);
                Crop(GImage);

                Frame(GImage);

                Grayscale(GImage);
                Threshold(GImage, 100);



                AutomaticThreshold(GImage, TImage, threshold1.Value);

                Check = new Bitmap(pictureBox2.Image);
                CheckImage(Check);
                Evaluate();
                pictureBox3.Image.Save("out.bmp");


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                
                //openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath;

                string[] dirs = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
                int szam = 1;
               // InnerLine = 0;

                foreach (string selected in dirs)
                {
                    MaxBlackLenght = 0;
                    Console.WriteLine("Képszáma: " + szam);
                    double olloSzazalek = (double)(talaltOllo * 100) / szam;
                    Console.WriteLine("Olló találat százaléka: " + Math.Round(olloSzazalek, 2) + "%");
                    double koSzazalek = (double)(talaltKo * 100) / szam;
                    Console.WriteLine("Kő találat százaléka: " + Math.Round(koSzazalek, 2) + "%");
                    double papirSzazalek = (double)(talaltPapir * 100) / szam;
                    Console.WriteLine("Papír találat százaléka: " + Math.Round(papirSzazalek, 2) + "%");
                    szam++;
                    label1.Text = "";
                    OgImage = new Bitmap(selected);
                    GImage = new Bitmap(selected);
                    pictureBox1.Image = OgImage;

                    Crop(OgImage);
                    Crop(GImage);

                    Frame(GImage);

                    Grayscale(GImage);
                    Threshold(GImage, 100);



                    AutomaticThreshold(GImage, TImage, threshold1.Value);

                    Check = new Bitmap(pictureBox2.Image);
                    CheckImage(Check);
                    Evaluate();
                    //pictureBox2.Image.Save(szam+".bmp");
                }

            }
        }

        private void Evaluate()
        {



            if (99 < MaxBlackLenght || blackpercent>40)
            {
                label1.Text = "Olló";
                talaltOllo++;
                Console.WriteLine("Talált olló: " + talaltOllo);
            }

            else if (52 < MaxBlackLenght && 99 > MaxBlackLenght || blackpercent < 40 && 20<blackpercent)
            {
                label1.Text = "Papír";
                talaltPapir++;
                Console.WriteLine("Talált papír: " + talaltPapir);
            }

            else if (50 > MaxBlackLenght || 20 > blackpercent && blackpercent > 0)
            {
                label1.Text = "Kő";
                talaltKo++;
                Console.WriteLine("Talált kő: " + talaltKo);
            }





        }

        private double whitepercent;
        private double blackpercent;
        private int elso;
        private int utolso;
        private int FirstWhite;
        
        private void CheckImage(Bitmap G)
        {
            leghosszabb = 0;
            Color R = Color.FromArgb(255, 0, 0);
            Color B = Color.FromArgb(0, 0, 0);
            Color W = Color.FromArgb(255, 255, 255);
            int countblack = 0;
            int countwhite = 0;
            int BlackLenght=0;


            for (int j = 0; j < G.Height; j++)
            {
                int hossz = 0;
                
                for (int i = 0; i < G.Width; i++)
                {
                    if (G.GetPixel(i, j) == R)
                    {
                        int foundred = 0;
                        

                        for (int z = i; z < G.Width - 20; z++)
                        {

                            // Console.WriteLine("z: " + z);
                            //countblack++;





                            if (G.GetPixel(z, j) == R)
                            {
                                
                                foundred++;
                                if (foundred == 1)
                                {
                                    
                                    elso = z;
                                }
                                // Console.WriteLine("Piros: "+foundred);

                                utolso = z;
                               
                                // Console.WriteLine("Z ertek: " + z);
                            }




                        }
                        // Console.WriteLine("elso: " + elso + "utolso: " + utolso);
                        if (utolso > 200)
                        {
                            utolso = 200;
                        }
                        
                        int foundwhite = 0;
                        for (int k = elso; k < utolso; k++)
                        {
                            
                            if (G.GetPixel(k, j) == W)
                            {
                                //Console.WriteLine("z: "+z+"j: "+j);
                                countwhite++;
                                hossz ++;
                                
                            }

                            else if (G.GetPixel(k, j) == B)
                            {
                                if (hossz > leghosszabb)
                                {
                                    
                                    leghosszabb = hossz;
                                    //Console.WriteLine("Utolso hossz: "+k+", "+j);
                                    
                                }
                                hossz = 0;
                                countblack++;
                                

                            }

                            if (G.GetPixel(k, j) == W)
                            {
                                foundwhite++;

                                 if (foundwhite == 1 )
                                {
                                    FirstWhite = k;
                                    BlackLenght = FirstWhite - elso;
                                   for(int d = elso; d < FirstWhite; d++)
                                    {
                                        //Console.WriteLine("d:"+d);
                                        G.SetPixel(d, j, Color.Blue);
                                    }
                                    //Console.WriteLine("Koordinatak: "+k+", "+j);
                                }
                                // Console.WriteLine("Blacklenght: " + BlackLenght);
                                

                            }else if(foundwhite==0 && k == 199)
                            {
                                BlackLenght = 200 - elso;
                                for (int d = elso; d < 199; d++)
                                {
                                    //Console.WriteLine("d:" + d);
                                    G.SetPixel(d, j, Color.Blue);
                                }
                            }

                            
                            if (BlackLenght > MaxBlackLenght)
                            {
                                MaxBlackLenght = BlackLenght;

                            }
                        
                            //Console.WriteLine(" Tav: " + InnerLine + "Koordinatak: " + k + ", " + j);
                        }

                    }
                   
                }
            }
    
           // Console.WriteLine("Leghosszabb: "+ leghosszabb);

            pictureBox3.Image = G;
            whitepercent = (double)countwhite / (countblack + countwhite) * 100;
            blackpercent = (double)countblack / (countblack + countwhite) * 100;
            Console.WriteLine("Black percent: " + blackpercent);
            Console.WriteLine("White percent: " + whitepercent);
            //Console.WriteLine("White pixel: " + countwhite);
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
            int x;
            Bitmap temp = (Bitmap)tpic.Clone();
            float whitePixels = 0;
            float blackPixels = 0;
            int fixValue = fixval;

            unsafe
            {
                BitmapData bitmapData = temp.LockBits(new Rectangle(0, 0, temp.Width, temp.Height), ImageLockMode.ReadWrite, temp.PixelFormat);
                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(temp.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {

                        int oldBlue = currentLine[x];
                        int oldGreen = currentLine[x + 1];
                        int oldRed = currentLine[x + 2];

                        if (oldBlue == 255 && oldGreen == 255 && oldRed == 255)
                        {
                            whitePixels++;
                        }
                        else if (oldBlue == 0 && oldGreen == 0 && oldRed == 0)
                        {
                            blackPixels++;
                        }
                    }
                });
                //Console.WriteLine("White pixels: " + whitePixels);
                //Console.WriteLine("Black pixels: " + blackPixels);
                Threshold(GImage, fixValue);
                //Console.WriteLine("Fehér pixelek százaléka: " + whitePixels / (blackPixels + whitePixels) * 100);
                if (whitePixels / (blackPixels + whitePixels) * 100 > 35)
                {
                    fixValue = fixValue + 10;
                    AutomaticThreshold(OgImage, TImage, fixValue);
                }
                temp.UnlockBits(bitmapData);
            }
        }

        private void Grayscale(Bitmap pic)
        {
            unsafe { 
            BitmapData bitmapData = pic.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), ImageLockMode.ReadWrite, pic.PixelFormat);
            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(pic.PixelFormat) / 8;
            int heightInPixels = bitmapData.Height;
            int widthInBytes = bitmapData.Width * bytesPerPixel;
            byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                        //Color pixelColor = temp.GetPixel(x, y);
                        //Color black = Color.FromArgb(0, 0, 0);
                        //Color white = Color.FromArgb(255, 255, 255);

                        int oldBlue = currentLine[x];
                        int oldGreen = currentLine[x + 1];
                        int oldRed = currentLine[x + 2];

                        int CValue = (oldRed + oldGreen + oldBlue) / 3;

                        currentLine[x] = (byte)CValue;
                        currentLine[x + 1] = (byte)CValue;
                        currentLine[x + 2] = (byte)CValue;


                }
            });


            pic.UnlockBits(bitmapData);
            pictureBox2.Image = pic;
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
            unsafe
            {

                BitmapData bitmapData = temp.LockBits(new Rectangle(0, 0, temp.Width, temp.Height), ImageLockMode.ReadWrite, temp.PixelFormat);
                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(temp.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                for (y = 0; y < heightInPixels; y++)
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        //Color pixelColor = temp.GetPixel(x, y);
                        //Color black = Color.FromArgb(0, 0, 0);
                        //Color white = Color.FromArgb(255, 255, 255);

                        int oldBlue = currentLine[ x];
                        int oldGreen = currentLine[x + 1];
                        int oldRed = currentLine[x + 2];

                        if (((oldRed + oldGreen + oldBlue)) / 3 < T)
                        {
                            currentLine[x] = 0;
                            currentLine[x + 1] = 0;
                            currentLine[x + 2] = 0;
                        }
                        else
                        {
                            currentLine[x] = 255;
                            currentLine[x + 1] = 255;
                            currentLine[x + 2] = 255;
                        }
                    }
                }

              
                temp.UnlockBits(bitmapData);
            }
            pictureBox2.Image = temp;
            threshold1.Value = value;
            TImage = temp;
            convexHull(temp);
        }

        private void Threshold(Bitmap pic)
        {
            int x;

            int T = Convert.ToInt32(threshold1.Value);
            Bitmap temp = (Bitmap)pic.Clone();

            unsafe
            {

                BitmapData bitmapData2 = temp.LockBits(new Rectangle(0, 0, temp.Width, temp.Height), ImageLockMode.ReadWrite, temp.PixelFormat);
                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(temp.PixelFormat) / 8;
                int heightInPixels = bitmapData2.Height;
                int widthInBytes = bitmapData2.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData2.Scan0;

                for (int y = 0; y < heightInPixels; y++)
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData2.Stride);
                    for (x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        //Color pixelColor = temp.GetPixel(x, y);
                        //Color black = Color.FromArgb(0, 0, 0);
                        //Color white = Color.FromArgb(255, 255, 255);

                        int oldBlue = currentLine[x];
                        int oldGreen = currentLine[x + 1];
                        int oldRed = currentLine[x + 2];

                        if (((oldRed + oldGreen + oldBlue)) / 3 < T)
                        {
                            currentLine[x] = 0;
                            currentLine[x + 1] = 0;
                            currentLine[x + 2] = 0;
                        }
                        else
                        {
                            currentLine[x] = 255;
                            currentLine[x + 1] = 255;
                            currentLine[x + 2] = 255;
                        }
                    }
                }

               
                temp.UnlockBits(bitmapData2);
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

            Pen redPen = new Pen(Color.Red, 1);

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
            HullCount = hull.Count;
            Console.WriteLine("Hullok szama: "+hull.Count);

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
