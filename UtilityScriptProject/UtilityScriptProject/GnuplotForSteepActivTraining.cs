﻿using System;
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
                    Directory.CreateDirectory(m_baseTargetPath + "\\Hidden activ varied\\");
                    File.Copy(file, m_baseTargetPath + "\\Hidden activ varied\\" + Path.GetFileName(file), true);
                }

                // Output activ
                if (
                    info[1] == baseHiddenActiv &&
                    //info[2] == baseOutputActiv &&
                    info[8 + int.Parse(info[3]) - 1] == baseTraining &&
                    info[10 + int.Parse(info[3]) - 1] == baseHiddenSteepness &&
                    info[11 + int.Parse(info[3]) - 1] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(m_baseTargetPath + "\\Output activ varied\\");
                    File.Copy(file, m_baseTargetPath + "\\Output activ varied\\" + Path.GetFileName(file), true);
                }

                // Training
                if (
                    info[1] == baseHiddenActiv &&
                    info[2] == baseOutputActiv &&
                    //info[8] == baseTraining &&
                    info[10 + int.Parse(info[3]) - 1] == baseHiddenSteepness &&
                    info[11 + int.Parse(info[3]) - 1] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(m_baseTargetPath + "\\Training varied\\");
                    File.Copy(file, m_baseTargetPath + "\\Training varied\\" + Path.GetFileName(file), true);
                }

                // Hidden steep
                if (
                    info[1] == baseHiddenActiv &&
                    info[2] == baseOutputActiv &&
                    info[8 + int.Parse(info[3]) - 1] == baseTraining &&
                    //info[10] == baseHiddenSteepness &&
                    info[11 + int.Parse(info[3]) - 1] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(m_baseTargetPath + "\\Hidden steep varied\\");
                    File.Copy(file, m_baseTargetPath + "\\Hidden steep varied\\" + Path.GetFileName(file), true);
                }

                // Output steep
                if (
                    info[1] == baseHiddenActiv &&
                    info[2] == baseOutputActiv &&
                    info[8 + int.Parse(info[3]) - 1] == baseTraining &&
                    info[10 + int.Parse(info[3]) - 1] == baseHiddenSteepness)
                //info[11] == baseOutputSteepness)
                {
                    Directory.CreateDirectory(m_baseTargetPath + "\\Output steep varied\\");
                    File.Copy(file, m_baseTargetPath + "\\Output steep varied\\" + Path.GetFileName(file), true);
                }

            }

            // Files are moved, now calculate stuff and put into gnuplot-ready file

            BarPlot("Hidden activ", 1);
            BarPlot("Output activ", 2);
            BarPlot("Training", 8);



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
                    string thisText;
                    if (p_infoIndex != 8)
                        thisText = GetActivationFunctionName(lineInfo[p_infoIndex]);
                    else
                    {
                        thisText = GetTrainingName(lineInfo[8 + int.Parse(lineInfo[3]) - 1]);
                    }
                    FileEntry thisEntry = new FileEntry(thisText, meanCorrect, meanWrong, correctStandardDeviation, wrongStandardDeviation);
                    entriesList.Add(thisEntry);
                }
            }
            ///////////////////BIGASS BUG HERE FOR SOME REASON//////////////
            GnuplotUtilities.SaveValuesToFile(path, entriesList);
            string gnuplotPath = path + "\\gnuplot bar script error bars - generic.plt";
            // -e "filename='default.data'" - e "foo='bar"'
            string args = "-e \"fileName='" + p_type + ".txt'\" -e \"outputFileName='" + p_type + ".svg'\"";
            Process.Start(gnuplotPath, args);


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
            trainingMap["1"] = "Incremental";
            trainingMap["2"] = "RProp";
            trainingMap["3"] = "QuickProp";
            trainingMap["4"] = "SarProp";

            return trainingMap[p_index];
        }
    }
}
