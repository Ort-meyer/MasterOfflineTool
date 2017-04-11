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
     Does very much the same as the other gnuplot scripts, except for steepness, 
     activation functions and training algorithm*/
    class GnuplotForSteepActivTraining
    {
        string m_baseTargetPath;
        public GnuplotForSteepActivTraining()
        {
            string baseHiddenSteepness = "0.6";
            string baseOutputSteepness = "0.6";
            string baseHiddenActiv = "11";
            string baseOutputActiv = "10";
            string baseTraining = "2";
            string sourcePath = @"..\..\..\..\Master Offline Tool\SavedNetSettingsSteepActivTrain\";
            m_baseTargetPath = @"..\..\..\..\Gnuplot result graph data";

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
                     info[8 + int.Parse(info[3]) - 1] == baseTraining &&
                     info[10 + int.Parse(info[3]) - 1] == baseHiddenSteepness &&
                     info[11 + int.Parse(info[3]) - 1] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(m_baseTargetPath + "\\Hidden activation functions varied\\");
                    File.Copy(file, m_baseTargetPath + "\\Hidden activation functions varied\\" + Path.GetFileName(file), true);
                }

                // Output activ
                if (
                    info[1] == baseHiddenActiv &&
                    //info[2] == baseOutputActiv &&
                    info[8 + int.Parse(info[3]) - 1] == baseTraining &&
                    info[10 + int.Parse(info[3]) - 1] == baseHiddenSteepness &&
                    info[11 + int.Parse(info[3]) - 1] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(m_baseTargetPath + "\\Output activation functions varied\\");
                    File.Copy(file, m_baseTargetPath + "\\Output activation functions varied\\" + Path.GetFileName(file), true);
                }

                // Training
                if (
                    info[1] == baseHiddenActiv &&
                    info[2] == baseOutputActiv &&
                    //info[8] == baseTraining &&
                    info[10 + int.Parse(info[3]) - 1] == baseHiddenSteepness &&
                    info[11 + int.Parse(info[3]) - 1] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(m_baseTargetPath + "\\Training algorithms varied\\");
                    File.Copy(file, m_baseTargetPath + "\\Training algorithms varied\\" + Path.GetFileName(file), true);
                }

                // Hidden steep
                if (
                    info[1] == baseHiddenActiv &&
                    info[2] == baseOutputActiv &&
                    info[8 + int.Parse(info[3]) - 1] == baseTraining &&
                    //info[10] == baseHiddenSteepness &&
                    info[11 + int.Parse(info[3]) - 1] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(m_baseTargetPath + "\\Hidden steepness varied\\");
                    File.Copy(file, m_baseTargetPath + "\\Hidden steepness varied\\" + Path.GetFileName(file), true);
                }

                // Output steep
                if (
                    info[1] == baseHiddenActiv &&
                    info[2] == baseOutputActiv &&
                    info[8 + int.Parse(info[3]) - 1] == baseTraining &&
                    info[10 + int.Parse(info[3]) - 1] == baseHiddenSteepness)
                //info[11] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(m_baseTargetPath + "\\Output steepness varied\\");
                    File.Copy(file, m_baseTargetPath + "\\Output steepness varied\\" + Path.GetFileName(file), true);
                }

            }

            // Files are moved, now calculate stuff and put into gnuplot-ready file

            BarPlot("Hidden activation functions", 1);
            BarPlot("Output activation functions", 2);
            BarPlot("Training algorithms", 8);
            BarPlot("Hidden steepness", 10);
            BarPlot("Output steepness", 11);



        }

        private void PrepareLinePlotData()
        {

        }

        private void BarPlot(string p_type, int p_infoIndex)
        {
            List<FileEntry> entriesList = new List<FileEntry>();
            string path = Path.GetFullPath(m_baseTargetPath);
            string newPath = path + "\\" + p_type + " varied";
            
            string[] files = Directory.GetFiles(newPath);
            foreach (string filePath in files)
            {
                // We don't want to try and read our dummy file
                if (Path.GetExtension(filePath) != ".txt")
                {
                    string[] lineInfo = Path.GetFileName(filePath).Split();
                    string thisLabel = lineInfo[p_infoIndex];
                    // Get values
                    float meanCorrect = 0, meanWrong = 0, correctStandardDeviation = 0, wrongStandardDeviation = 0;

                    GnuplotUtilities.GetValues(filePath, ref meanCorrect, ref meanWrong, ref correctStandardDeviation, ref wrongStandardDeviation);
                    string thisText = "";
                    if (p_infoIndex < 3)
                    {
                        thisText = GetActivationFunctionName(lineInfo[p_infoIndex]);
                    }
                    else if (p_infoIndex == 8)
                    {
                        thisText = GetTrainingName(lineInfo[8 + int.Parse(lineInfo[3]) - 1]);
                    }
                    else
                    {
                        thisText = lineInfo[p_infoIndex + int.Parse(lineInfo[3]) - 1];
                        thisText = thisText.Replace(".", ",");
                    }
                    FileEntry thisEntry = new FileEntry(thisText, meanCorrect, meanWrong, correctStandardDeviation, wrongStandardDeviation);
                    entriesList.Add(thisEntry);
                }
            }
            ///////////////////BIGASS BUG HERE FOR SOME REASON//////////////
            GnuplotUtilities.SaveValuesToFile(path+"\\"+p_type+".txt", entriesList);
            string gnuplotPath = path + "\\gnuplot bar script error bars - generic.plt";
            // -e "filename='default.data'" - e "foo='bar"'
            //string args = "-e \"fileName='" + p_type + ".txt'\" -e \"outputFileName='" + p_type + ".svg'\"";
            //string args = "-e \"fileName=\"" + p_type + ".txt\"";// ; outputFileName='" + p_type + ".svg'\"";


            // Load gnuplot script
            string[] gnuplotScriptLines = File.ReadAllLines(gnuplotPath);
            for (int i = 0; i < gnuplotScriptLines.Length; i++)
            {

                if (gnuplotScriptLines[i].Contains("ARGOutputFileName"))
                {
                    gnuplotScriptLines[i] = gnuplotScriptLines[i].Replace("ARGOutputFileName", p_type + ".pdf");
                }
                if (gnuplotScriptLines[i].Contains("ARGFileName"))
                {
                    gnuplotScriptLines[i] = gnuplotScriptLines[i].Replace("ARGFileName", p_type + ".txt");
                }
                if (gnuplotScriptLines[i].Contains("ARGTitle"))
                {
                    gnuplotScriptLines[i] = gnuplotScriptLines[i].Replace("ARGTitle", p_type);
                }

            }

            string tempFilePath = gnuplotPath;
            tempFilePath = tempFilePath.Replace("generic", "generic - temp");
            File.WriteAllLines(tempFilePath, gnuplotScriptLines);

            //Process.Start(tempFilePath);
            ProcessStartInfo PSI = new ProcessStartInfo();
            PSI.FileName = Path.GetFileName(tempFilePath);
            string dir = Path.GetDirectoryName(tempFilePath);
            PSI.WorkingDirectory = dir;
            using (Process exeProcess = Process.Start(PSI))
            {
                exeProcess.WaitForExit();
            }
            File.Delete(tempFilePath);


        }

        // Simple method that gets the activation function of a certain index
        private string GetActivationFunctionName(string p_index)
        {
            Dictionary<string, string> activationFunctionmap = new Dictionary<string, string>();
            activationFunctionmap["11"] = "Elliot symmetric";
            activationFunctionmap["5"] = "Sigmoid symmetric";
            activationFunctionmap["15"] = "Cos symmetric";
            activationFunctionmap["14"] = "Sin symmetric";
            activationFunctionmap["8"] = "Gaussian symmetric";
            activationFunctionmap["13"] = "Linear piece symmetric";
            activationFunctionmap["10"] = "Elliot";
            activationFunctionmap["3"] = "Sigmoid";
            activationFunctionmap["7"] = "Gaussian";
            activationFunctionmap["12"] = "Linear piece";

            return activationFunctionmap[p_index];
        }

        private string GetTrainingName(string p_index)
        {
            Dictionary<string, string> trainingMap = new Dictionary<string, string>();
            trainingMap["0"] = "Incremental";
            trainingMap["1"] = "Batch";
            trainingMap["2"] = "RProp";
            trainingMap["3"] = "QuickProp";
            trainingMap["4"] = "SarProp";

            return trainingMap[p_index];
        }
    }
}
