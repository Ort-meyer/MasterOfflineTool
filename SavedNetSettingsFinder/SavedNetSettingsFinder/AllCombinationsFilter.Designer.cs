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
            this.allwaysDeclain = new System.Windows.Forms.CheckBox();
            this.maxNumberOfMSEMissteps = new System.Windows.Forms.TextBox();
            this.lowestValidationPercent = new System.Windows.Forms.TextBox();
            this.meanValidationPercentError = new System.Windows.Forms.TextBox();
            this.highestValidatedMSE = new System.Windows.Forms.TextBox();
            this.highestUnvalidatedMSE = new System.Windows.Forms.TextBox();
            this.lowestValidationMeanPercent = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button1.Size = new System.Drawing.Size(299, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Folder ";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 244);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(299, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Send networks matching criteria to folder";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lowestValidationMeanPercent);
            this.groupBox1.Controls.Add(this.highestUnvalidatedMSE);
            this.groupBox1.Controls.Add(this.highestValidatedMSE);
            this.groupBox1.Controls.Add(this.meanValidationPercentError);
            this.groupBox1.Controls.Add(this.lowestValidationPercent);
            this.groupBox1.Controls.Add(this.maxNumberOfMSEMissteps);
            this.groupBox1.Controls.Add(this.allwaysDeclain);
            this.groupBox1.Location = new System.Drawing.Point(13, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 196);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Criteria";
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
            // maxNumberOfMSEMissteps
            // 
            this.maxNumberOfMSEMissteps.Location = new System.Drawing.Point(187, 22);
            this.maxNumberOfMSEMissteps.Name = "maxNumberOfMSEMissteps";
            this.maxNumberOfMSEMissteps.Size = new System.Drawing.Size(112, 22);
            this.maxNumberOfMSEMissteps.TabIndex = 5;
            this.maxNumberOfMSEMissteps.Text = "MAX missteps";
            // 
            // lowestValidationPercent
            // 
            this.lowestValidationPercent.Location = new System.Drawing.Point(6, 48);
            this.lowestValidationPercent.Name = "lowestValidationPercent";
            this.lowestValidationPercent.Size = new System.Drawing.Size(287, 22);
            this.lowestValidationPercent.TabIndex = 6;
            this.lowestValidationPercent.Text = "lowest validation percent";
            // 
            // meanValidationPercentError
            // 
            this.meanValidationPercentError.Location = new System.Drawing.Point(6, 104);
            this.meanValidationPercentError.Name = "meanValidationPercentError";
            this.meanValidationPercentError.Size = new System.Drawing.Size(287, 22);
            this.meanValidationPercentError.TabIndex = 7;
            this.meanValidationPercentError.Text = "mean validation percent error";
            // 
            // highestValidatedMSE
            // 
            this.highestValidatedMSE.Location = new System.Drawing.Point(6, 133);
            this.highestValidatedMSE.Name = "highestValidatedMSE";
            this.highestValidatedMSE.Size = new System.Drawing.Size(287, 22);
            this.highestValidatedMSE.TabIndex = 8;
            this.highestValidatedMSE.Text = "highest validated MSE";
            // 
            // highestUnvalidatedMSE
            // 
            this.highestUnvalidatedMSE.Location = new System.Drawing.Point(6, 161);
            this.highestUnvalidatedMSE.Name = "highestUnvalidatedMSE";
            this.highestUnvalidatedMSE.Size = new System.Drawing.Size(287, 22);
            this.highestUnvalidatedMSE.TabIndex = 9;
            this.highestUnvalidatedMSE.Text = "highest unvalidated MSE";
            // 
            // lowestValidationMeanPercent
            // 
            this.lowestValidationMeanPercent.Location = new System.Drawing.Point(6, 76);
            this.lowestValidationMeanPercent.Name = "lowestValidationMeanPercent";
            this.lowestValidationMeanPercent.Size = new System.Drawing.Size(287, 22);
            this.lowestValidationMeanPercent.TabIndex = 10;
            this.lowestValidationMeanPercent.Text = "lowest mean validation percent";
            // 
            // AllCombinationsFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 279);
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
    }
}