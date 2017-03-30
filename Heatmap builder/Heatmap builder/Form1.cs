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
        public string mergedFileName = "NotSet";
        public bool onlyShowHitsOnGolden = true;

        private Color wrongLost;
        private Color goldenLost;
        private Color correctLost;

        int blueValue;

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
        public Form1()
        {
            InitializeComponent();
            wrongLost = Color.Red;
            SetPreviewColor(ref ColorWrongLost, wrongLost);
            goldenLost = Color.FromArgb(0,Color.Blue);
            SetPreviewColor(ref ColorGoldenLost, goldenLost);
            correctLost = Color.Green;
            SetPreviewColor(ref ColorCorrectLost, correctLost);
        }

        // Form functions
        public void GenerateHeatmap_Click(object sender, EventArgs e)
        {
            // These arrays should have the same length
            string[] goldenAllHeatmapFiles = AllHeatmapFilesInDirectory(inFolderGoldenData.Text);
            string[] networkAllHeatmapFiles = AllHeatmapFilesInDirectory(InFolderNetworkData.Text);

            if (goldenAllHeatmapFiles != null)
            {
                goldenAllHeatmapFiles.OrderBy(q => q);
            }
            networkAllHeatmapFiles.OrderBy(q => q);

            for (int i = 0; i < networkAllHeatmapFiles.Length; i++)
            {
                int minX, minY, maxX, maxY;
                List<int[]> goldenAllLostPositions = new List<int[]>();
                List<int[]> networkAllLostPositions = ReadRawData(out minX, out minY, out maxX, out maxY, networkAllHeatmapFiles[i]);
                if (networkAllLostPositions == null)
                {
                    continue;
                }
                if (fitPositionsToImage.Checked)
                {
                    minX = int.Parse(imageLeftPositionX.Text);
                    minY = int.Parse(imageLowerPositionY.Text);
                    maxX = int.Parse(imageRightPositionX.Text);
                    maxY = int.Parse(imageUpperPositionY.Text);
                }
                else if (goldenAllHeatmapFiles != null)
                {
                    int tminX, tminY, tmaxX, tmaxY;
                    goldenAllLostPositions = ReadRawData(out tminX, out tminY, out tmaxX, out tmaxY, goldenAllHeatmapFiles[i]);
                    if (goldenAllLostPositions == null)
                    {
                        goldenAllLostPositions = new List<int[]>();
                    }
                    minX = Math.Min(minX, tminX);
                    minY = Math.Min(minY, tminY);
                    maxX = Math.Max(maxX, tmaxX);
                    maxY = Math.Max(maxY, tmaxY);
                }

                Bitmap completedImage = GetheatmapFromPositions(minX, minY, maxX, maxY, networkAllLostPositions, goldenAllLostPositions);

                Preview.Image = completedImage;
                string heatmapName = networkAllHeatmapFiles[i].Substring(networkAllHeatmapFiles[i].LastIndexOf("\\") + 1);
                string folderPath = Path.GetFullPath(OutFolder.Text);
                Directory.CreateDirectory(folderPath);
                completedImage.Save(folderPath + "/" + heatmapName + ".png", ImageFormat.Png);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        public void button2_Click(object sender, EventArgs e)
        {
            string[] goldenAllHeatmapFiles = AllHeatmapFilesInDirectory(inFolderGoldenData.Text);
            string[] networkAllHeatmapFiles = AllHeatmapFilesInDirectory(InFolderNetworkData.Text);

            if (goldenAllHeatmapFiles != null)
            {
                goldenAllHeatmapFiles.OrderBy(q => q);
            }
            networkAllHeatmapFiles.OrderBy(q => q);

            // Figure out min and max while reading all data.

            List<List<int[]>> networkAllPositions = new List<List<int[]>>();
            List<List<int[]>> goldenAllPositions = new List<List<int[]>>();

            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;
            for (int i = 0; i < networkAllHeatmapFiles.Length; i++)
            {
                int tminX, tminY, tmaxX, tmaxY;
                networkAllPositions.Add(ReadRawData(out tminX, out tminY, out tmaxX, out tmaxY, networkAllHeatmapFiles[i]));
                minX = Math.Min(minX, tminX);
                minY = Math.Min(minY, tminY);
                maxX = Math.Max(maxX, tmaxX);
                maxY = Math.Max(maxY, tmaxY);
            }
            if (goldenAllHeatmapFiles != null)
            {
                for (int i = 0; i < goldenAllHeatmapFiles.Length; i++)
                {
                    int tminX, tminY, tmaxX, tmaxY;
                    goldenAllPositions.Add(ReadRawData(out tminX, out tminY, out tmaxX, out tmaxY, goldenAllHeatmapFiles[i]));
                    minX = Math.Min(minX, tminX);
                    minY = Math.Min(minY, tminY);
                    maxX = Math.Max(maxX, tmaxX);
                    maxY = Math.Max(maxY, tmaxY);
                }
            }

            if (fitPositionsToImage.Checked)
            {
                minX = int.Parse(imageLeftPositionX.Text);
                minY = int.Parse(imageLowerPositionY.Text);
                maxX = int.Parse(imageRightPositionX.Text);
                maxY = int.Parse(imageUpperPositionY.Text);
            }

            blueValue = 0;
            // We have now read all the data, we trust that there is a 1:1 relation ship between golden and calc
            Bitmap completedImage = null;
            for (int i = 0; i < networkAllPositions.Count; i++)
            {
                List<int[]> networkToSend = networkAllPositions[i] != null ? networkAllPositions[i] : new List<int[]>();
                List<int[]> goldenToSend = new List<int[]>();
                if (goldenAllHeatmapFiles != null)
                {
                    goldenToSend = goldenAllPositions[i] != null ? goldenAllPositions[i] : new List<int[]>();
                }

                Bitmap b = GetheatmapFromPositions(minX, minY, maxX, maxY, networkToSend, goldenToSend);

                if (completedImage == null)
                {
                    completedImage = b;
                }
                else
                {
                    completedImage = MergeTwoImages(completedImage,b);
                }
                blueValue += 255 / networkAllPositions.Count;
            }

            Preview.Image = completedImage;
            string heatmapName = mergedFileName;
            string folderPath = Path.GetFullPath(OutFolder.Text);
            Directory.CreateDirectory(folderPath);
            completedImage.Save(folderPath + "/" + heatmapName + ".png", ImageFormat.Png);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        // Helper Functions
        private List<int[]> ReadRawData(out int minX, out int minY, out int maxX, out int maxY, string fileName)
        {
            string[] allLines = File.ReadAllLines(fileName);
            if (allLines.Length == 0)
            {
                minX = int.MaxValue;
                minY = int.MaxValue;
                maxX = int.MinValue;
                maxY = int.MinValue;
                return null;
            }
            List<int[]> returnList = new List<int[]>();

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
            string[] allFiles = null;
            try
            {

                allFiles = Directory.GetFiles(directory, "*.heatmap");
            }
            catch (Exception)
            {
            }
            return allFiles;
        }

        private Point ConvertPositionToPixel(int x, int y, int minX, int minY, int maxX, int maxY, Bitmap image)
        {
            Point returnPoint = new Point();

            x = x - minX; // make it go from 0
            y = y - minY; // make it go from 0
            maxX = maxX - minX; // make it go from 0
            maxY = maxY - minY; // make it go from 0
            returnPoint.X = (int)(((float)x / (float)maxX) * (image.Width - 1));
            returnPoint.Y = (int)(((float)y / (float)maxY) * (image.Height - 1));         
            

            return returnPoint;
        }

        private void ColorPixels(PixelLostInfo pixel, ref Bitmap map, Color underlayingColor)
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
                    float alpha = 255f * (1.0f - Math.Min(distance / maxDistance,1.0f));
                    if (float.IsNaN(alpha))
                    {
                        // this happens when propagation is set to 1
                        alpha = 255.0f;
                    }
                    if (x < 0 || x > map.Width - 1 || y < 0 || y > map.Height - 1)
                    {
                        continue;
                    }
                    float previousAlpha = map.GetPixel(x, y).A;
                    map.SetPixel(x, y, Color.FromArgb((int) Math.Max(previousAlpha , alpha), underlayingColor));
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

        private Bitmap GetheatmapFromPositions(int minX, int minY, int maxX, int maxY, List<int[]> networkAllLostPositions, List<int[]> goldenAllLostPositions)
        {
            Bitmap completedImage;
            Bitmap b;
            Bitmap underlayingImage = null;

            int highestNumberOfLost = 0;
            // sort the list so all the positions of the same type is next to eachother
            networkAllLostPositions = networkAllLostPositions.OrderBy(r => r[0]).ThenBy(r => r[1]).ToList();
            goldenAllLostPositions = goldenAllLostPositions.OrderBy(r => r[0]).ThenBy(r => r[1]).ToList();

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

            // Find out all the time the network guessed lost
            List<PixelLostInfo> networkPixelLost = new List<PixelLostInfo>();
            Point currentPixelAltered = new Point(-1, -1);
            for (int currentPosition = 0; currentPosition < networkAllLostPositions.Count; currentPosition++)
            {
                Point pixel = ConvertPositionToPixel(networkAllLostPositions[currentPosition][0], networkAllLostPositions[currentPosition][1],
                    minX, minY, maxX, maxY, b);
                if (currentPixelAltered != pixel)
                {
                    networkPixelLost.Add(new PixelLostInfo());
                    currentPixelAltered = pixel;
                }
                networkPixelLost[networkPixelLost.Count - 1].lostCount += 1;
                networkPixelLost[networkPixelLost.Count - 1].pixel = pixel;
                highestNumberOfLost = Math.Max(highestNumberOfLost, networkPixelLost[networkPixelLost.Count - 1].lostCount);
            }

            // Get the golden data
            List<PixelLostInfo> goldenPixelLost = new List<PixelLostInfo>();
            for (int currentPosition = 0; currentPosition < goldenAllLostPositions.Count; currentPosition++)
            {
                Point pixel = ConvertPositionToPixel(goldenAllLostPositions[currentPosition][0], goldenAllLostPositions[currentPosition][1],
                    minX, minY, maxX, maxY, b);
                if (currentPixelAltered != pixel)
                {
                    goldenPixelLost.Add(new PixelLostInfo());
                    currentPixelAltered = pixel;
                }
                goldenPixelLost[goldenPixelLost.Count - 1].lostCount += 1;
                goldenPixelLost[goldenPixelLost.Count - 1].pixel = pixel;
                highestNumberOfLost = Math.Max(highestNumberOfLost, goldenPixelLost[goldenPixelLost.Count - 1].lostCount);
            }

            foreach (var pixel in networkPixelLost)
            {
                if (goldenPixelLost.FindIndex(Q => Q.pixel == pixel.pixel) != -1)
                {
                    ColorPixels(pixel, ref b, correctLost);
                }
                else
                {
                    ColorPixels(pixel, ref b, wrongLost);
                }
            }
            if (!onlyShowHitsOnGolden)
            {
                foreach (var pixel in goldenPixelLost)
                {
                    if (networkPixelLost.FindIndex(Q => Q.pixel == pixel.pixel) == -1)
                    {
                        ColorPixels(pixel, ref b, goldenLost);
                    }
                }
            }        
            // We need to flip the heatmap, wrong way now
            b.RotateFlip(RotateFlipType.Rotate180FlipX);

            // Add the underlaying image to the given bitmap  
            if (underlayingImage != null)
            {
                completedImage = MergeTwoImages(underlayingImage, b);
            }
            else
            {
                completedImage = b;
            }
            return completedImage;
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

            int outputImageHeight = firstImage.Height > secondImage.Height ? firstImage.Height : secondImage.Height;

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

        // Functions to get/set things from controlls
        public string GetGoldenDataFolder() { return inFolderGoldenData.Text; }
        public void SetGoldenDataFolder(string text) { inFolderGoldenData.Text = text; }

        public string GetCalculatedDataFolder() { return InFolderNetworkData.Text; }
        public void SetCalculatedDataFolder(string text) { InFolderNetworkData.Text = text; }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            onlyShowHitsOnGolden = checkBox1.Checked;
        }

        private void ColorWrongLost_Click(object sender, EventArgs e)
        {
            wrongLost = ChangeColor(ref ColorWrongLost);
        }

        private Color ChangeColor(ref PictureBox previewer)
        {
            colorDialog1.ShowDialog();
            Color newColor = colorDialog1.Color;
            SetPreviewColor(ref previewer, newColor);
            return newColor;
        }

        private void SetPreviewColor(ref PictureBox previewer, Color newColor)
        {
            if (previewer != null)
            {
                Bitmap preview = new Bitmap(10, 10);
                Graphics g = Graphics.FromImage(preview);

                g.Clear(Color.FromArgb(255, newColor));
                previewer.Image = preview;
            }
        }

        private void ColorCorrectLost_Click(object sender, EventArgs e)
        {
            correctLost = ChangeColor(ref ColorCorrectLost);
        }

        private void ColorGoldenLost_Click(object sender, EventArgs e)
        {
            goldenLost = ChangeColor(ref ColorGoldenLost);
        }
    }
}
