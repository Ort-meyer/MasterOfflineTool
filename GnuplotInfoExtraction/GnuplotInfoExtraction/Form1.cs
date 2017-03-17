using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GnuplotInfoExtraction
{

    public partial class Form1 : Form
    {
        System.Globalization.NumberFormatInfo nf
  = new System.Globalization.NumberFormatInfo()
  {
      NumberGroupSeparator = "."
  };

        struct NetworkSettingInfo
        {
            public string ID;
            public float MSE;
            public float PercentileCorrect;
            public float MeanError;
            public float GuessedLostWhenFound;
            public bool DidRetraining;
            public List<int> HiddenCells;
            public int FunctionHidden;
            public int FunctionOutput;
            public float LearningRate;
            public float SteepnessHidden;
            public float SteepnessOutput;
            public bool deterministicWeights;
            public float BiggestMSEDifference;
            public List<float> MSEValues;
        }

        List<NetworkSettingInfo> allSettings;
        List<string> infoToSave;
        string extractedDataFileName = "";
        string interestingValueName = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void FileBrowser_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                FileBrowser.Text = file;
            }
        }

        private void ExtractData_Click(object sender, EventArgs e)
        {
            allSettings = new List<NetworkSettingInfo>();
            infoToSave = new List<string>();
            LoadFileIntoNetworkSettings(FileBrowser.Text);
            
            RadioButton checkedButton = ExtractionMode.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked);
            if (checkedButton == radio_HiddenCells)
            {
                // Hidden cells file extraction
                HiddenCellsExtraction();
            }
            else if (checkedButton == radio_HiddenLayers)
            {
                HiddenLayersExtraction();
            }
            else if (checkedButton == radio_LearningRate)
            {
                LearningRateExtraction();
            }
            else if (checkedButton == radio_SteepnessHidden)
            {
                SteepnessHiddenExtraction();
            }
            else if (checkedButton == radio_SteepnessOutput)
            {
                SteepnessOutputExtraction();
            }
            else if (checkedButton == radio_EpochsTrained)
            {
                EpochsTrainedExtraction();
            }
            else if (checkedButton == radio_MultipleNetworks)
            {
                MultipleNetworksExtraction();
            }
            DecideExtractionName();
            File.WriteAllLines(extractedDataFileName, infoToSave);
        }

        private void GnuPlot_Click(object sender, EventArgs e)
        {
            DecideExtractionName();
            DecideInterestingValueName();
            string dataFile = extractedDataFileName;                  // one data file
            string tempScriptFile = "tempPlotScript.plt";    // gnuplot script
            string pngFile = extractedDataFileName + ".png";             // output png file
            string gnuplotScriptShellFile = "PlotScriptLineChart.plt";
            if (radio_MultipleNetworks.Checked)
            {
                gnuplotScriptShellFile = "PlotScriptBarChart.plt";
            }

            // you can download it from file
            string[] gnuplot_script = File.ReadAllLines(gnuplotScriptShellFile);

            // change filenames in script
            for (int i = 0; i < gnuplot_script.Length; i++)
            {
                if (gnuplot_script[i].Contains("dummyFileName"))
                {
                    gnuplot_script[i] = gnuplot_script[i].Replace("dummyFileName", dataFile);
                }
                if (gnuplot_script[i].Contains("dummyImageOutputName"))
                {
                    gnuplot_script[i] = gnuplot_script[i].Replace("dummyImageOutputName", pngFile);
                }
                if (gnuplot_script[i].Contains("dummyXLable"))
                {
                    gnuplot_script[i] = gnuplot_script[i].Replace("dummyXLable", PlotXAxisName.Text);
                }
                if (gnuplot_script[i].Contains("dummyYLable"))
                {
                    gnuplot_script[i] = gnuplot_script[i].Replace("dummyYLable", PlotYAxisName.Text);
                }
                if (gnuplot_script[i].Contains("dummyTitle"))
                {
                    gnuplot_script[i] = gnuplot_script[i].Replace("dummyTitle", PlotTitle.Text);
                }
                if (gnuplot_script[i].Contains("dummyLineName"))
                {
                    gnuplot_script[i] = gnuplot_script[i].Replace("dummyLineName", interestingValueName);
                }
            }

            // write sccript to file
            File.WriteAllLines(tempScriptFile, gnuplot_script);

            // launch script
            ProcessStartInfo PSI = new ProcessStartInfo();
            PSI.FileName = tempScriptFile;
            string dir = Directory.GetCurrentDirectory();
            PSI.WorkingDirectory = dir;
            using (Process exeProcess = Process.Start(PSI))
            {
                exeProcess.WaitForExit();
            }

            //// OPTION: launch deafault program to see file
            //PSI.FileName = pngFile;
            //using (Process exeProcess = Process.Start(PSI))
            //{
            //}
        }

        // HELPER FUNCTION //
        private void LoadFileIntoNetworkSettings(string inputFileName)
        {
            string[] fileLines = File.ReadAllLines(inputFileName);
            for (int i = 0; i < fileLines.Length; i++)
            {
                NetworkSettingInfo newNetSetting = new NetworkSettingInfo();
                
                string[] infoInLine = fileLines[i].Split(' ');
                newNetSetting.ID = infoInLine[1];
                newNetSetting.MSE = float.Parse(infoInLine[3],nf);
                newNetSetting.PercentileCorrect = float.Parse(infoInLine[6], nf);
                newNetSetting.MeanError = float.Parse(infoInLine[9], nf);
                newNetSetting.GuessedLostWhenFound = float.Parse(infoInLine[14], nf);
                newNetSetting.DidRetraining = bool.Parse(infoInLine[17]);
                i++; // first line done

                i++; // skip ---Network settings--- line

                newNetSetting.HiddenCells = new List<int>();
                while (fileLines[i].Contains("hidden layers no:"))
                {
                    infoInLine = fileLines[i].Split(' ');
                    newNetSetting.HiddenCells.Add(int.Parse(infoInLine[6]));
                    i++;
                }

                // Now we should be down on "Function hidden" line
                infoInLine = fileLines[i].Split(' ');
                newNetSetting.FunctionHidden = int.Parse(infoInLine[3]);
                newNetSetting.FunctionOutput = int.Parse(infoInLine[6]);
                newNetSetting.LearningRate = float.Parse(infoInLine[9], nf);
                newNetSetting.SteepnessHidden = float.Parse(infoInLine[12], nf);
                newNetSetting.SteepnessOutput = float.Parse(infoInLine[15], nf);
                newNetSetting.deterministicWeights = int.Parse(infoInLine[18]) == 1;
                i++;

                infoInLine = fileLines[i].Split(' ');
                newNetSetting.BiggestMSEDifference = float.Parse(infoInLine[3],nf);
                i++;

                // Now we fetch all the MSE values, aka last line.
                infoInLine = fileLines[i].Split(' ');
                newNetSetting.MSEValues = new List<float>();
                // Start from 2 to skip MSE and Values:
                for (int mseValue = 2; mseValue < infoInLine.Length; mseValue++)
                {
                    try
                    {
                        newNetSetting.MSEValues.Add(float.Parse(infoInLine[mseValue], nf));
                    }
                    catch (Exception)
                    {                        
                    }
                }
                i++; // This means we skip the empty line between networks

                allSettings.Add(newNetSetting);
            }
        }

        private void HiddenCellsExtraction()
        {
            Dictionary<int, List<float>> sameTypeSetting = new Dictionary<int, List<float>>();

            foreach (var setting in allSettings)
            {
                if (!sameTypeSetting.ContainsKey(setting.HiddenCells[0]))
                {
                    sameTypeSetting.Add(setting.HiddenCells[0], new List<float>());
                }

                sameTypeSetting[setting.HiddenCells[0]].Add(GetIntrestingSuccessValue(setting));
            }

            SaveInfoInDictionary(sameTypeSetting);
        }

        private void HiddenLayersExtraction()
        {
            Dictionary<int, List<float>> sameTypeSetting = new Dictionary<int, List<float>>();

            foreach (var setting in allSettings)
            {
                if (!sameTypeSetting.ContainsKey(setting.HiddenCells.Count))
                {
                    sameTypeSetting.Add(setting.HiddenCells.Count, new List<float>());
                }

                sameTypeSetting[setting.HiddenCells.Count].Add(GetIntrestingSuccessValue(setting));  
            }

            SaveInfoInDictionary(sameTypeSetting);
        }

        private void LearningRateExtraction()
        {
            Dictionary<float, List<float>> sameTypeSetting = new Dictionary<float, List<float>>();

            foreach (var setting in allSettings)
            {
                if (!sameTypeSetting.ContainsKey(setting.LearningRate))
                {
                    sameTypeSetting.Add(setting.LearningRate, new List<float>());
                }

                sameTypeSetting[setting.LearningRate].Add(GetIntrestingSuccessValue(setting));
            }

            SaveInfoInDictionary(sameTypeSetting);
        }

        private void SteepnessHiddenExtraction()
        {
            Dictionary<float, List<float>> sameTypeSetting = new Dictionary<float, List<float>>();

            foreach (var setting in allSettings)
            {
                if (!sameTypeSetting.ContainsKey(setting.SteepnessHidden))
                {
                    sameTypeSetting.Add(setting.SteepnessHidden, new List<float>());
                }

                sameTypeSetting[setting.SteepnessHidden].Add(GetIntrestingSuccessValue(setting));
            }

            SaveInfoInDictionary(sameTypeSetting);
        }

        private void SteepnessOutputExtraction()
        {
            Dictionary<float, List<float>> sameTypeSetting = new Dictionary<float, List<float>>();

            foreach (var setting in allSettings)
            {
                if (!sameTypeSetting.ContainsKey(setting.SteepnessOutput))
                {
                    sameTypeSetting.Add(setting.SteepnessOutput, new List<float>());
                }

                sameTypeSetting[setting.SteepnessOutput].Add(GetIntrestingSuccessValue(setting));
            }

            SaveInfoInDictionary(sameTypeSetting);
        }

        private void EpochsTrainedExtraction()
        {
            int totalEpochs = int.Parse(NumberOfEpochsTrained.Text);

            Dictionary<int, List<float>> sameTypeSetting = new Dictionary<int, List<float>>();

            foreach (var setting in allSettings)
            {
                for (int i = 0; i <= totalEpochs; i+= totalEpochs / (setting.MSEValues.Count - 1))
                {
                    if (!sameTypeSetting.ContainsKey(i))
                    {
                        sameTypeSetting.Add(i, new List<float>());
                    }

                    sameTypeSetting[i].Add(setting.MSEValues[i / (setting.MSEValues.Count - 1)]);
                }
            }
            SaveInfoInDictionary(sameTypeSetting);
        }

        private void MultipleNetworksExtraction()
        {
            // This whole thing is a special case
            float meanValue = 0;
            for (int i = 0; i < allSettings.Count; i++)
            {
                float value = GetIntrestingSuccessValue(allSettings[i]);
                infoToSave.Add((i+1) + " " + value);
                meanValue += value;
            }
            meanValue /= allSettings.Count;
            infoToSave.Insert(0 ,"0 " + meanValue);
        }

        private void SaveInfoInDictionary<TKey>(Dictionary<TKey, List<float>> dictionary)
        {
            foreach (var pair in dictionary)
            {
                string gnuCompatibleInfo = "";
                gnuCompatibleInfo += pair.Key;
                gnuCompatibleInfo += " ";
                float totalMSE = pair.Value.Sum();
                gnuCompatibleInfo += totalMSE / pair.Value.Count;
                foreach (var value in pair.Value)
                {
                    gnuCompatibleInfo += " ";
                    gnuCompatibleInfo += value;
                }
                // To remove possible , from float parsing
                gnuCompatibleInfo = gnuCompatibleInfo.Replace(',', '.');
                infoToSave.Add(gnuCompatibleInfo);
            }
        }

        private void DecideExtractionName()
        {
            RadioButton checkedButton = ExtractionMode.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked);
            if (checkedButton == radio_HiddenCells)
            {
                extractedDataFileName = "hidden cells.gnuplott";
            }
            else if (checkedButton == radio_HiddenLayers)
            {
                extractedDataFileName = "hidden layers.gnuplott";
            }
            else if (checkedButton == radio_LearningRate)
            {
                extractedDataFileName = "learning rate.gnuplott";
            }
            else if (checkedButton == radio_SteepnessHidden)
            {
                extractedDataFileName = "steepness hidden.gnuplott";
            }
            else if (checkedButton == radio_SteepnessOutput)
            {
                extractedDataFileName = "steepness output.gnuplott";
            }
            else if (checkedButton == radio_EpochsTrained)
            {
                extractedDataFileName = "epochs trained.gnuplott";
            }
            else if (checkedButton == radio_MultipleNetworks)
            {
                extractedDataFileName = "multiple networks.gnuplott";
            }
        }

        private void DecideInterestingValueName()
        {
            RadioButton checkedButton = YAxisValue.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked);
            if (checkedButton == YAxisValidatedMSE)
            {
                interestingValueName = "MSE";
            }
            else if (checkedButton == YAxisPercentileCorrect)
            {
                interestingValueName = "Percentile correct";
            }
            else if (checkedButton == YAxisMSEDifference)
            {
                interestingValueName = "MSE difference";
            }
            else if (checkedButton == YAxisLowestMSE)
            {
                interestingValueName = "Lowest MSE";
            }
            else if (checkedButton == YAxisMSEValues)
            {
                interestingValueName = "MSE value";
            }
        }

        private float GetIntrestingSuccessValue(NetworkSettingInfo p_setting)
        {
            RadioButton checkedButton = YAxisValue.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked);
            if (checkedButton == YAxisValidatedMSE)
            {
                return p_setting.MSE;
            }
            else if (checkedButton == YAxisPercentileCorrect)
            {
                return p_setting.PercentileCorrect;
            }
            else if (checkedButton == YAxisMSEDifference)
            {
                return p_setting.BiggestMSEDifference;
            }
            else if (checkedButton == YAxisLowestMSE)
            {
                return p_setting.MSEValues.Min();
            }
            return 0.0f;
        }
    }

    public class Pair<T, U>
    {
        public T first { get; set; }
        public U second { get; set; }
    }
}
