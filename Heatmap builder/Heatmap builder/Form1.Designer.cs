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
            ((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
            this.SuspendLayout();
            // 
            // GenerateHeatmap
            // 
            this.GenerateHeatmap.Location = new System.Drawing.Point(9, 379);
            this.GenerateHeatmap.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GenerateHeatmap.Name = "GenerateHeatmap";
            this.GenerateHeatmap.Size = new System.Drawing.Size(240, 46);
            this.GenerateHeatmap.TabIndex = 0;
            this.GenerateHeatmap.Text = "Generate Heatmap";
            this.GenerateHeatmap.UseVisualStyleBackColor = true;
            this.GenerateHeatmap.Click += new System.EventHandler(this.GenerateHeatmap_Click);
            // 
            // Preview
            // 
            this.Preview.Location = new System.Drawing.Point(274, 10);
            this.Preview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(374, 261);
            this.Preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Preview.TabIndex = 1;
            this.Preview.TabStop = false;
            // 
            // InFolder
            // 
            this.InFolder.Location = new System.Drawing.Point(148, 10);
            this.InFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.InFolder.Name = "InFolder";
            this.InFolder.Size = new System.Drawing.Size(123, 20);
            this.InFolder.TabIndex = 3;
            this.InFolder.Text = "LostData";
            // 
            // OutFolder
            // 
            this.OutFolder.Location = new System.Drawing.Point(148, 32);
            this.OutFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OutFolder.Name = "OutFolder";
            this.OutFolder.Size = new System.Drawing.Size(123, 20);
            this.OutFolder.TabIndex = 4;
            this.OutFolder.Text = "HeatMaps";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Folder containing data files";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Folder to save heatmaps";
            // 
            // TextureWidth
            // 
            this.TextureWidth.Location = new System.Drawing.Point(148, 76);
            this.TextureWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TextureWidth.Name = "TextureWidth";
            this.TextureWidth.Size = new System.Drawing.Size(123, 20);
            this.TextureWidth.TabIndex = 8;
            this.TextureWidth.Text = "4000";
            // 
            // TextureHeight
            // 
            this.TextureHeight.Location = new System.Drawing.Point(148, 99);
            this.TextureHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TextureHeight.Name = "TextureHeight";
            this.TextureHeight.Size = new System.Drawing.Size(123, 20);
            this.TextureHeight.TabIndex = 9;
            this.TextureHeight.Text = "4000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Pixel width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Pixel height";
            // 
            // propagation
            // 
            this.propagation.Location = new System.Drawing.Point(148, 125);
            this.propagation.Name = "propagation";
            this.propagation.Size = new System.Drawing.Size(123, 20);
            this.propagation.TabIndex = 12;
            this.propagation.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Pixel propagation";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Underlaying image";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(148, 152);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
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
            this.imageLeftPositionX.Location = new System.Drawing.Point(148, 204);
            this.imageLeftPositionX.Name = "imageLeftPositionX";
            this.imageLeftPositionX.Size = new System.Drawing.Size(121, 20);
            this.imageLeftPositionX.TabIndex = 17;
            // 
            // imageLowerPositionY
            // 
            this.imageLowerPositionY.Location = new System.Drawing.Point(148, 230);
            this.imageLowerPositionY.Name = "imageLowerPositionY";
            this.imageLowerPositionY.Size = new System.Drawing.Size(121, 20);
            this.imageLowerPositionY.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 204);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Image left X pos";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 230);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Image lower Y pos";
            // 
            // sizeFromImage
            // 
            this.sizeFromImage.AutoSize = true;
            this.sizeFromImage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sizeFromImage.Location = new System.Drawing.Point(9, 57);
            this.sizeFromImage.Name = "sizeFromImage";
            this.sizeFromImage.Size = new System.Drawing.Size(154, 17);
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
            this.fitPositionsToImage.Location = new System.Drawing.Point(13, 181);
            this.fitPositionsToImage.Name = "fitPositionsToImage";
            this.fitPositionsToImage.Size = new System.Drawing.Size(148, 17);
            this.fitPositionsToImage.TabIndex = 22;
            this.fitPositionsToImage.Text = "Fit positions to image        ";
            this.fitPositionsToImage.UseVisualStyleBackColor = true;
            this.fitPositionsToImage.CheckedChanged += new System.EventHandler(this.fitPositionsToImage_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 282);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Image upper Y pos";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 256);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "Image right X pos";
            // 
            // imageUpperPositionY
            // 
            this.imageUpperPositionY.Location = new System.Drawing.Point(148, 282);
            this.imageUpperPositionY.Name = "imageUpperPositionY";
            this.imageUpperPositionY.Size = new System.Drawing.Size(121, 20);
            this.imageUpperPositionY.TabIndex = 24;
            // 
            // imageRightPositionX
            // 
            this.imageRightPositionX.Location = new System.Drawing.Point(148, 256);
            this.imageRightPositionX.Name = "imageRightPositionX";
            this.imageRightPositionX.Size = new System.Drawing.Size(121, 20);
            this.imageRightPositionX.TabIndex = 23;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 435);
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
            this.Controls.Add(this.InFolder);
            this.Controls.Add(this.Preview);
            this.Controls.Add(this.GenerateHeatmap);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
    }
}

