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
            this.InFolderNetworkData = new System.Windows.Forms.TextBox();
            this.OutFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TextureWidth = new System.Windows.Forms.TextBox();
            this.TextureHeight = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.propagation = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.imageLeftPositionX = new System.Windows.Forms.TextBox();
            this.imageLowerPositionY = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.sizeFromImage = new System.Windows.Forms.CheckBox();
            this.fitPositionsToImage = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.imageUpperPositionY = new System.Windows.Forms.TextBox();
            this.imageRightPositionX = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.inFolderGoldenData = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ColorWrongLost = new System.Windows.Forms.PictureBox();
            this.ColorCorrectLost = new System.Windows.Forms.PictureBox();
            this.ColorGoldenLost = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorWrongLost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorCorrectLost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorGoldenLost)).BeginInit();
            this.SuspendLayout();
            // 
            // GenerateHeatmap
            // 
            this.GenerateHeatmap.Location = new System.Drawing.Point(12, 556);
            this.GenerateHeatmap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GenerateHeatmap.Name = "GenerateHeatmap";
            this.GenerateHeatmap.Size = new System.Drawing.Size(320, 57);
            this.GenerateHeatmap.TabIndex = 0;
            this.GenerateHeatmap.Text = "Generate Heatmap";
            this.GenerateHeatmap.UseVisualStyleBackColor = true;
            this.GenerateHeatmap.Click += new System.EventHandler(this.GenerateHeatmap_Click);
            // 
            // Preview
            // 
            this.Preview.Location = new System.Drawing.Point(365, 12);
            this.Preview.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(374, 261);
            this.Preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Preview.TabIndex = 1;
            this.Preview.TabStop = false;
            // 
            // InFolderNetworkData
            // 
            this.InFolderNetworkData.Location = new System.Drawing.Point(197, 12);
            this.InFolderNetworkData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InFolderNetworkData.Name = "InFolderNetworkData";
            this.InFolderNetworkData.Size = new System.Drawing.Size(163, 22);
            this.InFolderNetworkData.TabIndex = 3;
            this.InFolderNetworkData.Text = "../../../../Master Offline Tool/Heatmaps/Calculated/";
            // 
            // OutFolder
            // 
            this.OutFolder.Location = new System.Drawing.Point(197, 64);
            this.OutFolder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.label2.Location = new System.Drawing.Point(13, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Folder to save heatmaps";
            // 
            // TextureWidth
            // 
            this.TextureWidth.Location = new System.Drawing.Point(197, 119);
            this.TextureWidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TextureWidth.Name = "TextureWidth";
            this.TextureWidth.Size = new System.Drawing.Size(163, 22);
            this.TextureWidth.TabIndex = 8;
            this.TextureWidth.Text = "1000";
            // 
            // TextureHeight
            // 
            this.TextureHeight.Location = new System.Drawing.Point(197, 147);
            this.TextureHeight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TextureHeight.Name = "TextureHeight";
            this.TextureHeight.Size = new System.Drawing.Size(163, 22);
            this.TextureHeight.TabIndex = 9;
            this.TextureHeight.Text = "1000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 119);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Pixel width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 147);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Pixel height";
            // 
            // propagation
            // 
            this.propagation.Location = new System.Drawing.Point(197, 179);
            this.propagation.Margin = new System.Windows.Forms.Padding(4);
            this.propagation.Name = "propagation";
            this.propagation.Size = new System.Drawing.Size(163, 22);
            this.propagation.TabIndex = 12;
            this.propagation.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 179);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "Pixel propagation";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 212);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 17);
            this.label6.TabIndex = 15;
            this.label6.Text = "Underlaying image";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(197, 212);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(161, 28);
            this.button1.TabIndex = 16;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // imageLeftPositionX
            // 
            this.imageLeftPositionX.Location = new System.Drawing.Point(197, 276);
            this.imageLeftPositionX.Margin = new System.Windows.Forms.Padding(4);
            this.imageLeftPositionX.Name = "imageLeftPositionX";
            this.imageLeftPositionX.Size = new System.Drawing.Size(160, 22);
            this.imageLeftPositionX.TabIndex = 17;
            this.imageLeftPositionX.Text = "99135";
            // 
            // imageLowerPositionY
            // 
            this.imageLowerPositionY.Location = new System.Drawing.Point(197, 308);
            this.imageLowerPositionY.Margin = new System.Windows.Forms.Padding(4);
            this.imageLowerPositionY.Name = "imageLowerPositionY";
            this.imageLowerPositionY.Size = new System.Drawing.Size(160, 22);
            this.imageLowerPositionY.TabIndex = 18;
            this.imageLowerPositionY.Text = "-158175";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 276);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 17);
            this.label7.TabIndex = 19;
            this.label7.Text = "Image left X pos";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 308);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 17);
            this.label8.TabIndex = 20;
            this.label8.Text = "Image lower Y pos";
            // 
            // sizeFromImage
            // 
            this.sizeFromImage.AutoSize = true;
            this.sizeFromImage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sizeFromImage.Location = new System.Drawing.Point(12, 95);
            this.sizeFromImage.Margin = new System.Windows.Forms.Padding(4);
            this.sizeFromImage.Name = "sizeFromImage";
            this.sizeFromImage.Size = new System.Drawing.Size(204, 21);
            this.sizeFromImage.TabIndex = 21;
            this.sizeFromImage.Text = "Use underlaying image size";
            this.sizeFromImage.UseVisualStyleBackColor = true;
            this.sizeFromImage.CheckedChanged += new System.EventHandler(this.sizeFromImage_CheckedChanged);
            // 
            // fitPositionsToImage
            // 
            this.fitPositionsToImage.AutoSize = true;
            this.fitPositionsToImage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.fitPositionsToImage.Checked = true;
            this.fitPositionsToImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fitPositionsToImage.Location = new System.Drawing.Point(17, 248);
            this.fitPositionsToImage.Margin = new System.Windows.Forms.Padding(4);
            this.fitPositionsToImage.Name = "fitPositionsToImage";
            this.fitPositionsToImage.Size = new System.Drawing.Size(195, 21);
            this.fitPositionsToImage.TabIndex = 22;
            this.fitPositionsToImage.Text = "Fit positions to image        ";
            this.fitPositionsToImage.UseVisualStyleBackColor = true;
            this.fitPositionsToImage.CheckedChanged += new System.EventHandler(this.fitPositionsToImage_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 372);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(127, 17);
            this.label9.TabIndex = 26;
            this.label9.Text = "Image upper Y pos";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 340);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 17);
            this.label10.TabIndex = 25;
            this.label10.Text = "Image right X pos";
            // 
            // imageUpperPositionY
            // 
            this.imageUpperPositionY.Location = new System.Drawing.Point(197, 372);
            this.imageUpperPositionY.Margin = new System.Windows.Forms.Padding(4);
            this.imageUpperPositionY.Name = "imageUpperPositionY";
            this.imageUpperPositionY.Size = new System.Drawing.Size(160, 22);
            this.imageUpperPositionY.TabIndex = 24;
            this.imageUpperPositionY.Text = "-100200";
            // 
            // imageRightPositionX
            // 
            this.imageRightPositionX.Location = new System.Drawing.Point(197, 340);
            this.imageRightPositionX.Margin = new System.Windows.Forms.Padding(4);
            this.imageRightPositionX.Name = "imageRightPositionX";
            this.imageRightPositionX.Size = new System.Drawing.Size(160, 22);
            this.imageRightPositionX.TabIndex = 23;
            this.imageRightPositionX.Text = "154537";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 38);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(178, 17);
            this.label11.TabIndex = 28;
            this.label11.Text = "Folder containing data files";
            // 
            // inFolderGoldenData
            // 
            this.inFolderGoldenData.Location = new System.Drawing.Point(197, 38);
            this.inFolderGoldenData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.inFolderGoldenData.Name = "inFolderGoldenData";
            this.inFolderGoldenData.Size = new System.Drawing.Size(163, 22);
            this.inFolderGoldenData.TabIndex = 27;
            this.inFolderGoldenData.Text = "../../../../Master Offline Tool/Heatmaps/Golden/";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 490);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(320, 57);
            this.button2.TabIndex = 29;
            this.button2.Text = "Generate Merged Heatmap";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(17, 402);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(216, 21);
            this.checkBox1.TabIndex = 30;
            this.checkBox1.Text = "Only show hits on golden       ";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ColorWrongLost
            // 
            this.ColorWrongLost.Location = new System.Drawing.Point(22, 429);
            this.ColorWrongLost.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ColorWrongLost.Name = "ColorWrongLost";
            this.ColorWrongLost.Size = new System.Drawing.Size(20, 20);
            this.ColorWrongLost.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ColorWrongLost.TabIndex = 31;
            this.ColorWrongLost.TabStop = false;
            this.ColorWrongLost.Click += new System.EventHandler(this.ColorWrongLost_Click);
            // 
            // ColorCorrectLost
            // 
            this.ColorCorrectLost.Location = new System.Drawing.Point(73, 429);
            this.ColorCorrectLost.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ColorCorrectLost.Name = "ColorCorrectLost";
            this.ColorCorrectLost.Size = new System.Drawing.Size(20, 20);
            this.ColorCorrectLost.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ColorCorrectLost.TabIndex = 32;
            this.ColorCorrectLost.TabStop = false;
            this.ColorCorrectLost.Click += new System.EventHandler(this.ColorCorrectLost_Click);
            // 
            // ColorGoldenLost
            // 
            this.ColorGoldenLost.Location = new System.Drawing.Point(123, 429);
            this.ColorGoldenLost.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ColorGoldenLost.Name = "ColorGoldenLost";
            this.ColorGoldenLost.Size = new System.Drawing.Size(20, 20);
            this.ColorGoldenLost.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ColorGoldenLost.TabIndex = 33;
            this.ColorGoldenLost.TabStop = false;
            this.ColorGoldenLost.Click += new System.EventHandler(this.ColorGoldenLost_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 623);
            this.Controls.Add(this.ColorGoldenLost);
            this.Controls.Add(this.ColorCorrectLost);
            this.Controls.Add(this.ColorWrongLost);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.inFolderGoldenData);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.imageUpperPositionY);
            this.Controls.Add(this.imageRightPositionX);
            this.Controls.Add(this.fitPositionsToImage);
            this.Controls.Add(this.sizeFromImage);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.imageLowerPositionY);
            this.Controls.Add(this.imageLeftPositionX);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.propagation);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TextureHeight);
            this.Controls.Add(this.TextureWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OutFolder);
            this.Controls.Add(this.InFolderNetworkData);
            this.Controls.Add(this.Preview);
            this.Controls.Add(this.GenerateHeatmap);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorWrongLost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorCorrectLost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorGoldenLost)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GenerateHeatmap;
        private System.Windows.Forms.PictureBox Preview;
        private System.Windows.Forms.TextBox InFolderNetworkData;
        private System.Windows.Forms.TextBox OutFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextureWidth;
        private System.Windows.Forms.TextBox TextureHeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox propagation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox imageLeftPositionX;
        private System.Windows.Forms.TextBox imageLowerPositionY;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox sizeFromImage;
        private System.Windows.Forms.CheckBox fitPositionsToImage;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox imageUpperPositionY;
        private System.Windows.Forms.TextBox imageRightPositionX;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox inFolderGoldenData;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox ColorWrongLost;
        private System.Windows.Forms.PictureBox ColorCorrectLost;
        private System.Windows.Forms.PictureBox ColorGoldenLost;
    }
}

