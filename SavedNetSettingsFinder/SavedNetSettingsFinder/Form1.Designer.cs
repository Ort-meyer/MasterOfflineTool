namespace SavedNetSettingsFinder
{
    partial class Form1
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
            this.minValidatedMSE = new System.Windows.Forms.TextBox();
            this.maxValidatedMSE = new System.Windows.Forms.TextBox();
            this.minNonValidatedMSE = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.maxNonValidatedMSE = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.minMSEChange = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.maxMSEChange = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.fileToUse = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // minValidatedMSE
            // 
            this.minValidatedMSE.Location = new System.Drawing.Point(180, 8);
            this.minValidatedMSE.Name = "minValidatedMSE";
            this.minValidatedMSE.Size = new System.Drawing.Size(90, 22);
            this.minValidatedMSE.TabIndex = 0;
            // 
            // maxValidatedMSE
            // 
            this.maxValidatedMSE.Location = new System.Drawing.Point(180, 36);
            this.maxValidatedMSE.Name = "maxValidatedMSE";
            this.maxValidatedMSE.Size = new System.Drawing.Size(90, 22);
            this.maxValidatedMSE.TabIndex = 1;
            // 
            // minNonValidatedMSE
            // 
            this.minNonValidatedMSE.Location = new System.Drawing.Point(180, 64);
            this.minNonValidatedMSE.Name = "minNonValidatedMSE";
            this.minNonValidatedMSE.Size = new System.Drawing.Size(90, 22);
            this.minNonValidatedMSE.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Min Validated MSE";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Max Validated MSE";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(-86, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(260, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Min Non-validated MSE";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(-86, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(260, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Max Non-validated MSE";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maxNonValidatedMSE
            // 
            this.maxNonValidatedMSE.Location = new System.Drawing.Point(180, 94);
            this.maxNonValidatedMSE.Name = "maxNonValidatedMSE";
            this.maxNonValidatedMSE.Size = new System.Drawing.Size(90, 22);
            this.maxNonValidatedMSE.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(-86, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(260, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Min MSE change";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // minMSEChange
            // 
            this.minMSEChange.Location = new System.Drawing.Point(180, 124);
            this.minMSEChange.Name = "minMSEChange";
            this.minMSEChange.Size = new System.Drawing.Size(90, 22);
            this.minMSEChange.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(-86, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(260, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "Max MSE change";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maxMSEChange
            // 
            this.maxMSEChange.Location = new System.Drawing.Point(180, 153);
            this.maxMSEChange.Name = "maxMSEChange";
            this.maxMSEChange.Size = new System.Drawing.Size(90, 22);
            this.maxMSEChange.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(136, 218);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Find net settings";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 218);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Brows File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // fileToUse
            // 
            this.fileToUse.AutoSize = true;
            this.fileToUse.Location = new System.Drawing.Point(12, 198);
            this.fileToUse.Name = "fileToUse";
            this.fileToUse.Size = new System.Drawing.Size(26, 17);
            this.fileToUse.TabIndex = 14;
            this.fileToUse.Text = "file";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.fileToUse);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.maxMSEChange);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.minMSEChange);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.maxNonValidatedMSE);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.minNonValidatedMSE);
            this.Controls.Add(this.maxValidatedMSE);
            this.Controls.Add(this.minValidatedMSE);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox minValidatedMSE;
        private System.Windows.Forms.TextBox maxValidatedMSE;
        private System.Windows.Forms.TextBox minNonValidatedMSE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox maxNonValidatedMSE;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox minMSEChange;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox maxMSEChange;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label fileToUse;
    }
}

