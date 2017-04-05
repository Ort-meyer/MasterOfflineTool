using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace UtilityScriptProject
{
    /* Takes all of our stuff from gnuplot result graph data and puts into text files
     * Then proceeds to launch gnuplot scripts to generate appropriate bar charts
     */
    class GnuplotBarsVaryingStuff
    {
        public GnuplotBarsVaryingStuff()
        {

            // Find our way to where the different folders to be plotted are
            string path = Directory.GetCurrentDirectory();
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\Gnuplot result graph data"));
            string gnuplotBaseName = "gnuplot bar script error bars - "; // We add specific data type where applicable

            // Combo variations first
            List<FileEntry> comboVariedArray = new List<FileEntry>();
            string comboPath = Path.GetFullPath(Path.Combine(newPath, "Combination varied"));
            string[] files = Directory.GetFiles(comboPath);
            foreach (string filePath in files)
            {
                // We don't want to try and read our dummy file
                if (Path.GetExtension(filePath) != ".txt")
                {

                    // Get values
                    float meanCorrect = 0, meanWrong = 0, correctStandardDeviation = 0, wrongStandardDeviation = 0;
                    GetValues(filePath, ref meanCorrect, ref meanWrong, ref correctStandardDeviation, ref wrongStandardDeviation);

                    // Find which combo we're working on from file name
                    string fileName = Path.GetFileName(filePath);
                    int comboSeparationIndex = fileName.IndexOf(" ");
                    // WARNING!! Hard coded 4 since we run 6020 as baseline still!
                    string thisCombo = fileName.Substring(4, comboSeparationIndex);
                    // Store in array
                    FileEntry thisEntry = new FileEntry(thisCombo, meanCorrect, meanWrong, correctStandardDeviation, wrongStandardDeviation);
                    comboVariedArray.Add(thisEntry);
                }
            }
            // Save to file
            SaveValuesToFile(newPath + "\\combos.txt", comboVariedArray);
            // Plot them with gnuplot
            PlotWithGnuplot(newPath, gnuplotBaseName + "combos.plt");

            // Datastill variations second 
            List<FileEntry> stillVariedArray = new List<FileEntry>();
            string stillPath = Path.GetFullPath(Path.Combine(newPath, "Datastill varied"));
            string[] stillDirs = Directory.GetDirectories(stillPath);
            Console.WriteLine(stillDirs[1]);
            foreach (string stillDir in stillDirs)
            {
                // Has to be an array, even though we know there's only one file in there
                string[] stillFiles = Directory.GetFiles(stillDir);
                float meanCorrect = 0, meanWrong = 0, correctStandardDeviation = 0, wrongStandardDeviation = 0;
                GetValues(stillFiles[0], ref meanCorrect, ref meanWrong, ref correctStandardDeviation, ref wrongStandardDeviation);

                int stillSeparationIndex = stillDir.LastIndexOf("\\");
                string thisStill = stillDir.Substring(stillSeparationIndex + 1);
                // Store in array
                FileEntry thisEntry = new FileEntry(thisStill, meanCorrect, meanWrong, correctStandardDeviation, wrongStandardDeviation);
                stillVariedArray.Add(thisEntry);
            }

            // Save to file
            SaveValuesToFile(newPath + "\\stills.txt", stillVariedArray);
            // Plot them with gnuplot
            PlotWithGnuplot(newPath, gnuplotBaseName + "stills.plt");

            // Layers varied last
            List<FileEntry> layerVariedArray = new List<FileEntry>();
            string layersPath = Path.GetFullPath(Path.Combine(newPath, "Layers varied"));
            string[] layerFiles = Directory.GetFiles(layersPath);
            foreach (string filePath in layerFiles)
            {
                // We don't want to try and read our dummy file
                if (Path.GetExtension(filePath) != ".txt")
                {
                    // Get values
                    float meanCorrect = 0, meanWrong = 0, correctStandardDeviation = 0, wrongStandardDeviation = 0;
                    GetValues(filePath, ref meanCorrect, ref meanWrong, ref correctStandardDeviation, ref wrongStandardDeviation);

                    // Figure out network configuration string
                    string thisLayers = "";
                    string fileName = Path.GetFileName(filePath);
                    string[] infoInLine = fileName.Split(' ');
                    int numLayers = int.Parse(infoInLine[3]);
                    for (int i = 0; i < numLayers; i++)
                    {
                        thisLayers += infoInLine[5 + i];
                        thisLayers += " ";
                    }
                    // Store in array
                    FileEntry thisEntry = new FileEntry(thisLayers, meanCorrect, meanWrong, correctStandardDeviation, wrongStandardDeviation);
                    layerVariedArray.Add(thisEntry);
                }
            }

            // Save to file
            SaveValuesToFile(newPath + "\\layers.txt", layerVariedArray);
            // Plot them with gnuplot
            PlotWithGnuplot(newPath, gnuplotBaseName + "layers.plt");

        }

        // Gets the values set in ref parameters from p_filename (which needsless to say is a file with tons of networks)
        private void GetValues(string p_filePath, ref float o_meanCorrect, ref float o_correctStandardDeviation, ref float o_meanWrong, ref float o_wrongStandardDeviation)
        {
            // Something we need to parse floats apparently
            System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo();


            // Start wtih finding all values and their totals
            int numEntries = 0;
            List<float> correctPercentiles = new List<float>();
            float meanCorrect = 0;
            List<float> wrongPercentiles = new List<float>();
            float meanWrong = 0;
            string[] lines = File.ReadAllLines(p_filePath);
            foreach (string line in lines)
            {
                // Get info from this line (this was clever, m8)
                string[] infoInLine = line.Split(' ');
                // Check that we're found the right line
                if (line.Contains("Percentile correct: "))
                {
                    correctPercentiles.Add(float.Parse(infoInLine[6], nf)); // Don't forget to divide at end!
                    meanCorrect += correctPercentiles[correctPercentiles.Count - 1];
                    wrongPercentiles.Add(float.Parse(infoInLine[14], nf));
                    meanWrong += wrongPercentiles[wrongPercentiles.Count - 1]; // Don't forget to divide at end!
                    numEntries++;
                }
            }

            // Calculate mean values
            meanCorrect /= numEntries;
            meanWrong /= numEntries;

            // Now we calculate standard deviations for both correct and wrong
            o_correctStandardDeviation = CalculateStandardDeviation(correctPercentiles, meanCorrect);
            o_wrongStandardDeviation = CalculateStandardDeviation(wrongPercentiles, meanWrong);

            o_meanCorrect = meanCorrect;
            o_meanWrong = meanWrong;

        }


        // Calculates standard deviation according to https://www.mathsisfun.com/data/standard-deviation-formulas.html
        private float CalculateStandardDeviation(List<float> p_numbers, float p_mean)
        {
            double correctSum = 0;
            foreach (float f in p_numbers)
            {
                // These casts tho...
                correctSum += Math.Pow(Convert.ToDouble(f - p_mean), 2);
            }
            double sumMean = correctSum / p_numbers.Count;
            float standardDeviation = (float)Math.Sqrt(sumMean);
            return standardDeviation;
        }

        // Writes the file entries to the file path specified
        private void SaveValuesToFile(string p_filePath, List<FileEntry> p_fileEntries)
        {
            // Convert to lines
            List<string> lines = new List<string>();
            int i = 0;
            foreach (FileEntry entry in p_fileEntries)
            {
                string thisLine = entry.text;
                thisLine += ". "; // Unsure this works
                thisLine += Convert.ToString(entry.meanCorrect);
                thisLine += ". ";
                thisLine += Convert.ToString(entry.correctStandardDeviation);
                thisLine += ". ";
                thisLine += Convert.ToString(entry.meanWrong);
                thisLine += ". ";
                thisLine += Convert.ToString(entry.wrongStandardDeviation);
                lines.Add(thisLine);
                i++;
            }

            File.WriteAllLines(p_filePath, lines.ToArray());
        }

        // Plots the provided text file with a hardcoded gnuplot script. Used to get true and false positives with error bars
        private void PlotWithGnuplot(string p_plotScriptDirectory, string p_plotScriptFileName)
        {
            string derp = Directory.GetCurrentDirectory();
            ProcessStartInfo PSI = new ProcessStartInfo();
            PSI.FileName = p_plotScriptFileName;
            PSI.WorkingDirectory = p_plotScriptDirectory;
            using (Process exeProcess = Process.Start(PSI))
            {
                exeProcess.WaitForExit();
            }
        }
    }

    // Used to easily group file entries for writing
    public class FileEntry
    {
        public FileEntry() { }
        // Short variable names. I'm in a rush
        public FileEntry(string p_text, float p_mc, float p_mw, float p_csd, float p_wsd)
        {
            text = p_text;
            meanCorrect = p_mc;
            meanWrong = p_mw;
            correctStandardDeviation = p_csd;
            wrongStandardDeviation = p_wsd;
        }
        public string text;
        public float meanCorrect = 0, meanWrong = 0, correctStandardDeviation = 0, wrongStandardDeviation = 0;
    }
}
