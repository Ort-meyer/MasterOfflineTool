using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UtilityScriptProject
{
    /**
     This script reads raw position data and outputs in two separate folders.
     The intent of this output is to be used to create a heatmap*/
    class RawPositionExtraction
    {
        public RawPositionExtraction()
        {
            // Create directories where we want to save stuff
            string allSaveDir = "RawPositionsExtraction\\allPositions";
            string lostSaveDir = "RawPositionsExtraction\\lostPositions";
            Directory.CreateDirectory(allSaveDir);
            Directory.CreateDirectory(lostSaveDir);
            string targetPath = "..\\..\\..\\..\\Master Offline Tool\\RawData";
            string[] files = Directory.GetFiles(targetPath);
            // Loop over each person
            foreach (string s in files)
            {
                // Make sure we only read positions
                if (!Path.GetFileName(s).Contains("Position"))
                    continue;
                List<string> allPositions = new List<string>();
                List<string> lostPositions = new List<string>();

                // Read through this specific file 3 lines at a time
                string[] lines = File.ReadAllLines(s);
                List<string> linesList = lines.ToList<string>();
                for (int i = 0; i < linesList.Count; i += 3)
                {
                    allPositions.Add(linesList[i + 1]);
                    if (linesList[i + 2] == "1")
                        lostPositions.Add(linesList[i + 1]);
                }

                // Save this person
                string fileNameBase = Path.GetFileNameWithoutExtension(s);
                File.WriteAllLines(allSaveDir + "\\" + fileNameBase + ".heatmap", allPositions.ToArray());
                File.WriteAllLines(lostSaveDir + "\\" + fileNameBase + ".heatmap", lostPositions.ToArray());
            }
        }
    }
}
