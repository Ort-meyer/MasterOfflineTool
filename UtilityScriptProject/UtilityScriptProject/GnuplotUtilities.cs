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
    }
}
