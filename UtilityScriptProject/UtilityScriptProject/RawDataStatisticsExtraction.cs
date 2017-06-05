using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityScriptProject
{
    class RawDataStatisticsExtraction
    {
        string rawDataFOlderLocation = "../../../../Master Offline Tool/RawData";
        string outFolderAndFile = "../../../../Gnuplot result graph data/RawDataStatistics.txt";
        string gnuPlotFile = "../../../../Gnuplot result graph data/gnuplot bar script - raw data statistics.plt";
        public RawDataStatisticsExtraction()
        {
            List<string> files = GnuplotUtilities.GetAllFilesInFolderStructure(rawDataFOlderLocation, "rawData");

            List<string> lines = new List<string>();
            for (int i = 0; i < 24; i++)
            {
                lines.Add((i+1) + " " + GetStatisticsFromFile(files[i]));
            }
            File.Delete(outFolderAndFile);
            File.WriteAllLines(outFolderAndFile, lines);
            // Plot them with gnuplot
            GnuplotUtilities.PlotWithGnuplot(outFolderAndFile, gnuPlotFile);
        }

        private string GetStatisticsFromFile(string p_file)
        {
            string[] lines = File.ReadAllLines(p_file);
            int frames = lines.Length / 3;
            int framesLost = 0;
            int framesFound = 0;
            for (int i = 2; i < lines.Length; i+=3)
            {
                if (lines[i] == "0")
                {
                    framesFound++;
                }
                else
                {
                    framesLost++;
                }
            }
            return frames + " " + framesLost;
        }
    }
}
