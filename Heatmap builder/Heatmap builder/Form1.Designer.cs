namespace Heatmap_builder
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
            this.GenerateHeatmap = new System.Windows.Forms.Button();
            this.Preview = new System.Windows.Forms.PictureBox();
            this.InFolder = new System.Windows.Forms.TextBox();
            this.OutFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
            this.SuspendLayout();
            // 
            // GenerateHeatmap
            // 
            this.GenerateHeatmap.Location = new System.Drawing.Point(12, 467);
            this.GenerateHeatmap.Name = "GenerateHeatmap";
            this.GenerateHeatmap.Size = new System.Drawing.Size(320, 56);
            this.GenerateHeatmap.TabIndex = 0;
            this.GenerateHeatmap.Text = "Generate Heatmap";
            this.GenerateHeatmap.UseVisualStyleBackColor = true;
            this.GenerateHeatmap.Click += new System.EventHandler(this.GenerateHeatmap_Click);
            // 
            // Preview
            // 
            this.Preview.Location = new System.Drawing.Point(366, 12);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(374, 261);
            this.Preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Preview.TabIndex = 1;
            this.Preview.TabStop = false;
            // 
            // InFolder
            // 
            this.InFolder.Location = new System.Drawing.Point(197, 12);
            this.InFolder.Name = "InFolder";
            this.InFolder.Size = new System.Drawing.Size(163, 22);
            this.InFolder.TabIndex = 3;
            this.InFolder.Text = "LostData";
            // 
            // OutFolder
            // 
            this.OutFolder.Location = new System.Drawing.Point(197, 40);
            this.OutFolder.Name = "OutFolder";
            this.OutFolder.Size = new System.Drawing.Size(163, 22);
            this.OutFolder.TabIndex = 4;
            this.OutFolder.Text = "HeatMaps";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Folder containing data files";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Folder to save heatmaps";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 535);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OutFolder);
            this.Controls.Add(this.InFolder);
            this.Controls.Add(this.Preview);
            this.Controls.Add(this.GenerateHeatmap);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GenerateHeatmap;
        private System.Windows.Forms.PictureBox Preview;
        private System.Windows.Forms.TextBox InFolder;
        private System.Windows.Forms.TextBox OutFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

