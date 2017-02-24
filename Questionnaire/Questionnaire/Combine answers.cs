using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Questionnaire
{
    public partial class Combine_answers : Form
    {
        private List<GroupBox> groupBoxes = new List<GroupBox>();
        public Combine_answers()
        {
            InitializeComponent();
            Form1 toCopyfrom = new Form1();
            foreach (var group in toCopyfrom.Controls.OfType<GroupBox>())
            {
                GroupBox newBox = new GroupBox();
                newBox.Name = group.Name;
                newBox.Text = group.Text;
                newBox.Location = group.Location;
                newBox.Padding = group.Padding;
                newBox.Size = group.Size;
                newBox.Parent = this;
                foreach (var alternative in group.Controls.OfType<RadioButton>())
                {
                    CheckBox newCheckBox = new CheckBox();
                    newCheckBox.AutoSize = true;
                    newCheckBox.Location = alternative.Location;
                    newCheckBox.Padding = alternative.Padding;
                    newCheckBox.Text = alternative.Text;
                    newCheckBox.CheckAlign = alternative.CheckAlign;                 
                    newCheckBox.Parent = newBox;
                }
                CheckBox allCheckbox = new CheckBox();
                allCheckbox.Location = new Point(newBox.Width - allCheckbox.Width/2, newBox.Height / 2 - allCheckbox.Height/2);
                allCheckbox.AutoSize = true;
                allCheckbox.Text ="all";
                allCheckbox.CheckAlign = ContentAlignment.TopCenter;
                allCheckbox.Parent = newBox;
                allCheckbox.CheckedChanged += HandleCustomEvent;
                groupBoxes.Add(newBox);
            }
        }

        private void combine_Click(object sender, EventArgs e)
        {
            string logDirectory = "DataLogs/";
            string[] subDirs = Directory.GetDirectories(logDirectory);
            foreach (var subdir in subDirs)
            {
                // Find if we are interested in this person
                string[] lines = File.ReadAllLines(subdir + "/" + "QuestionnaireAnsweres.txt");
                bool interesting = false;
                foreach (var line in lines)
                {
                    // Find the box that contains the answers
                    GroupBox interestedBox = new GroupBox();
                    foreach (var box in this.Controls.OfType<GroupBox>())
                    {
                        if (line.Contains(box.Text))
                        {
                            interestedBox = box;
                            break;
                        }
                    }

                    // Find if the checkbox for the answer given is checked as wanted
                    foreach (var checkbox in interestedBox.Controls.OfType<CheckBox>())
                    {
                        if (line.Contains(checkbox.Text))
                        {
                            interesting = true;
                            break;
                        }
                    }
                }

                if (interesting)
                {
                    CopyAllRawDataToNewDirectory(subdir);
                }

            }
        }

        void HandleCustomEvent(object sender, EventArgs a)
        {
            CheckBox theCheckbox = (CheckBox)sender;
            foreach (var item in theCheckbox.Parent.Controls.OfType<CheckBox>())
            {
                item.Checked = theCheckbox.Checked;
            }
            
            
        }

        void CopyAllRawDataToNewDirectory(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*.rawData");
            Directory.CreateDirectory("RawDataOfAnswer");
            foreach (var file in files)
            {
                string copyToLocation = "RawDataOfAnswer" + "/" + file.Substring(file.LastIndexOf("\\") + 1);
                File.Copy(file, copyToLocation);
            }
        }
    }
}
