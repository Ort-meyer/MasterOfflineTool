using System;
using System.Collections.Generic;
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
                string golden = form.GetGoldenDataFolder();
                string network = form.GetCalculatedDataFolder();
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
            }
            else
            {
                Application.Run(new Form1());
            }
        }
    }
}
