using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityScriptProject
{
    /*
     Class containing some utility stuff for gnuplot scripts*/
    class GnuplotUtilities
    {
        public static List<string> GetAllFilesInFolderStructure(string p_rootDirectory, string p_fileEnding)
        {
            List<string> files = new List<string>();
            // FInd all subdirectories
            string[] subDirectories = Directory.GetDirectories(p_rootDirectory);
            foreach (var directoryPath in subDirectories)
            {
                // Go to the next subfolder and do the same thing again
                files.AddRange(GetAllFilesInFolderStructure(directoryPath, p_fileEnding));
            }
            files.AddRange(Directory.GetFiles(p_rootDirectory, "*" + p_fileEnding));
            return files;
        }

        public static List<string> GetALLFilePaths(string sDir, List<string> p_fileNames)
        {
            foreach (string dir in Directory.GetDirectories(sDir))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    p_fileNames.Add(file);
                }
                GetALLFilePaths(dir, p_fileNames);
            }
            return p_fileNames;
        }
        // Calculates standard deviation according to https://www.mathsisfun.com/data/standard-deviation-formulas.html
        public static float CalculateStandardDeviation(List<float> p_numbers, float p_mean)
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
        public static void SaveValuesToFile(string p_filePath, List<FileEntry> p_fileEntries)
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

        // Gets the values set in ref parameters from p_filename (which needsless to say is a file with tons of networks)
        public static void GetValues(string p_filePath, ref float o_meanCorrect, ref float o_correctStandardDeviation, ref float o_meanWrong, ref float o_wrongStandardDeviation)
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
            o_correctStandardDeviation = GnuplotUtilities.CalculateStandardDeviation(correctPercentiles, meanCorrect);
            o_wrongStandardDeviation = GnuplotUtilities.CalculateStandardDeviation(wrongPercentiles, meanWrong);

            o_meanCorrect = meanCorrect;
            o_meanWrong = meanWrong;

        }

        // Plots the provided text file with a hardcoded gnuplot script. Used to get true and false positives with error bars
        public static void PlotWithGnuplot(string p_plotScriptDirectory, string p_plotScriptFileName)
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
