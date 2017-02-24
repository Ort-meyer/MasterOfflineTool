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
        public class PixelLostInfo
        {
            public Point pixel { get; set; }
            public int lostCount { get; set; }
        }
        Bitmap b;
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
                int pixelsX = int.Parse(TextureWidth.Text);
                int pixelsY = int.Parse(TextureHeight.Text);
                int[,] lostPerPixel = new int[pixelsX, pixelsY];
                int minX, minY, maxX, maxY;
                List<int[]> allLostPositions = ReadRawData(out minX, out minY, out maxX, out maxY, allHeatmapFiles[i]);
                // sort the list so all the positions of the same type is next to eachother
                allLostPositions= allLostPositions.OrderBy(r => r[0]).ThenBy(r => r[1]).ToList();
                b = new Bitmap(int.Parse(TextureWidth.Text), int.Parse(TextureHeight.Text));
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
                    float alpha = ((float)pixel.lostCount / (float)highestNumberOfLost) * 255.0f;
                    b.SetPixel(pixel.pixel.X, pixel.pixel.Y, Color.FromArgb((int)alpha, Color.Red));
                }
                
                Preview.Image = b;
                string heatmapName = allHeatmapFiles[i].Substring(allHeatmapFiles[i].LastIndexOf("\\") + 1);
                string folderPath = Path.GetFullPath(OutFolder.Text);
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
            returnPoint.X = (int)(((float)x / (float)maxX) * (float.Parse(TextureWidth.Text) - 1));
            returnPoint.Y = (int)(((float)y / (float)maxY) * (float.Parse(TextureWidth.Text) - 1));
            return returnPoint;
        }

    }
}
