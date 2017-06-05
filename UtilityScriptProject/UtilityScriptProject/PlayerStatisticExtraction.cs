using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityScriptProject
{
    class PlayerStatisticExtraction
    {
        string rawDataFolder = "../../../../../DataLogs/";
        Dictionary<string, int>[] answers = new Dictionary<string, int>[4];
        //Dictionary<string, int> erfarenMedMMORPG = new Dictionary<string, int>();
        //Dictionary<string, int> hurOftaMMORPGPerVecka = new Dictionary<string, int>();
        //Dictionary<string, int> hurOftaDatorspelPerVecka = new Dictionary<string, int>();
        public PlayerStatisticExtraction()
        {
            for (int i = 0; i < 4; i++)
            {
                answers[i] = new Dictionary<string, int>();
            }
            answers[0].Add("Ja", 0);
            answers[0].Add("Nej", 0);

            answers[1].Add("Inte alls", 0);
            answers[1].Add("Något", 0);
            answers[1].Add("Medium", 0);
            answers[1].Add("Mycket", 0);
            answers[1].Add("Jättemycket", 0);

            answers[2].Add("< 3 h", 0);
            answers[2].Add("3 h - 6 h", 0);
            answers[2].Add("6 h - 9 h", 0);
            answers[2].Add("> 9 h", 0);

            answers[3].Add("< 3 h", 0);
            answers[3].Add("3 h - 6 h", 0);
            answers[3].Add("6 h - 9 h", 0);
            answers[3].Add("> 9 h", 0);

            List<string> allAnswersFiles = GnuplotUtilities.GetAllFilesInFolderStructure(rawDataFolder, "txt");
            foreach (var item in allAnswersFiles)
            {
                string[] lines = File.ReadAllLines(item);
                for (int i = 0; i < 4; i++)
                {
                    string answer = lines[i].Substring(lines[i].LastIndexOf(':') + 2);
                    if (answer.Last() == ' ')
                    {
                        answer = answer.Remove(answer.Length - 1);
                    }
                    if (!answers[i].ContainsKey(answer))
                    {
                        answers[i].Add(answer,0);
                    }
                    answers[i][answer]++;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                string filename = "";
                string result = "";
                switch (i)
                {
                    case 0:
                        filename = "HarSpelatMO.txt";
                        result = "Yes. " + answers[i]["Ja"] + "\nNo. " + answers[i]["Nej"];
                        break;
                    case 1:
                        filename = "ErfarenhetMedMMORPG.txt";
                        result = "No experience. " + answers[i]["Inte alls"] + "\nSome experience. " + answers[i]["Något"] + 
                            "\nMedium Experience. " + answers[i]["Medium"] + "\nVery experienced. " + answers[i]["Mycket"] + 
                            "\nSuper experienced. " + answers[i]["Jättemycket"];
                        break;
                    case 2:
                        filename = "FrekvensMMORPGSpelande.txt";
                        result = "< 3 h. " + answers[i]["< 3 h"] + "\n3 h - 6 h. " + answers[i]["3 h - 6 h"] +
                            "\n6 h - 9 h. " + answers[i]["6 h - 9 h"] + "\n> 9 h. " + answers[i]["> 9 h"];
                        break;
                    case 3:
                        filename = "FrekvensDatorspelande.txt";
                        result = "< 3 h. " + answers[i]["< 3 h"] + "\n3 h - 6 h. " + answers[i]["3 h - 6 h"] +
                        "\n6 h - 9 h. " + answers[i]["6 h - 9 h"] + "\n> 9 h. " + answers[i]["> 9 h"];
                        break;
                    default:
                        break;
                }
                File.WriteAllText(filename, result);
            }
        }
    }
}
