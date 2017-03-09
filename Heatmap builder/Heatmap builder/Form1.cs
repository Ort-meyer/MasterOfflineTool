using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Heatmap_builder
{
    public partial class Form1 : Form
    {
        System.Globalization.NumberFormatInfo nf
          = new System.Globalization.NumberFormatInfo()
          {
              NumberGroupSeparator = "."
          };

        public class PixelLostInfo
        {
            public Point pixel { get; set; }
            public int lostCount { get; set; }
        }
        Bitmap b;
        Bitmap underlayingImage;
        public Form1()
        {
            InitializeComponent();
        }

        // Form functions
        private void GenerateHeatmap_Click(object sender, EventArgs e)
        {
            string[] allHeatmapFiles = AllHeatmapFilesInDirectory(InFolder.Text);

            for (int i = 0; i < allHeatmapFiles.Length; i++)
            {
                int highestNumberOfLost = 0;
                //int pixelsX = int.Parse(TextureWidth.Text);
                //int pixelsY = int.Parse(TextureHeight.Text);
                //int[,] lostPerPixel = new int[pixelsX, pixelsY];
                int minX, minY, maxX, maxY;
                List<int[]> allLostPositions = ReadRawData(out minX, out minY, out maxX, out maxY, allHeatmapFiles[i]);
                if (fitPositionsToImage.Checked)
                {
                    minX = int.Parse(imageLeftPositionX.Text);
                    minY = int.Parse(imageLowerPositionY.Text);
                    maxX = int.Parse(imageRightPositionX.Text);
                    maxY = int.Parse(imageUpperPositionY.Text);
                }
                    // sort the list so all the positions of the same type is next to eachother
                allLostPositions = allLostPositions.OrderBy(r => r[0]).ThenBy(r => r[1]).ToList();
                try
                {
                    underlayingImage = (Bitmap)Bitmap.FromFile(button1.Text);
                }
                catch (Exception)
                { }
                if (sizeFromImage.Checked)
                {
                    b = new Bitmap(underlayingImage.Width, underlayingImage.Height);
                }
                else
                {
                    b = new Bitmap(int.Parse(TextureWidth.Text), int.Parse(TextureHeight.Text));
                }
                Graphics g = Graphics.FromImage(b);
                
                g.Clear(Color.FromArgb(1, Color.Red));
                List<PixelLostInfo> pixelLost = new List<PixelLostInfo>();
                Point currentPixelAltered = new Point(-1,-1);
                for (int currentPosition = 0; currentPosition < allLostPositions.Count; currentPosition++)
                {
                    Point pixel = ConvertPositionToPixel(allLostPositions[currentPosition][0], allLostPositions[currentPosition][1],
                        minX,minY,maxX,maxY);
                    if (currentPixelAltered != pixel)
                    {
                        pixelLost.Add(new PixelLostInfo());
                        currentPixelAltered = pixel;
                    }
                    pixelLost[pixelLost.Count - 1].lostCount += 1;
                    pixelLost[pixelLost.Count - 1].pixel = pixel;
                    highestNumberOfLost = Math.Max(highestNumberOfLost, pixelLost[pixelLost.Count - 1].lostCount);
                }
                foreach (var pixel in pixelLost)
                {             
                    ColorPixels(pixel, ref b);
                }

                // We need to flip the heatmap, wrong way now
                b.RotateFlip(RotateFlipType.Rotate180FlipX);

                // Add the underlaying image to the given bitmap   
                Bitmap completedImage = MergeTwoImages(underlayingImage,b);

                Preview.Image = completedImage;
                string heatmapName = allHeatmapFiles[i].Substring(allHeatmapFiles[i].LastIndexOf("\\") + 1);
                string folderPath = Path.GetFullPath(OutFolder.Text);
                Directory.CreateDirectory(folderPath);
                b.Save(folderPath + "/" + heatmapName + ".png", ImageFormat.Png);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
         
        // Helper Functions
        private List<int[]> ReadRawData(out int minX, out int minY, out int maxX, out int maxY, string fileName)
        {
            List<int[]> returnList = new List<int[]>();
            string[] allLines = File.ReadAllLines(fileName);

            // Loop unroll to get start max min value
            int[] position = new int[2];
            string thisLine = allLines[0];
            string[] numbers = thisLine.Split(' ');
            position[0] = (int)double.Parse(numbers[0], CultureInfo.InvariantCulture);
            position[1] = (int)double.Parse(numbers[1], CultureInfo.InvariantCulture);
            returnList.Add(position);
            minX = position[0];
            minY = position[1];
            maxX = minX;
            maxY = minY;

            for (int i = 1; i < allLines.Length; i++)
            {
                position = new int[2];
                thisLine = allLines[i];
                numbers = thisLine.Split(' ');
                position[0] = (int)double.Parse(numbers[0], CultureInfo.InvariantCulture);
                position[1] = (int)double.Parse(numbers[1], CultureInfo.InvariantCulture);
                returnList.Add(position);
                minX = Math.Min(minX, position[0]);
                minY = Math.Min(minY, position[1]);
                maxX = Math.Max(maxX, position[0]);
                maxY = Math.Max(maxY, position[1]);
            }
            return returnList;
        }

        private string[] AllHeatmapFilesInDirectory(string directory)
        {
            string[] allFiles = Directory.GetFiles(directory, "*.heatmap");
            return allFiles;
        }

        private Point ConvertPositionToPixel(int x, int y, int minX, int minY, int maxX, int maxY)
        {
            Point returnPoint = new Point();

            x = x - minX; // make it go from 0
            y = y - minY; // make it go from 0
            maxX = maxX - minX; // make it go from 0
            maxY = maxY - minY; // make it go from 0
            returnPoint.X = (int)(((float)x / (float)maxX) * (b.Width - 1));
            returnPoint.Y = (int)(((float)y / (float)maxY) * (b.Height - 1));         
            

            return returnPoint;
        }

        private void ColorPixels(PixelLostInfo pixel, ref Bitmap map)
        {
            int propagationLevel = int.Parse(propagation.Text);
            int halfPropagation = propagationLevel / 2;
            float maxDistanceX = Math.Abs(propagationLevel);
            float maxDistanceY = Math.Abs(propagationLevel);
            float maxDistance = (float)Math.Sqrt((maxDistanceX * maxDistanceX + maxDistanceY * maxDistanceY));
            for (int x = pixel.pixel.X - (int)maxDistance; x <= pixel.pixel.X + (int)maxDistance; x++)
            {
                for (int y = pixel.pixel.Y - (int)maxDistance; y <= pixel.pixel.Y + (int)maxDistance; y++)
                {
                    float distanceX = Math.Abs(pixel.pixel.X - x);
                    float distanceY = Math.Abs(pixel.pixel.Y - y);
                    float distance = (float)Math.Sqrt((distanceX * distanceX + distanceY * distanceY));
                    float alpha = 255.0f * (1.0f - Math.Min(distance / maxDistance,1.0f));
                    if (float.IsNaN(alpha))
                    {
                        // this happens when propagation is set to 1
                        alpha = 255.0f;
                    }
                    if (x < 0 || x > map.Width - 1 || y < 0 || y > map.Height - 1)
                    {
                        continue;
                    }
                    float previousAlpha = b.GetPixel(x, y).A;
                    b.SetPixel(x, y, Color.FromArgb((int) Math.Max(previousAlpha , alpha), Color.Red));
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                button1.Text = file;
                try
                {
                    string text = File.ReadAllText(file);
                    size = text.Length;
                }
                catch (IOException)
                {
                }
            }
            Console.WriteLine(size); // <-- Shows file size in debugging mode.
            Console.WriteLine(result); // <-- For debugging use.
        }

        private void fitPositionsToImage_CheckedChanged(object sender, EventArgs e)
        {
            imageLowerPositionY.Enabled = fitPositionsToImage.Checked;
            imageUpperPositionY.Enabled = fitPositionsToImage.Checked;
            imageLeftPositionX.Enabled = fitPositionsToImage.Checked;
            imageRightPositionX.Enabled = fitPositionsToImage.Checked;
        }

        private void sizeFromImage_CheckedChanged(object sender, EventArgs e)
        {
            TextureHeight.Enabled = !sizeFromImage.Checked;
            TextureWidth.Enabled = !sizeFromImage.Checked;
        }

        public static Bitmap MergeTwoImages(Image firstImage, Image secondImage)
        {
            if (firstImage == null)
            {
                throw new ArgumentNullException("firstImage");
            }

            if (secondImage == null)
            {
                throw new ArgumentNullException("secondImage");
            }

            int outputImageWidth = firstImage.Width > secondImage.Width ? firstImage.Width : secondImage.Width;

            int outputImageHeight = firstImage.Height + secondImage.Height + 1;

            Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(firstImage, new Rectangle(new Point(), firstImage.Size),
                    new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(secondImage, new Rectangle(new Point(), secondImage.Size),
                    new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }
    }
}
