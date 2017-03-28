using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SavedNetSettingsFinder
{
    public partial class AllCombinationsFilter : Form
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
            public int TrainingAlgorithm;
            public float LearningRate;
            public float SteepnessHidden;
            public float SteepnessOutput;
            public bool deterministicWeights;
            public float BiggestMSEDifference;
            public List<float> MSEValues;
        }

        public AllCombinationsFilter()
        {
            InitializeComponent();
        }

        List<NetworkSettingInfo> allSettingsInFile;
        List<string> filesThatPassedCriteria;

        private void button3_Click(object sender, EventArgs e)
        {
            // Reset values
            allSettingsInFile = new List<NetworkSettingInfo>();
            filesThatPassedCriteria = new List<string>();

            string saveFolder = "";
            DialogResult result = folderBrowserDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                saveFolder = folderBrowserDialog1.SelectedPath;
            }
            if (saveFolder == "")
            {
                saveFolder = button3.Text;
            }
            string filesRootFolder = button1.Text;
            List<string> allFilesInFolderStructure = new List<string>();
            allFilesInFolderStructure = GetAllFilesInFolderStructure(filesRootFolder, "netSettings");

            foreach (var file in allFilesInFolderStructure)
            {
                allSettingsInFile.Clear();
                LoadFileIntoNetworkSettings(file);
                if (PassedCriteria(allSettingsInFile))
                {
                    filesThatPassedCriteria.Add(file);
                }
            }

            // copy files to new directory
            foreach (var file in filesThatPassedCriteria)
            {
                string fileName = file.Substring(file.LastIndexOf('\\'));
                File.Copy(file, saveFolder + fileName, true);
            }
        }

        private bool PassedCriteria(List<NetworkSettingInfo> p_allSettingsInFile)
        {
            bool passed = true;
            float minValidationPercent = 0;
            float minMeanValidationPercent= 0;
            float maxMeanValidationPercentError = float.MaxValue;
            float maxValidatedMSE = float.MaxValue;
            float maxUnValidatedMSE = float.MaxValue;
            float maxMeanFalsePositive = float.MaxValue;
            float maxMeanFalsePositiveError = float.MaxValue;
            float maxFalsePositive = float.MaxValue;

            // Figure out what we will use
            float.TryParse(lowestValidationPercent.Text, NumberStyles.Any, nf, out minValidationPercent);
            float.TryParse(lowestValidationMeanPercent.Text, NumberStyles.Any, nf, out minMeanValidationPercent);
            if (!float.TryParse(meanValidationPercentError.Text, NumberStyles.Any, nf, out maxMeanValidationPercentError))
            {
                maxMeanValidationPercentError = float.MaxValue;
            }
            if (!float.TryParse(highestValidatedMSE.Text, NumberStyles.Any, nf, out maxValidatedMSE))
            {
                maxValidatedMSE = float.MaxValue;
            }
            if (!float.TryParse(highestUnvalidatedMSE.Text, NumberStyles.Any, nf, out maxUnValidatedMSE))
            {
                maxUnValidatedMSE = float.MaxValue;
            }
            if (!float.TryParse(criteriaHighestMeanFalsePositive.Text, NumberStyles.Any, nf, out maxMeanFalsePositive))
            {
                maxMeanFalsePositive = float.MaxValue;
            }
            if (!float.TryParse(criteriaMeanFalsePositiveError.Text, NumberStyles.Any, nf, out maxMeanFalsePositiveError))
            {
                maxMeanFalsePositiveError = float.MaxValue;
            }
            if (!float.TryParse(criteriaHighestFalsePositive.Text, NumberStyles.Any, nf, out maxFalsePositive))
            {
                maxFalsePositive = float.MaxValue;
            }

            float netValidationPercent;
            float netMeanValidationPercent;
            float netMeanValidationPercentError;
            float netValidatedMSE;
            float netUnValidatedMSE;
            float netMeanFalsePositive;
            float netMeanFalsePositivesError;
            float netHighestFalsePositive;

            GetNetValues(out netValidationPercent, out netMeanValidationPercent, out netMeanValidationPercentError,
                out netValidatedMSE, out netUnValidatedMSE, out netMeanFalsePositive, out netMeanFalsePositivesError,
                out netHighestFalsePositive);

            // Check criteria
            if (netValidationPercent < minValidationPercent)
            {
                passed = false;
            }
            if (netMeanValidationPercent < minMeanValidationPercent)
            {
                passed = false;
            }
            if (netMeanValidationPercentError > maxMeanValidationPercentError)
            {
                passed = false;
            }
            if (netValidatedMSE > maxValidatedMSE)
            {
                passed = false;
            }
            if (netUnValidatedMSE > maxUnValidatedMSE)
            {
                passed = false;
            }
            if (netMeanFalsePositive > maxMeanFalsePositive)
            {
                passed = false;
            }
            if (netMeanFalsePositivesError > maxMeanFalsePositiveError)
            {
                passed = false;
            }
            if (netHighestFalsePositive > maxFalsePositive)
            {
                passed = false;
            }

            return passed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string folder = folderBrowserDialog1.SelectedPath;
                button1.Text = folder;
            }
        }

        // SHOULD BE IDENTICAL TO THE FUNCTION IN GNUPLOT EXTRACTOR!!!!
        private void LoadFileIntoNetworkSettings(string inputFileName)
        {
            string[] fileLines = File.ReadAllLines(inputFileName);
            for (int i = 0; i < fileLines.Length; i++)
            {
                NetworkSettingInfo newNetSetting = new NetworkSettingInfo();

                string[] infoInLine = fileLines[i].Split(' ');
                newNetSetting.ID = infoInLine[1];
                newNetSetting.MSE = float.Parse(infoInLine[3], nf);
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
                newNetSetting.TrainingAlgorithm = int.Parse(infoInLine[9]);
                newNetSetting.LearningRate = float.Parse(infoInLine[12], nf);
                newNetSetting.SteepnessHidden = float.Parse(infoInLine[15], nf);
                newNetSetting.SteepnessOutput = float.Parse(infoInLine[18], nf);
                newNetSetting.deterministicWeights = int.Parse(infoInLine[21]) == 1;
                i++;

                infoInLine = fileLines[i].Split(' ');
                newNetSetting.BiggestMSEDifference = float.Parse(infoInLine[3], nf);
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

                allSettingsInFile.Add(newNetSetting);
            }
        }

        private List<string> GetAllFilesInFolderStructure(string p_rootDirectory, string p_fileEnding)
        {
            List<string> files = new List<string>();
            // FInd all subdirectories
            string[] subDirectories = Directory.GetDirectories(p_rootDirectory);
            foreach (var directoryPath in subDirectories)
            {
                // Go to the next subfolder and do the same thing again
                files.AddRange(GetAllFilesInFolderStructure(directoryPath, p_fileEnding));
            }
            // When we have been to the bottom of the directory structure we look for .json files
            files.AddRange(Directory.GetFiles(p_rootDirectory, "PosRot*"));
            return files;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Reset values
            allSettingsInFile = new List<NetworkSettingInfo>();

            string filesRootFolder = button1.Text;
            List<string> allFilesInFolderStructure = new List<string>();
            allFilesInFolderStructure = GetAllFilesInFolderStructure(filesRootFolder, "netSettings");
            string bestFile = "";

            float bestNetValidationPercent = 0;
            float bestNetMeanValidationPercent = 0;
            float bestNetMeanValidationPercentError = float.MaxValue;
            float bestNetValidatedMSE = float.MaxValue;
            float bestNetUnValidatedMSE = float.MaxValue;
            float bestNetMeanFalsePositives = float.MaxValue;
            float bestNetMeanFalsePositivesError = float.MaxValue;
            float bestNetFalsePositives = float.MaxValue;

            foreach (var file in allFilesInFolderStructure)
            {
                allSettingsInFile.Clear();
                LoadFileIntoNetworkSettings(file);

                float netValidationPercent;
                float netMeanValidationPercent;
                float netMeanValidationPercentError;
                float netValidatedMSE;
                float netUnValidatedMSE;
                float netMeanFalsePositives;
                float netMeanFalsePositivesError;
                float netHighestFalsePositive;

                GetNetValues(out netValidationPercent, out netMeanValidationPercent, out netMeanValidationPercentError, 
                    out netValidatedMSE, out netUnValidatedMSE, out netMeanFalsePositives, out netMeanFalsePositivesError,
                    out netHighestFalsePositive);

                if ((findOnMeanValidationPercent.Checked && bestNetMeanValidationPercent < netMeanValidationPercent)
                    || (findOnValidationPercent.Checked && bestNetValidationPercent < netValidationPercent)
                    || (findOnValidatedMSE.Checked && bestNetValidatedMSE > netValidatedMSE)
                    || (findOnUnvalidatedMSE.Checked && bestNetUnValidatedMSE > netUnValidatedMSE)
                    || (findOnValidationPercentError.Checked && bestNetMeanValidationPercentError > netMeanValidationPercentError)
                    || (findOnMeanFalsePositive.Checked && bestNetMeanFalsePositives > netMeanFalsePositives)
                    || (findOnMeanFalsePositiveError.Checked && bestNetMeanFalsePositivesError > netMeanFalsePositivesError)
                    || (findOnFalsePositive.Checked && bestNetFalsePositives > netHighestFalsePositive))
                {
                    bestNetMeanValidationPercent = netMeanValidationPercent;
                    bestNetMeanValidationPercentError = netMeanValidationPercentError;
                    bestNetValidationPercent = netValidationPercent;
                    bestNetUnValidatedMSE = netUnValidatedMSE;
                    bestNetValidatedMSE = netValidatedMSE;
                    bestNetMeanFalsePositives = netMeanFalsePositives;
                    bestNetMeanFalsePositivesError = netMeanFalsePositivesError;
                    bestNetFalsePositives = netHighestFalsePositive;
                    bestFile = file;
                }
            }

            // Precent the best net
            button2.Text = bestFile.Substring(bestFile.LastIndexOf('\\'));
            bestMeanValidationPercent.Text = bestNetMeanValidationPercent.ToString();
            bestValidationPercent.Text = bestNetValidationPercent.ToString();
            bestMeanValidationPercentError.Text = bestNetMeanValidationPercentError.ToString();
            bestUnvalidatedMSE.Text = bestNetUnValidatedMSE.ToString();
            bestValidatedMSE.Text = bestNetValidatedMSE.ToString();
            bestMeanFalsePositive.Text = bestNetMeanFalsePositives.ToString();
            bestMeanFalsePositiveError.Text = bestNetMeanFalsePositivesError.ToString();
            bestFalsePositive.Text = bestNetFalsePositives.ToString();
        }

        // HELPER
        void GetNetValues(out float ValidationPercent,out float MeanValidationPercent,out float MeanValidationPercentError,
        out float ValidatedMSE,out float UnValidatedMSE, out float MeanFalsePositives, out float MeanFalsePositivesError, out float FalsePositives)
        {
            float netValidationPercent = 0;
            float netMeanValidationPercent = 0;
            float netMeanValidationPercentError = 0;
            float netValidatedMSE = float.MaxValue;
            float netUnValidatedMSE = float.MaxValue;
            float netMeanFalsePositives = 0;
            float netMeanFalsePositivesError = 0;
            float netFalsePositives = 0;

            foreach (var net in allSettingsInFile)
            {
                netMeanValidationPercent += net.PercentileCorrect;
                netMeanFalsePositives += net.GuessedLostWhenFound;
            }
            netMeanValidationPercent /= allSettingsInFile.Count;
            netMeanFalsePositives /= allSettingsInFile.Count;

            foreach (var net in allSettingsInFile)
            {
                if (net.PercentileCorrect > netValidationPercent)
                {
                    netValidationPercent = net.PercentileCorrect;
                }
                if (net.MSE < netValidatedMSE)
                {
                    netValidatedMSE = net.MSE;
                }
                if (net.GuessedLostWhenFound > netFalsePositives)
                {
                    netFalsePositives = net.GuessedLostWhenFound;
                }
                netMeanValidationPercentError += Math.Abs(net.PercentileCorrect - netMeanValidationPercent);
                netMeanFalsePositivesError += Math.Abs(net.GuessedLostWhenFound - netMeanFalsePositives);
                foreach (var item in net.MSEValues)
                {
                    if (item < netUnValidatedMSE)
                    {
                        netUnValidatedMSE = item;
                    }
                }
            }
            netMeanValidationPercentError /= allSettingsInFile.Count;
            netMeanFalsePositivesError /= allSettingsInFile.Count;

            ValidationPercent = netValidationPercent;
            MeanValidationPercent = netMeanValidationPercent;
            MeanValidationPercentError = netMeanValidationPercentError;
            ValidatedMSE = netValidatedMSE;
            UnValidatedMSE = netUnValidatedMSE;
            MeanFalsePositives = netMeanFalsePositives;
            MeanFalsePositivesError = netMeanFalsePositivesError;
            FalsePositives = netFalsePositives;
        }
    }
}
