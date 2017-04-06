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
                    GnuplotUtilities.GetValues(filePath, ref meanCorrect, ref meanWrong, ref correctStandardDeviation, ref wrongStandardDeviation);

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
            GnuplotUtilities.SaveValuesToFile(newPath + "\\combos.txt", comboVariedArray);
            // Plot them with gnuplot
            PlotWithGnuplot(newPath, gnuplotBaseName + "combos.plt");

            // Datastill variations second 
            List<FileEntry> stillVariedArray = new List<FileEntry>();
            string stillPath = Path.GetFullPath(Path.Combine(newPath, "Datastill varied"));
            string[] stillFiles = Directory.GetFiles(stillPath);
            foreach (string stillFile in stillFiles)
            {
                // Has to be an array, even though we know there's only one file in there
                float meanCorrect = 0, meanWrong = 0, correctStandardDeviation = 0, wrongStandardDeviation = 0;
                GnuplotUtilities.GetValues(stillFile, ref meanCorrect, ref meanWrong, ref correctStandardDeviation, ref wrongStandardDeviation);

                //string thisStill = stillDir.Substring(stillSeparationIndex + 1);
                // Hax. We look as long as its not a number
                var stillCharactersInSomeWeirdFormat = Path.GetFileName(stillFile).TakeWhile(c => Char.IsDigit(c));
                string thisStill = new string(stillCharactersInSomeWeirdFormat.ToArray());
                // Store in array
                FileEntry thisEntry = new FileEntry(thisStill, meanCorrect, meanWrong, correctStandardDeviation, wrongStandardDeviation);
                stillVariedArray.Add(thisEntry);
            }

            // Save to file
            GnuplotUtilities.SaveValuesToFile(newPath + "\\stills.txt", stillVariedArray);
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
                    GnuplotUtilities.GetValues(filePath, ref meanCorrect, ref meanWrong, ref correctStandardDeviation, ref wrongStandardDeviation);

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
            GnuplotUtilities.SaveValuesToFile(newPath + "\\layers.txt", layerVariedArray);
            // Plot them with gnuplot
            PlotWithGnuplot(newPath, gnuplotBaseName + "layers.plt");

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


}
