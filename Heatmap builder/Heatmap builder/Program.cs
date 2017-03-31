using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Heatmap_builder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length != 0)
            {
                Form1 form = new Form1();
                // Since we are launching from a different place, it need to be from masterofflinetool/debug
                string golden = "../heatmaps/Golden/";
                string network = "../heatmaps/Calculated/";

                form.SetGoldenDataFolder(golden);
                form.SetCalculatedDataFolder(network);
                // Merged
                form.mergedFileName = "merged" + args[0];
                form.button2_Click(null, null);
            
                // Calculated
                form.mergedFileName = "calc" + args[0];
                form.SetGoldenDataFolder("");
                form.SetCalculatedDataFolder(network);
                form.button2_Click(null, null);
            
                // Golden
                form.mergedFileName = "gold" + args[0];
                form.SetGoldenDataFolder("");
                form.SetCalculatedDataFolder(golden);
                form.button2_Click(null, null);

                // Merged together all data
                form.mergedFileName = "ShowAll" + args[0];
                form.onlyShowHitsOnGolden = false;
                form.SetGoldenDataFolder(golden);
                form.SetCalculatedDataFolder(network);
                form.button2_Click(null, null);
            }
            else
            {
                Application.Run(new Form1());
            }
        }
    }
}
