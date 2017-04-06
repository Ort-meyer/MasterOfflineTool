using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityScriptProject
{
    /*
     Class containing some utility stuff for gnuplot scripts*/
    class GnuplotUtilities
    {
        
        public static List<string> GetALLFilePaths(string sDir, List<string> p_fileNames)
        {
            foreach (string dir in Directory.GetDirectories(sDir))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    p_fileNames.Add(file);
                }
                GetALLFilePaths(dir, p_fileNames);
            }
            return p_fileNames;
        }
        // Calculates standard deviation according to https://www.mathsisfun.com/data/standard-deviation-formulas.html
        public static float CalculateStandardDeviation(List<float> p_numbers, float p_mean)
        {
            double correctSum = 0;
            foreach (float f in p_numbers)
            {
                // These casts tho...
                correctSum += Math.Pow(Convert.ToDouble(f - p_mean), 2);
            }
            double sumMean = correctSum / p_numbers.Count;
            float standardDeviation = (float)Math.Sqrt(sumMean);
            return standardDeviation;
        }
    }
}
