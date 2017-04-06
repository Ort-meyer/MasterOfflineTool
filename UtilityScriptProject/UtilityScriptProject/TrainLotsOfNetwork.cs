using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityScriptProject
{
    class TrainLotsOfNetwork
    {
        int[] entriesToAvrage = { 60,20,120};
        int[] entriesToMerge = {20,10,5 };
        string[] combinationsToUse = { "PosRot", "KeyPos", "KeyRot" ,"Pos", "Rot" ,"Key" ,"KeyPosRot" };

        int[][] hiddenLayers = { 
        new int[]{ 40, 0, 0, 0, 0 },
        new int[]{ 20, 0, 0, 0, 0 },
        new int[]{ 10, 0, 0, 0, 0 },
        new int[]{ 10, 5, 0, 0, 0 },
        new int[]{ 20, 10, 5, 0, 0 },
        new int[]{ 40, 30, 20, 10, 0 },
        new int[]{ 100, 70, 50, 30, 10 },
        new int[]{ 20, 60, 20, 0,0 },
        new int[]{ 20, 40, 80, 20, 0 },};

        int[] hiddenLayerCount = {
        1,
        1,
        1,
        2,
        3,
        4,
        5,
        3,
        4
        };
        string rootFolder = "../../../../Master Offline Tool/SavedNetSettingsDifferentStills/";
        string masterOfflineFolder = "../../../../Master Offline Tool/Debug/";
        string settingsFolder = "../../../../Master Offline Tool/SavedNetSettings/";
        public TrainLotsOfNetwork()
        {
            for (int avrage = 0; avrage < entriesToAvrage.Length; avrage++)
            {
                for (int merge = 0; merge < entriesToMerge.Length; merge++)
                {
                    int avrageRate = entriesToAvrage[avrage];
                    int mergeRate = entriesToMerge[merge];
                    if (ShouldRunStillSetting(avrageRate, mergeRate))
                    {
                        // RUN STILL WITH SETTING
                        ProcessStartInfo PSI = new ProcessStartInfo();
                        PSI.FileName = masterOfflineFolder + "DataStill64.exe";
                        PSI.WorkingDirectory = masterOfflineFolder;
                        string arguments = avrageRate.ToString() + " " + mergeRate.ToString();
                        PSI.Arguments = arguments;
                        PSI.UseShellExecute = false;
                        using (Process exeProcess = Process.Start(PSI))
                        {
                            exeProcess.WaitForExit();
                        }

                        LoopMasterOfflineTool(avrageRate, mergeRate);

                        // Copy data to new folder
                        MoveDataToCorrectLocation(avrageRate, mergeRate);
                    }
                }
            }
        }

        private void LoopMasterOfflineTool(int avrage, int merge)
        {
            for (int combinationIndex = 0; combinationIndex < combinationsToUse.Length; combinationIndex++)
            {
                for (int hidddenLayersindex = 0; hidddenLayersindex < hiddenLayers.Length; hidddenLayersindex++)
                {
                    string combo = combinationsToUse[combinationIndex];
                    int[] layers = hiddenLayers[hidddenLayersindex];
                    int count = hiddenLayerCount[hidddenLayersindex];
                    if (ShouldRunNetworkSetting(avrage, merge, combo, layers, count))
                    {
                        //RUN STILL WITH SETTING
                        ProcessStartInfo PSI = new ProcessStartInfo();
                        PSI.FileName = masterOfflineFolder + "Master Offline Tool.exe";
                        PSI.WorkingDirectory = masterOfflineFolder;
                        string arguments = combo + " " + count + " " + layers[0] + " " + layers[1] + " " + layers[2] + " " + layers[3] + " " + layers[4] + " " + avrage + " " + merge;
                        PSI.Arguments = arguments;
                        PSI.UseShellExecute = false;
                        using (Process exeProcess = Process.Start(PSI))
                        {
                            exeProcess.WaitForExit();
                        }
                    }
                }
            }
        }

        private bool ShouldRunStillSetting(int avrage, int merge)
        {
            bool shouldRun = false;
            string folderName = rootFolder + avrage.ToString() + " " + merge.ToString();
            List<string> files = GetAllFilesInFolderStructure(folderName, "");
            foreach (var item in files)
            {
                string fileName = item.Substring(item.LastIndexOf('\\') + 1);
                if (!char.IsNumber(fileName[0]))
                {
                    shouldRun = true;
                    File.Delete(item);
                }
            }
            if (files.Count != 7*9)
            {
                shouldRun = true;
            }
            return shouldRun;
        }

        private bool ShouldRunNetworkSetting(int avrage,int merge,string combo, int[] layers, int layerCount)
        {
            string fileLocation = rootFolder + avrage.ToString() + " " + merge.ToString() + "/" + combo + "/1110/";
            string fileName = avrage.ToString() + merge.ToString() + combo + " 11 10 " + layerCount.ToString() + " ";
            for (int i = 0; i < layerCount; i++)
            {
                fileName += " " + layers[i].ToString();
            }
            fileName += " 11 10 2 0.7 0.6 0.6 1";
            bool run = !File.Exists(fileLocation + fileName);
            return run;
        }

        private void MoveDataToCorrectLocation(int avrage, int merge)
        {
            string directoryToMoveTo = rootFolder + "/" + avrage + " " + merge;
            if (!Directory.Exists(directoryToMoveTo))
            {
                Directory.CreateDirectory(directoryToMoveTo);
            }
            MoveAll(settingsFolder, directoryToMoveTo);   
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
            files.AddRange(Directory.GetFiles(p_rootDirectory, "*" + p_fileEnding));
            return files;
        }

        public static void MoveAll(string source, string target)
        {
            if (source.ToLower() == target.ToLower())
            {
                return;
            }

            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target) == false)
            {
                Directory.CreateDirectory(target);
            }

            // Copy each file into it's new directory.
            foreach (var fi in Directory.GetFiles( source))
            {
                if (fi.Contains("allActivationFunctions.bigrun"))
                {
                    continue;
                }
                string goodFile = fi.Replace('\\', '/');
                File.Copy(goodFile, target + "/" + goodFile.Substring(goodFile.LastIndexOf('/')), true);
                File.Delete(fi);
            }

            // Copy each subdirectory using recursion.
            foreach (var sub in Directory.GetDirectories( source))
            {
                string goodSub = sub.Replace('\\', '/');
                string nextTargetSubDir = target + "/" + goodSub.Substring(goodSub.LastIndexOf('/'));           
                MoveAll(goodSub, nextTargetSubDir);
                Directory.Delete(sub);
            }
        }

    }
}
