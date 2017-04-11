using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityScriptProject
{
    /*
     This script is meant to replace the equivalent bash script.
     It looks through the results from varying stills run (that is
     the stuff in SavednetSettingsDifferentStills folder) and finds
     the particular stuff we want to later plot with gnuplot.
     
     It is meant to run in conjunction (i.e. directly before) the
     GnuplotBarsVaryingStuff script*/
    class GnuplotBarPrepareScript
    {

        private List<string> m_allFiles;
        public GnuplotBarPrepareScript()
        {
            m_allFiles = new List<string>();
            string baseStill = "12010";
            string baseCombo = "PosRot";
            string baseLayers = "5  100 70 50 30 10";
            
            string baseSourcePath = "..\\..\\..\\..\\Master Offline Tool\\SavedNetSettingsDifferentStills";
            string baseTargetPath = "..\\..\\..\\..\\Gnuplot result graph data";

            // Get a big nice list of all files
            m_allFiles = GnuplotUtilities.GetALLFilePaths(baseSourcePath, m_allFiles);
            
            // Loop through all files and fit them accordingly
            foreach (string file in m_allFiles)
            {
                // Layer varied
                if (file.Contains(baseStill) && file.Contains(baseCombo) && !file.Contains("Key"))
                {
                    Directory.CreateDirectory(baseTargetPath + "\\Layers varied\\");
                    File.Copy(file, baseTargetPath + "\\Layers varied\\" + Path.GetFileName(file), true);
                }
                // Combo varied
                if (file.Contains(baseStill) && file.Contains(baseLayers))
                {
                    Directory.CreateDirectory(baseTargetPath + "\\Combination varied\\");
                    File.Copy(file, baseTargetPath + "\\Combination varied\\" + Path.GetFileName(file), true);
                }
                // Still varied
                if (file.Contains(baseCombo) && file.Contains(baseLayers) && !file.Contains("Key"))
                {
                    Directory.CreateDirectory(baseTargetPath + "\\Datastill varied\\");
                    File.Copy(file, baseTargetPath + "\\Datastill varied\\" + Path.GetFileName(file), true);
                }
            }
        }

        // Copies files from the source to the garget. Also creates all directories necessary
        public void CopyFile(string p_sourcePath, string p_targetPath)
        {
            // First create directories
            Directory.CreateDirectory(Path.GetDirectoryName(p_sourcePath));
            // Then copy
            File.Copy(p_sourcePath, p_targetPath + Path.GetFileName(p_sourcePath), true);
        }
    }
}
