using System;
using System.Collections.Generic;
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
