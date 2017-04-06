using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityScriptProject
{
    /*
     Does very much the same as the other gnuplot scripts, except for steepness, 
     activation functions and training algorithm*/
    class GnuplotForSteepActivTraining
    {
        public GnuplotForSteepActivTraining()
        {
            string baseHiddenSteepness = "0.6";
            string baseOutputSteepness = "0.6";
            string baseHiddenActiv = "11";
            string baseOutputActiv = "10";
            string baseTraining = "2";
            string sourcePath = @"..\..\..\..\Master Offline Tool\SavedNetSettingsSteepActivTrain\";
            string baseTargetPath = @"..\..\..\..\Gnuplot result graph data";

            // Get a big list of all files
            List<string> allFiles = new List<string>();
            allFiles = GnuplotUtilities.GetALLFilePaths(sourcePath, allFiles);

            foreach (string file in allFiles)
            {
                string[] info = Path.GetFileName(file).Split();
                // Only the best network
                if (
                    info[1] == baseHiddenActiv &&
                    info[2] == baseOutputActiv &&
                    info[8] == baseTraining &&
                    info[10] == baseHiddenSteepness &&
                    info[11] == baseOutputSteepness)
                {
                    // Do nothing, really
                }

                // Hidden activ
                if (
                     //info[1] == baseHiddenActiv &&
                     info[2] == baseOutputActiv &&
                     info[8+int.Parse(info[3])-1] == baseTraining &&
                     info[10 + int.Parse(info[3]) - 1] == baseHiddenSteepness &&
                     info[11 + int.Parse(info[3]) - 1] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(baseTargetPath + "\\Hidden activ varied\\");
                    File.Copy(file, baseTargetPath + "\\Hidden activ varied\\" + Path.GetFileName(file), true);
                }

                // Output activ
                if (
                    info[1] == baseHiddenActiv &&
                    //info[2] == baseOutputActiv &&
                    info[8 + int.Parse(info[3]) - 1] == baseTraining &&
                    info[10 + int.Parse(info[3]) - 1] == baseHiddenSteepness &&
                    info[11 + int.Parse(info[3]) - 1] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(baseTargetPath + "\\Output activ varied\\");
                    File.Copy(file, baseTargetPath + "\\Output activ varied\\" + Path.GetFileName(file), true);
                }

                // Training
                if (
                    info[1] == baseHiddenActiv &&
                    info[2] == baseOutputActiv &&
                    //info[8] == baseTraining &&
                    info[10 + int.Parse(info[3]) - 1] == baseHiddenSteepness &&
                    info[11 + int.Parse(info[3]) - 1] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(baseTargetPath + "\\Training varied\\");
                    File.Copy(file, baseTargetPath + "\\Training varied\\" + Path.GetFileName(file), true);
                }

                // Hidden steep
                if (
                    info[1] == baseHiddenActiv &&
                    info[2] == baseOutputActiv &&
                    info[8 + int.Parse(info[3]) - 1] == baseTraining &&
                    //info[10] == baseHiddenSteepness &&
                    info[11 + int.Parse(info[3]) - 1] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(baseTargetPath + "\\Hidden steep varied\\");
                    File.Copy(file, baseTargetPath + "\\Hidden steep varied\\" + Path.GetFileName(file), true);
                }

                // Output steep
                if (
                    info[1] == baseHiddenActiv &&
                    info[2] == baseOutputActiv &&
                    info[8 + int.Parse(info[3]) - 1] == baseTraining &&
                    info[10 + int.Parse(info[3]) - 1] == baseHiddenSteepness)
                    //info[11] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(baseTargetPath + "\\Output steep varied\\");
                    File.Copy(file, baseTargetPath + "\\Output steep varied\\" + Path.GetFileName(file), true);
                }

            }


        }
    }
}
