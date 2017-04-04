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
        public bool ignoreFileWithNoLost = true;

        private Color wrongLost;
        private Color goldenLost;
        private Color correctLost;

        Bitmap completedImage;

        int blueValue;

        int highestWrongPixel, highestCorrectPixel, highestGoldenPixel;
        List<PixelLostInfo> allWrongGuesses = new List<PixelLostInfo>();
        List<PixelLostInfo> allCorrectGuesses = new List<PixelLostInfo>();
        List<PixelLostInfo> allGoldenInfo = new List<PixelLostInfo>();

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
            // reset values
            ResetGlobals();

            Bitmap underlayingImage = null;
            try
            {
                underlayingImage = (Bitmap)Bitmap.FromFile(button1.Text);
                underlayingImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                underlayingImage.RotateFlip(RotateFlipType.Rotate180FlipY);
            }
            catch (Exception)
            { }
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
                if (sizeFromImage.Checked)
                {
                    completedImage = new Bitmap(underlayingImage.Width, underlayingImage.Height);
                }
                else
                {
                    completedImage = new Bitmap(int.Parse(TextureWidth.Text), int.Parse(TextureHeight.Text));
                }

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
                        if (ignoreFileWithNoLost)
                        {
                            continue;
                        }
                        goldenAllLostPositions = new List<int[]>();
                    }
                    minX = Math.Min(minX, tminX);
                    minY = Math.Min(minY, tminY);
                    maxX = Math.Max(maxX, tmaxX);
                    maxY = Math.Max(maxY, tmaxY);
                }

                GetheatmapFromPositions(minX, minY, maxX, maxY, networkAllLostPositions, goldenAllLostPositions);

                completedImage.RotateFlip(RotateFlipType.Rotate180FlipY);
                if (underlayingImage != null)
                {
                    completedImage = MergeTwoImages(underlayingImage, completedImage);
                }

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
            // reset values
            ResetGlobals();

            Bitmap underlayingImage = null;
            try
            {
                underlayingImage = (Bitmap)Bitmap.FromFile(button1.Text);
                underlayingImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                underlayingImage.RotateFlip(RotateFlipType.Rotate180FlipY);
            }
            catch (Exception t)
            {
                MessageBox.Show(t.ToString());
            }
            if (sizeFromImage.Checked)
            {
                completedImage = new Bitmap(underlayingImage.Width, underlayingImage.Height);
            }
            else
            {
                completedImage = new Bitmap(int.Parse(TextureWidth.Text), int.Parse(TextureHeight.Text));
            }

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
            for (int i = 0; i < networkAllPositions.Count; i++)
            {
                List<int[]> networkToSend = networkAllPositions[i] != null ? networkAllPositions[i] : new List<int[]>();
                List<int[]> goldenToSend = new List<int[]>();
                if (goldenAllHeatmapFiles != null)
                {
                    goldenToSend = goldenAllPositions[i] != null ? goldenAllPositions[i] : new List<int[]>();
                }
                if (goldenToSend == null && ignoreFileWithNoLost)
                {
                    continue;
                }
                GetheatmapFromPositions(minX, minY, maxX, maxY, networkToSend, goldenToSend);
            }
            ColorTheImage();
            completedImage.RotateFlip(RotateFlipType.Rotate180FlipY);

            // Add the underlaying image to the given bitmap  
            if (underlayingImage != null)
            {
                completedImage = MergeTwoImages(underlayingImage, completedImage);
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
            if (y < -168993)
            {
                int a = 4;
            }
            int zeroBasedX = x - minX; // make it go from 0
            int zeroBasedY = y - minY; // make it go from 0
            int zeroBasedmaxX = maxX - minX; // make it go from 0
            int zeroBasedmaxY = maxY - minY; // make it go from 0
            float percentOfMaxX = (float)zeroBasedX / (float)zeroBasedmaxX;
            float percentOfMaxY = (float)zeroBasedY / (float)zeroBasedmaxY;
            returnPoint.X = (int)(percentOfMaxX * (image.Width - 1));
            returnPoint.Y = (int)(percentOfMaxY * (image.Height - 1));         
            
            return returnPoint;
        }

        private void ColorPixels(PixelLostInfo pixel, ref Bitmap map, Color underlayingColor, int highestValue)
        {
            // Check which highest to use when coloring
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
                    //float alpha = 255.0f * (1.0f - Math.Min(distance / maxDistance,1.0f));
                    float alpha = ((float)pixel.lostCount / (float)80) * 255.0f;
                    alpha = Math.Min(alpha, 255);
                    alpha = 255;
                    if (float.IsNaN(alpha))
                    {
                        // this happens when propagation is set to 1
                        alpha = 255.0f;
                    }
                    if (x < 0 || x > map.Width - 1 || y < 0 || y > map.Height - 1)
                    {
                        continue;
                    }
                    Color previousColor = map.GetPixel(x, y);
                    int R = (int)Math.Ceiling((float)previousColor.R + (float)underlayingColor.R);
                    int G = (int)Math.Ceiling((float)previousColor.G + (float)underlayingColor.G);
                    int B = (int)Math.Ceiling((float)previousColor.B + (float)underlayingColor.B);

                    Color newColor = Color.FromArgb((int)alpha, Math.Min(R,255), Math.Min(G, 255), Math.Min(B, 255));
                    
                    float previousAlpha = map.GetPixel(x, y).A;
                    map.SetPixel(x, y, newColor);
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                button1.Text = file;

            }
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

        private void GetheatmapFromPositions(int minX, int minY, int maxX, int maxY, List<int[]> networkAllLostPositions, List<int[]> goldenAllLostPositions)
        {
            // sort the list so all the positions of the same type is next to eachother
            networkAllLostPositions = networkAllLostPositions.OrderBy(r => r[0]).ThenBy(r => r[1]).ToList();
            goldenAllLostPositions = goldenAllLostPositions.OrderBy(r => r[0]).ThenBy(r => r[1]).ToList();

            // Find out all the time the network guessed lost
            List<PixelLostInfo> networkPixelLost = new List<PixelLostInfo>();
            Point currentPixelAltered = new Point(-1, -1);
            for (int currentPosition = 0; currentPosition < networkAllLostPositions.Count; currentPosition++)
            {
                Point pixel = ConvertPositionToPixel(networkAllLostPositions[currentPosition][0], networkAllLostPositions[currentPosition][1],
                    minX, minY, maxX, maxY, completedImage);
                if (currentPixelAltered != pixel)
                {
                    networkPixelLost.Add(new PixelLostInfo());
                    currentPixelAltered = pixel;
                }
                networkPixelLost[networkPixelLost.Count - 1].lostCount += 1;
                networkPixelLost[networkPixelLost.Count - 1].pixel = pixel;
            }

            // Get the golden data
            List<PixelLostInfo> goldenPixelLost = new List<PixelLostInfo>();
            currentPixelAltered = new Point(-1, -1);
            for (int currentPosition = 0; currentPosition < goldenAllLostPositions.Count; currentPosition++)
            {
                Point pixel = ConvertPositionToPixel(goldenAllLostPositions[currentPosition][0], goldenAllLostPositions[currentPosition][1],
                    minX, minY, maxX, maxY, completedImage);
                if (currentPixelAltered != pixel)
                {
                    goldenPixelLost.Add(new PixelLostInfo());
                    currentPixelAltered = pixel;
                }
                goldenPixelLost[goldenPixelLost.Count - 1].lostCount += 1;
                goldenPixelLost[goldenPixelLost.Count - 1].pixel = pixel;
            }

            foreach (var pixel in networkPixelLost)
            {
                if (goldenPixelLost.FindIndex(Q => Q.pixel.X == pixel.pixel.X && Q.pixel.Y == pixel.pixel.Y) >= 0)
                {
                    // see if the pixel already exists in the list
                    int index = allCorrectGuesses.FindIndex(Q => Q.pixel.X == pixel.pixel.X && Q.pixel.Y == pixel.pixel.Y);
                    if (index < 0)
                    {
                        allCorrectGuesses.Add(pixel);
                        index = allCorrectGuesses.Count - 1;
                        allCorrectGuesses[index].lostCount = 0;
                    }
                    allCorrectGuesses[index].lostCount += pixel.lostCount;
                    highestCorrectPixel = Math.Max(highestCorrectPixel, allCorrectGuesses[index].lostCount);
                }
                else
                {
                    // see if the pixel already exists in the list
                    int index = allWrongGuesses.FindIndex(Q => Q.pixel.X == pixel.pixel.X && Q.pixel.Y == pixel.pixel.Y);
                    if (index < 0)
                    {
                        allWrongGuesses.Add(pixel);
                        index = allWrongGuesses.Count - 1;
                        allWrongGuesses[index].lostCount = 0;
                    }
                    allWrongGuesses[index].lostCount += pixel.lostCount;
                    highestWrongPixel = Math.Max(highestWrongPixel, allWrongGuesses[index].lostCount);
                }
            }
            if (!onlyShowHitsOnGolden)
            {
                foreach (var pixel in goldenPixelLost)
                {
                    if (true || networkPixelLost.FindIndex(Q => Q.pixel.X == pixel.pixel.X && Q.pixel.Y == pixel.pixel.Y) < 0)
                    {
                        // see if the pixel already exists in the list
                        int index = allGoldenInfo.FindIndex(Q => Q.pixel.X == pixel.pixel.X && Q.pixel.Y == pixel.pixel.Y);
                        if (index < 0)
                        {
                            allGoldenInfo.Add(pixel);
                            index = allGoldenInfo.Count - 1;
                            allGoldenInfo[index].lostCount = 0;
                        }
                        allGoldenInfo[index].lostCount += pixel.lostCount;
                        highestGoldenPixel = Math.Max(highestGoldenPixel, allGoldenInfo[index].lostCount);
                    }
                }
            }

            // We need to flip the heatmap, wrong way now
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

        private void ColorTheImage()
        {
            // Color pixels
            foreach (var item in allCorrectGuesses)
            {
                ColorPixels(item, ref completedImage, correctLost, highestCorrectPixel);
            }
            foreach (var item in allWrongGuesses)
            {
                ColorPixels(item, ref completedImage, wrongLost, highestWrongPixel);
            }
            foreach (var item in allGoldenInfo)
            {
                ColorPixels(item, ref completedImage, goldenLost, highestGoldenPixel);
            }
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

        private void ResetGlobals()
        {
            allGoldenInfo.Clear();
            allCorrectGuesses.Clear();
            allWrongGuesses.Clear();

            highestGoldenPixel = 0;
            highestCorrectPixel = 0;
            highestWrongPixel = 0;
        }
    }
}
