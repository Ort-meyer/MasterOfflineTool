namespace SavedNetSettingsFinder
{
    partial class AllCombinationsFilter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bestMeanFalsePositiveError = new System.Windows.Forms.Label();
            this.bestFalsePositive = new System.Windows.Forms.Label();
            this.bestMeanFalsePositive = new System.Windows.Forms.Label();
            this.findOnMeanFalsePositiveError = new System.Windows.Forms.RadioButton();
            this.findOnFalsePositive = new System.Windows.Forms.RadioButton();
            this.findOnMeanFalsePositive = new System.Windows.Forms.RadioButton();
            this.criteriaMeanFalsePositiveError = new System.Windows.Forms.TextBox();
            this.criteriaHighestFalsePositive = new System.Windows.Forms.TextBox();
            this.criteriaHighestMeanFalsePositive = new System.Windows.Forms.TextBox();
            this.bestUnvalidatedMSE = new System.Windows.Forms.Label();
            this.bestValidatedMSE = new System.Windows.Forms.Label();
            this.bestMeanValidationPercentError = new System.Windows.Forms.Label();
            this.bestMeanValidationPercent = new System.Windows.Forms.Label();
            this.bestValidationPercent = new System.Windows.Forms.Label();
            this.findOnUnvalidatedMSE = new System.Windows.Forms.RadioButton();
            this.findOnValidatedMSE = new System.Windows.Forms.RadioButton();
            this.findOnValidationPercentError = new System.Windows.Forms.RadioButton();
            this.findOnMeanValidationPercent = new System.Windows.Forms.RadioButton();
            this.findOnValidationPercent = new System.Windows.Forms.RadioButton();
            this.lowestValidationMeanPercent = new System.Windows.Forms.TextBox();
            this.highestUnvalidatedMSE = new System.Windows.Forms.TextBox();
            this.highestValidatedMSE = new System.Windows.Forms.TextBox();
            this.meanValidationPercentError = new System.Windows.Forms.TextBox();
            this.lowestValidationPercent = new System.Windows.Forms.TextBox();
            this.maxNumberOfMSEMissteps = new System.Windows.Forms.TextBox();
            this.allwaysDeclain = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button1.Size = new System.Drawing.Size(299, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "../../../../Master Offline Tool/SavedNetSettingsDifferentStills/";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 367);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(299, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "../../../../Master Offline Tool/SavedNetSettings/PassedCriteria/";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bestMeanFalsePositiveError);
            this.groupBox1.Controls.Add(this.bestFalsePositive);
            this.groupBox1.Controls.Add(this.bestMeanFalsePositive);
            this.groupBox1.Controls.Add(this.findOnMeanFalsePositiveError);
            this.groupBox1.Controls.Add(this.findOnFalsePositive);
            this.groupBox1.Controls.Add(this.findOnMeanFalsePositive);
            this.groupBox1.Controls.Add(this.criteriaMeanFalsePositiveError);
            this.groupBox1.Controls.Add(this.criteriaHighestFalsePositive);
            this.groupBox1.Controls.Add(this.criteriaHighestMeanFalsePositive);
            this.groupBox1.Controls.Add(this.bestUnvalidatedMSE);
            this.groupBox1.Controls.Add(this.bestValidatedMSE);
            this.groupBox1.Controls.Add(this.bestMeanValidationPercentError);
            this.groupBox1.Controls.Add(this.bestMeanValidationPercent);
            this.groupBox1.Controls.Add(this.bestValidationPercent);
            this.groupBox1.Controls.Add(this.findOnUnvalidatedMSE);
            this.groupBox1.Controls.Add(this.findOnValidatedMSE);
            this.groupBox1.Controls.Add(this.findOnValidationPercentError);
            this.groupBox1.Controls.Add(this.findOnMeanValidationPercent);
            this.groupBox1.Controls.Add(this.findOnValidationPercent);
            this.groupBox1.Controls.Add(this.lowestValidationMeanPercent);
            this.groupBox1.Controls.Add(this.highestUnvalidatedMSE);
            this.groupBox1.Controls.Add(this.highestValidatedMSE);
            this.groupBox1.Controls.Add(this.meanValidationPercentError);
            this.groupBox1.Controls.Add(this.lowestValidationPercent);
            this.groupBox1.Controls.Add(this.maxNumberOfMSEMissteps);
            this.groupBox1.Controls.Add(this.allwaysDeclain);
            this.groupBox1.Location = new System.Drawing.Point(13, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(644, 319);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Criteria";
            // 
            // bestMeanFalsePositiveError
            // 
            this.bestMeanFalsePositiveError.AutoSize = true;
            this.bestMeanFalsePositiveError.Location = new System.Drawing.Point(261, 221);
            this.bestMeanFalsePositiveError.Name = "bestMeanFalsePositiveError";
            this.bestMeanFalsePositiveError.Size = new System.Drawing.Size(46, 17);
            this.bestMeanFalsePositiveError.TabIndex = 29;
            this.bestMeanFalsePositiveError.Text = "label5";
            // 
            // bestFalsePositive
            // 
            this.bestFalsePositive.AutoSize = true;
            this.bestFalsePositive.Location = new System.Drawing.Point(261, 249);
            this.bestFalsePositive.Name = "bestFalsePositive";
            this.bestFalsePositive.Size = new System.Drawing.Size(46, 17);
            this.bestFalsePositive.TabIndex = 28;
            this.bestFalsePositive.Text = "label5";
            // 
            // bestMeanFalsePositive
            // 
            this.bestMeanFalsePositive.AutoSize = true;
            this.bestMeanFalsePositive.Location = new System.Drawing.Point(261, 193);
            this.bestMeanFalsePositive.Name = "bestMeanFalsePositive";
            this.bestMeanFalsePositive.Size = new System.Drawing.Size(46, 17);
            this.bestMeanFalsePositive.TabIndex = 27;
            this.bestMeanFalsePositive.Text = "label5";
            // 
            // findOnMeanFalsePositiveError
            // 
            this.findOnMeanFalsePositiveError.AutoSize = true;
            this.findOnMeanFalsePositiveError.Location = new System.Drawing.Point(238, 221);
            this.findOnMeanFalsePositiveError.Name = "findOnMeanFalsePositiveError";
            this.findOnMeanFalsePositiveError.Size = new System.Drawing.Size(17, 16);
            this.findOnMeanFalsePositiveError.TabIndex = 26;
            this.findOnMeanFalsePositiveError.TabStop = true;
            this.findOnMeanFalsePositiveError.UseVisualStyleBackColor = true;
            // 
            // findOnFalsePositive
            // 
            this.findOnFalsePositive.AutoSize = true;
            this.findOnFalsePositive.Location = new System.Drawing.Point(238, 249);
            this.findOnFalsePositive.Name = "findOnFalsePositive";
            this.findOnFalsePositive.Size = new System.Drawing.Size(17, 16);
            this.findOnFalsePositive.TabIndex = 25;
            this.findOnFalsePositive.TabStop = true;
            this.findOnFalsePositive.UseVisualStyleBackColor = true;
            // 
            // findOnMeanFalsePositive
            // 
            this.findOnMeanFalsePositive.AutoSize = true;
            this.findOnMeanFalsePositive.Location = new System.Drawing.Point(238, 193);
            this.findOnMeanFalsePositive.Name = "findOnMeanFalsePositive";
            this.findOnMeanFalsePositive.Size = new System.Drawing.Size(17, 16);
            this.findOnMeanFalsePositive.TabIndex = 24;
            this.findOnMeanFalsePositive.TabStop = true;
            this.findOnMeanFalsePositive.UseVisualStyleBackColor = true;
            // 
            // criteriaMeanFalsePositiveError
            // 
            this.criteriaMeanFalsePositiveError.Location = new System.Drawing.Point(6, 218);
            this.criteriaMeanFalsePositiveError.Name = "criteriaMeanFalsePositiveError";
            this.criteriaMeanFalsePositiveError.Size = new System.Drawing.Size(226, 22);
            this.criteriaMeanFalsePositiveError.TabIndex = 23;
            this.criteriaMeanFalsePositiveError.Text = "Highest mean false positive error";
            // 
            // criteriaHighestFalsePositive
            // 
            this.criteriaHighestFalsePositive.Location = new System.Drawing.Point(6, 246);
            this.criteriaHighestFalsePositive.Name = "criteriaHighestFalsePositive";
            this.criteriaHighestFalsePositive.Size = new System.Drawing.Size(226, 22);
            this.criteriaHighestFalsePositive.TabIndex = 22;
            this.criteriaHighestFalsePositive.Text = "Highest false positive";
            // 
            // criteriaHighestMeanFalsePositive
            // 
            this.criteriaHighestMeanFalsePositive.Location = new System.Drawing.Point(6, 190);
            this.criteriaHighestMeanFalsePositive.Name = "criteriaHighestMeanFalsePositive";
            this.criteriaHighestMeanFalsePositive.Size = new System.Drawing.Size(226, 22);
            this.criteriaHighestMeanFalsePositive.TabIndex = 21;
            this.criteriaHighestMeanFalsePositive.Text = "Highest mean false positive";
            // 
            // bestUnvalidatedMSE
            // 
            this.bestUnvalidatedMSE.AutoSize = true;
            this.bestUnvalidatedMSE.Location = new System.Drawing.Point(261, 164);
            this.bestUnvalidatedMSE.Name = "bestUnvalidatedMSE";
            this.bestUnvalidatedMSE.Size = new System.Drawing.Size(46, 17);
            this.bestUnvalidatedMSE.TabIndex = 20;
            this.bestUnvalidatedMSE.Text = "label5";
            // 
            // bestValidatedMSE
            // 
            this.bestValidatedMSE.AutoSize = true;
            this.bestValidatedMSE.Location = new System.Drawing.Point(261, 136);
            this.bestValidatedMSE.Name = "bestValidatedMSE";
            this.bestValidatedMSE.Size = new System.Drawing.Size(46, 17);
            this.bestValidatedMSE.TabIndex = 19;
            this.bestValidatedMSE.Text = "label4";
            // 
            // bestMeanValidationPercentError
            // 
            this.bestMeanValidationPercentError.AutoSize = true;
            this.bestMeanValidationPercentError.Location = new System.Drawing.Point(261, 107);
            this.bestMeanValidationPercentError.Name = "bestMeanValidationPercentError";
            this.bestMeanValidationPercentError.Size = new System.Drawing.Size(46, 17);
            this.bestMeanValidationPercentError.TabIndex = 18;
            this.bestMeanValidationPercentError.Text = "label3";
            // 
            // bestMeanValidationPercent
            // 
            this.bestMeanValidationPercent.AutoSize = true;
            this.bestMeanValidationPercent.Location = new System.Drawing.Point(261, 79);
            this.bestMeanValidationPercent.Name = "bestMeanValidationPercent";
            this.bestMeanValidationPercent.Size = new System.Drawing.Size(46, 17);
            this.bestMeanValidationPercent.TabIndex = 17;
            this.bestMeanValidationPercent.Text = "label2";
            // 
            // bestValidationPercent
            // 
            this.bestValidationPercent.AutoSize = true;
            this.bestValidationPercent.Location = new System.Drawing.Point(261, 51);
            this.bestValidationPercent.Name = "bestValidationPercent";
            this.bestValidationPercent.Size = new System.Drawing.Size(46, 17);
            this.bestValidationPercent.TabIndex = 16;
            this.bestValidationPercent.Text = "label1";
            // 
            // findOnUnvalidatedMSE
            // 
            this.findOnUnvalidatedMSE.AutoSize = true;
            this.findOnUnvalidatedMSE.Location = new System.Drawing.Point(238, 167);
            this.findOnUnvalidatedMSE.Name = "findOnUnvalidatedMSE";
            this.findOnUnvalidatedMSE.Size = new System.Drawing.Size(17, 16);
            this.findOnUnvalidatedMSE.TabIndex = 15;
            this.findOnUnvalidatedMSE.TabStop = true;
            this.findOnUnvalidatedMSE.UseVisualStyleBackColor = true;
            // 
            // findOnValidatedMSE
            // 
            this.findOnValidatedMSE.AutoSize = true;
            this.findOnValidatedMSE.Location = new System.Drawing.Point(238, 139);
            this.findOnValidatedMSE.Name = "findOnValidatedMSE";
            this.findOnValidatedMSE.Size = new System.Drawing.Size(17, 16);
            this.findOnValidatedMSE.TabIndex = 14;
            this.findOnValidatedMSE.TabStop = true;
            this.findOnValidatedMSE.UseVisualStyleBackColor = true;
            // 
            // findOnValidationPercentError
            // 
            this.findOnValidationPercentError.AutoSize = true;
            this.findOnValidationPercentError.Location = new System.Drawing.Point(238, 110);
            this.findOnValidationPercentError.Name = "findOnValidationPercentError";
            this.findOnValidationPercentError.Size = new System.Drawing.Size(17, 16);
            this.findOnValidationPercentError.TabIndex = 13;
            this.findOnValidationPercentError.TabStop = true;
            this.findOnValidationPercentError.UseVisualStyleBackColor = true;
            // 
            // findOnMeanValidationPercent
            // 
            this.findOnMeanValidationPercent.AutoSize = true;
            this.findOnMeanValidationPercent.Location = new System.Drawing.Point(238, 82);
            this.findOnMeanValidationPercent.Name = "findOnMeanValidationPercent";
            this.findOnMeanValidationPercent.Size = new System.Drawing.Size(17, 16);
            this.findOnMeanValidationPercent.TabIndex = 12;
            this.findOnMeanValidationPercent.TabStop = true;
            this.findOnMeanValidationPercent.UseVisualStyleBackColor = true;
            // 
            // findOnValidationPercent
            // 
            this.findOnValidationPercent.AutoSize = true;
            this.findOnValidationPercent.Location = new System.Drawing.Point(238, 54);
            this.findOnValidationPercent.Name = "findOnValidationPercent";
            this.findOnValidationPercent.Size = new System.Drawing.Size(17, 16);
            this.findOnValidationPercent.TabIndex = 11;
            this.findOnValidationPercent.TabStop = true;
            this.findOnValidationPercent.UseVisualStyleBackColor = true;
            // 
            // lowestValidationMeanPercent
            // 
            this.lowestValidationMeanPercent.Location = new System.Drawing.Point(6, 76);
            this.lowestValidationMeanPercent.Name = "lowestValidationMeanPercent";
            this.lowestValidationMeanPercent.Size = new System.Drawing.Size(226, 22);
            this.lowestValidationMeanPercent.TabIndex = 10;
            this.lowestValidationMeanPercent.Text = "lowest mean validation percent";
            // 
            // highestUnvalidatedMSE
            // 
            this.highestUnvalidatedMSE.Location = new System.Drawing.Point(6, 161);
            this.highestUnvalidatedMSE.Name = "highestUnvalidatedMSE";
            this.highestUnvalidatedMSE.Size = new System.Drawing.Size(226, 22);
            this.highestUnvalidatedMSE.TabIndex = 9;
            this.highestUnvalidatedMSE.Text = "highest unvalidated MSE";
            // 
            // highestValidatedMSE
            // 
            this.highestValidatedMSE.Location = new System.Drawing.Point(6, 133);
            this.highestValidatedMSE.Name = "highestValidatedMSE";
            this.highestValidatedMSE.Size = new System.Drawing.Size(226, 22);
            this.highestValidatedMSE.TabIndex = 8;
            this.highestValidatedMSE.Text = "highest validated MSE";
            // 
            // meanValidationPercentError
            // 
            this.meanValidationPercentError.Location = new System.Drawing.Point(6, 104);
            this.meanValidationPercentError.Name = "meanValidationPercentError";
            this.meanValidationPercentError.Size = new System.Drawing.Size(226, 22);
            this.meanValidationPercentError.TabIndex = 7;
            this.meanValidationPercentError.Text = "mean validation percent error";
            // 
            // lowestValidationPercent
            // 
            this.lowestValidationPercent.Location = new System.Drawing.Point(6, 48);
            this.lowestValidationPercent.Name = "lowestValidationPercent";
            this.lowestValidationPercent.Size = new System.Drawing.Size(226, 22);
            this.lowestValidationPercent.TabIndex = 6;
            this.lowestValidationPercent.Text = "lowest validation percent";
            // 
            // maxNumberOfMSEMissteps
            // 
            this.maxNumberOfMSEMissteps.Location = new System.Drawing.Point(187, 22);
            this.maxNumberOfMSEMissteps.Name = "maxNumberOfMSEMissteps";
            this.maxNumberOfMSEMissteps.Size = new System.Drawing.Size(112, 22);
            this.maxNumberOfMSEMissteps.TabIndex = 5;
            this.maxNumberOfMSEMissteps.Text = "MAX missteps";
            // 
            // allwaysDeclain
            // 
            this.allwaysDeclain.AutoSize = true;
            this.allwaysDeclain.Location = new System.Drawing.Point(6, 21);
            this.allwaysDeclain.Name = "allwaysDeclain";
            this.allwaysDeclain.Size = new System.Drawing.Size(174, 21);
            this.allwaysDeclain.TabIndex = 4;
            this.allwaysDeclain.Text = "Always declaining MSE";
            this.allwaysDeclain.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 397);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(1140, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Find best network";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // AllCombinationsFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 431);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Name = "AllCombinationsFilter";
            this.Text = "AllCombinationsFilter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox highestValidatedMSE;
        private System.Windows.Forms.TextBox meanValidationPercentError;
        private System.Windows.Forms.TextBox lowestValidationPercent;
        private System.Windows.Forms.TextBox maxNumberOfMSEMissteps;
        private System.Windows.Forms.CheckBox allwaysDeclain;
        private System.Windows.Forms.TextBox highestUnvalidatedMSE;
        private System.Windows.Forms.TextBox lowestValidationMeanPercent;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton findOnUnvalidatedMSE;
        private System.Windows.Forms.RadioButton findOnValidatedMSE;
        private System.Windows.Forms.RadioButton findOnValidationPercentError;
        private System.Windows.Forms.RadioButton findOnMeanValidationPercent;
        private System.Windows.Forms.RadioButton findOnValidationPercent;
        private System.Windows.Forms.Label bestUnvalidatedMSE;
        private System.Windows.Forms.Label bestValidatedMSE;
        private System.Windows.Forms.Label bestMeanValidationPercentError;
        private System.Windows.Forms.Label bestMeanValidationPercent;
        private System.Windows.Forms.Label bestValidationPercent;
        private System.Windows.Forms.Label bestMeanFalsePositiveError;
        private System.Windows.Forms.Label bestFalsePositive;
        private System.Windows.Forms.Label bestMeanFalsePositive;
        private System.Windows.Forms.RadioButton findOnMeanFalsePositiveError;
        private System.Windows.Forms.RadioButton findOnFalsePositive;
        private System.Windows.Forms.RadioButton findOnMeanFalsePositive;
        private System.Windows.Forms.TextBox criteriaMeanFalsePositiveError;
        private System.Windows.Forms.TextBox criteriaHighestFalsePositive;
        private System.Windows.Forms.TextBox criteriaHighestMeanFalsePositive;
    }
}