using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SavedNetSettingsFinder
{
    public partial class Form1 : Form
    {
        System.Globalization.NumberFormatInfo nf
          = new System.Globalization.NumberFormatInfo()
          {
              NumberGroupSeparator = "."
          };

        List<string> allValidLines = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            allValidLines.AddRange(File.ReadAllLines(fileToUse.Text));
            
            NonValidatedMSEFilter();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                fileToUse.Text = file;
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

        // Filter functions
        private void NonValidatedMSEFilter()
        {
            // Check if the range have been given.
            if (minNonValidatedMSE.Text.Length == 0 && maxNonValidatedMSE.Text.Length == 0)
            {
                return;
            }
            float min = -1;
            float max = float.MaxValue;
            if (minNonValidatedMSE.Text.Length != 0)
            {
                min = float.Parse(minNonValidatedMSE.Text, nf);    
            }
            if (maxNonValidatedMSE.Text.Length != 0)
            {
                max = float.Parse(maxNonValidatedMSE.Text, nf);
            }

            for (int i = 0; i < allValidLines.Count; i++)
            {
                if (allValidLines[i].Contains("MSE values") )
                {
                    // This is a line we are interested in!
                    float bestMSE = float.MaxValue;
                    string[] allMSE = allValidLines[i].Substring(allValidLines[i].LastIndexOf(":") + 1).Split(' ');
                    foreach (var mse in allMSE)
                    {
                        float mseNumber;
                        if (float.TryParse(mse,NumberStyles.Any,nf, out mseNumber))
                        {
                            if (mseNumber < bestMSE)
                            {
                                bestMSE = mseNumber;
                            }
                        }
                    }
                    if (bestMSE < min || bestMSE > max)
                    {
                        // Throw Away
                        RemoveNetworkFromList(i);
                    }
                }
            }
        }

        private void ValidatedMSEFilter()
        {

        }

        private void MSEDifferenceFilter()
        {

        }

        // Helper func
        private void RemoveNetworkFromList(int indexInsideNetwork)
        {
            int firstLineInNet = 0;
            int lastLineInNet = allValidLines.Count -1;
            for (int i = indexInsideNetwork; i < allValidLines.Count; i++)
            {
                if (allValidLines[i] == "")
                {
                    lastLineInNet = i;
                    break;
                }
            }
            for (int i = indexInsideNetwork; i >= 0; i--)
            {
                if (allValidLines[i] == "")
                {
                    firstLineInNet = i;
                    break;
                }
            }

            allValidLines.RemoveRange(firstLineInNet, lastLineInNet+1);
        }
    }
}
