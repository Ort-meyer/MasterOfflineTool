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
                int minX, minY, maxX, maxY;
                List<int[]> allLostPositions = ReadRawData(out minX, out minY, out maxX, out maxY, allHeatmapFiles[i]);

                b = new Bitmap(maxX - minX + 1, maxY - minY + 1);
                Graphics g = Graphics.FromImage(b);
                
                g.Clear(Color.FromArgb(1, Color.Red));
                for (int currentPosition = 0; currentPosition < allLostPositions.Count; currentPosition++)
                {
                    Color pixelColor = b.GetPixel(allLostPositions[currentPosition][0] - minX, allLostPositions[currentPosition][1] - minY);
                    b.SetPixel(allLostPositions[currentPosition][0] - minX, allLostPositions[currentPosition][1] - minY
                        , Color.FromArgb(pixelColor.A + 254/allHeatmapFiles.Length, pixelColor));
                }
                      
                
                Preview.Image = b;

                string folderPath = Path.GetFullPath(OutFolder.Text);
                b.Save(folderPath + "ShouldNotBeNamedThis" + ".png", ImageFormat.Png);
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
    }
}
