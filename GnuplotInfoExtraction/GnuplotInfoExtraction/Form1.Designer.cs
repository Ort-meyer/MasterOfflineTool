namespace GnuplotInfoExtraction
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
            this.FileBrowser = new System.Windows.Forms.Button();
            this.NumberOfEpochsTrained = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ExtractionMode = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.radio_MultipleNetworks = new System.Windows.Forms.RadioButton();
            this.radio_EpochsTrained = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.radio_SteepnessOutput = new System.Windows.Forms.RadioButton();
            this.radio_SteepnessHidden = new System.Windows.Forms.RadioButton();
            this.radio_LearningRate = new System.Windows.Forms.RadioButton();
            this.radio_HiddenLayers = new System.Windows.Forms.RadioButton();
            this.radio_HiddenCells = new System.Windows.Forms.RadioButton();
            this.YAxisMSEValues = new System.Windows.Forms.RadioButton();
            this.ExtractData = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.GnuPlot = new System.Windows.Forms.Button();
            this.PlotYAxisName = new System.Windows.Forms.TextBox();
            this.PlotXAxisName = new System.Windows.Forms.TextBox();
            this.PlotTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.YAxisValue = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.YAxisLowestMSE = new System.Windows.Forms.RadioButton();
            this.YAxisMSEDifference = new System.Windows.Forms.RadioButton();
            this.YAxisPercentileCorrect = new System.Windows.Forms.RadioButton();
            this.YAxisValidatedMSE = new System.Windows.Forms.RadioButton();
            this.ExtractionMode.SuspendLayout();
            this.YAxisValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // FileBrowser
            // 
            this.FileBrowser.Location = new System.Drawing.Point(13, 13);
            this.FileBrowser.Name = "FileBrowser";
            this.FileBrowser.Size = new System.Drawing.Size(325, 23);
            this.FileBrowser.TabIndex = 0;
            this.FileBrowser.Text = "Brows for file";
            this.FileBrowser.UseVisualStyleBackColor = true;
            this.FileBrowser.Click += new System.EventHandler(this.FileBrowser_Click);
            // 
            // NumberOfEpochsTrained
            // 
            this.NumberOfEpochsTrained.Location = new System.Drawing.Point(130, 42);
            this.NumberOfEpochsTrained.Name = "NumberOfEpochsTrained";
            this.NumberOfEpochsTrained.Size = new System.Drawing.Size(208, 22);
            this.NumberOfEpochsTrained.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Total epochs";
            // 
            // ExtractionMode
            // 
            this.ExtractionMode.Controls.Add(this.label7);
            this.ExtractionMode.Controls.Add(this.radio_MultipleNetworks);
            this.ExtractionMode.Controls.Add(this.radio_EpochsTrained);
            this.ExtractionMode.Controls.Add(this.label5);
            this.ExtractionMode.Controls.Add(this.radio_SteepnessOutput);
            this.ExtractionMode.Controls.Add(this.radio_SteepnessHidden);
            this.ExtractionMode.Controls.Add(this.radio_LearningRate);
            this.ExtractionMode.Controls.Add(this.radio_HiddenLayers);
            this.ExtractionMode.Controls.Add(this.radio_HiddenCells);
            this.ExtractionMode.Location = new System.Drawing.Point(16, 70);
            this.ExtractionMode.Name = "ExtractionMode";
            this.ExtractionMode.Size = new System.Drawing.Size(322, 232);
            this.ExtractionMode.TabIndex = 3;
            this.ExtractionMode.TabStop = false;
            this.ExtractionMode.Text = "Extraction mode";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(4, 180);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(312, 22);
            this.label7.TabIndex = 9;
            this.label7.Text = "Below is options that creates a staple diagram";
            // 
            // radio_MultipleNetworks
            // 
            this.radio_MultipleNetworks.AutoSize = true;
            this.radio_MultipleNetworks.Location = new System.Drawing.Point(6, 205);
            this.radio_MultipleNetworks.Name = "radio_MultipleNetworks";
            this.radio_MultipleNetworks.Size = new System.Drawing.Size(139, 21);
            this.radio_MultipleNetworks.TabIndex = 8;
            this.radio_MultipleNetworks.TabStop = true;
            this.radio_MultipleNetworks.Text = "Multiple Networks";
            this.radio_MultipleNetworks.UseVisualStyleBackColor = true;
            // 
            // radio_EpochsTrained
            // 
            this.radio_EpochsTrained.AutoSize = true;
            this.radio_EpochsTrained.Location = new System.Drawing.Point(6, 156);
            this.radio_EpochsTrained.Name = "radio_EpochsTrained";
            this.radio_EpochsTrained.Size = new System.Drawing.Size(124, 21);
            this.radio_EpochsTrained.TabIndex = 7;
            this.radio_EpochsTrained.TabStop = true;
            this.radio_EpochsTrained.Text = "Epochs trained";
            this.radio_EpochsTrained.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(312, 53);
            this.label5.TabIndex = 6;
            this.label5.Text = "Below is options for when the file only contains one type of network. This only w" +
    "orks with Y axis for one type of network";
            // 
            // radio_SteepnessOutput
            // 
            this.radio_SteepnessOutput.AutoSize = true;
            this.radio_SteepnessOutput.Location = new System.Drawing.Point(7, 76);
            this.radio_SteepnessOutput.Name = "radio_SteepnessOutput";
            this.radio_SteepnessOutput.Size = new System.Drawing.Size(140, 21);
            this.radio_SteepnessOutput.TabIndex = 4;
            this.radio_SteepnessOutput.TabStop = true;
            this.radio_SteepnessOutput.Text = "Steepness output";
            this.radio_SteepnessOutput.UseVisualStyleBackColor = true;
            // 
            // radio_SteepnessHidden
            // 
            this.radio_SteepnessHidden.AutoSize = true;
            this.radio_SteepnessHidden.Location = new System.Drawing.Point(157, 49);
            this.radio_SteepnessHidden.Name = "radio_SteepnessHidden";
            this.radio_SteepnessHidden.Size = new System.Drawing.Size(143, 21);
            this.radio_SteepnessHidden.TabIndex = 3;
            this.radio_SteepnessHidden.TabStop = true;
            this.radio_SteepnessHidden.Text = "Steepness hidden";
            this.radio_SteepnessHidden.UseVisualStyleBackColor = true;
            // 
            // radio_LearningRate
            // 
            this.radio_LearningRate.AutoSize = true;
            this.radio_LearningRate.Location = new System.Drawing.Point(7, 49);
            this.radio_LearningRate.Name = "radio_LearningRate";
            this.radio_LearningRate.Size = new System.Drawing.Size(114, 21);
            this.radio_LearningRate.TabIndex = 2;
            this.radio_LearningRate.TabStop = true;
            this.radio_LearningRate.Text = "Learning rate";
            this.radio_LearningRate.UseVisualStyleBackColor = true;
            // 
            // radio_HiddenLayers
            // 
            this.radio_HiddenLayers.AutoSize = true;
            this.radio_HiddenLayers.Location = new System.Drawing.Point(157, 22);
            this.radio_HiddenLayers.Name = "radio_HiddenLayers";
            this.radio_HiddenLayers.Size = new System.Drawing.Size(116, 21);
            this.radio_HiddenLayers.TabIndex = 1;
            this.radio_HiddenLayers.TabStop = true;
            this.radio_HiddenLayers.Text = "Hidden layers";
            this.radio_HiddenLayers.UseVisualStyleBackColor = true;
            // 
            // radio_HiddenCells
            // 
            this.radio_HiddenCells.AutoSize = true;
            this.radio_HiddenCells.Location = new System.Drawing.Point(7, 22);
            this.radio_HiddenCells.Name = "radio_HiddenCells";
            this.radio_HiddenCells.Size = new System.Drawing.Size(106, 21);
            this.radio_HiddenCells.TabIndex = 0;
            this.radio_HiddenCells.TabStop = true;
            this.radio_HiddenCells.Text = "Hidden cells";
            this.radio_HiddenCells.UseVisualStyleBackColor = true;
            // 
            // YAxisMSEValues
            // 
            this.YAxisMSEValues.AutoSize = true;
            this.YAxisMSEValues.Location = new System.Drawing.Point(6, 112);
            this.YAxisMSEValues.Name = "YAxisMSEValues";
            this.YAxisMSEValues.Size = new System.Drawing.Size(105, 21);
            this.YAxisMSEValues.TabIndex = 5;
            this.YAxisMSEValues.TabStop = true;
            this.YAxisMSEValues.Text = "MSE Values";
            this.YAxisMSEValues.UseVisualStyleBackColor = true;
            // 
            // ExtractData
            // 
            this.ExtractData.Location = new System.Drawing.Point(16, 457);
            this.ExtractData.Name = "ExtractData";
            this.ExtractData.Size = new System.Drawing.Size(322, 23);
            this.ExtractData.TabIndex = 4;
            this.ExtractData.Text = "Extract Data";
            this.ExtractData.UseVisualStyleBackColor = true;
            this.ExtractData.Click += new System.EventHandler(this.ExtractData_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // GnuPlot
            // 
            this.GnuPlot.Location = new System.Drawing.Point(16, 571);
            this.GnuPlot.Name = "GnuPlot";
            this.GnuPlot.Size = new System.Drawing.Size(322, 23);
            this.GnuPlot.TabIndex = 5;
            this.GnuPlot.Text = "Build GNU plot";
            this.GnuPlot.UseVisualStyleBackColor = true;
            this.GnuPlot.Click += new System.EventHandler(this.GnuPlot_Click);
            // 
            // PlotYAxisName
            // 
            this.PlotYAxisName.Location = new System.Drawing.Point(191, 543);
            this.PlotYAxisName.Name = "PlotYAxisName";
            this.PlotYAxisName.Size = new System.Drawing.Size(147, 22);
            this.PlotYAxisName.TabIndex = 6;
            // 
            // PlotXAxisName
            // 
            this.PlotXAxisName.Location = new System.Drawing.Point(191, 515);
            this.PlotXAxisName.Name = "PlotXAxisName";
            this.PlotXAxisName.Size = new System.Drawing.Size(147, 22);
            this.PlotXAxisName.TabIndex = 7;
            // 
            // PlotTitle
            // 
            this.PlotTitle.Location = new System.Drawing.Point(191, 487);
            this.PlotTitle.Name = "PlotTitle";
            this.PlotTitle.Size = new System.Drawing.Size(147, 22);
            this.PlotTitle.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 488);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Plot title";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 515);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "X axis name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 543);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Y axis name";
            // 
            // YAxisValue
            // 
            this.YAxisValue.Controls.Add(this.label6);
            this.YAxisValue.Controls.Add(this.YAxisMSEValues);
            this.YAxisValue.Controls.Add(this.YAxisLowestMSE);
            this.YAxisValue.Controls.Add(this.YAxisMSEDifference);
            this.YAxisValue.Controls.Add(this.YAxisPercentileCorrect);
            this.YAxisValue.Controls.Add(this.YAxisValidatedMSE);
            this.YAxisValue.Location = new System.Drawing.Point(16, 308);
            this.YAxisValue.Name = "YAxisValue";
            this.YAxisValue.Size = new System.Drawing.Size(322, 143);
            this.YAxisValue.TabIndex = 12;
            this.YAxisValue.TabStop = false;
            this.YAxisValue.Text = "Y axis value";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(312, 37);
            this.label6.TabIndex = 7;
            this.label6.Text = "Below is options for when the file only contains one type of network";
            // 
            // YAxisLowestMSE
            // 
            this.YAxisLowestMSE.AutoSize = true;
            this.YAxisLowestMSE.Location = new System.Drawing.Point(157, 48);
            this.YAxisLowestMSE.Name = "YAxisLowestMSE";
            this.YAxisLowestMSE.Size = new System.Drawing.Size(106, 21);
            this.YAxisLowestMSE.TabIndex = 8;
            this.YAxisLowestMSE.TabStop = true;
            this.YAxisLowestMSE.Text = "Lowest MSE";
            this.YAxisLowestMSE.UseVisualStyleBackColor = true;
            // 
            // YAxisMSEDifference
            // 
            this.YAxisMSEDifference.AutoSize = true;
            this.YAxisMSEDifference.Location = new System.Drawing.Point(7, 48);
            this.YAxisMSEDifference.Name = "YAxisMSEDifference";
            this.YAxisMSEDifference.Size = new System.Drawing.Size(127, 21);
            this.YAxisMSEDifference.TabIndex = 7;
            this.YAxisMSEDifference.TabStop = true;
            this.YAxisMSEDifference.Text = "MSE Difference";
            this.YAxisMSEDifference.UseVisualStyleBackColor = true;
            // 
            // YAxisPercentileCorrect
            // 
            this.YAxisPercentileCorrect.AutoSize = true;
            this.YAxisPercentileCorrect.Location = new System.Drawing.Point(157, 21);
            this.YAxisPercentileCorrect.Name = "YAxisPercentileCorrect";
            this.YAxisPercentileCorrect.Size = new System.Drawing.Size(140, 21);
            this.YAxisPercentileCorrect.TabIndex = 6;
            this.YAxisPercentileCorrect.TabStop = true;
            this.YAxisPercentileCorrect.Text = "Percentile correct";
            this.YAxisPercentileCorrect.UseVisualStyleBackColor = true;
            // 
            // YAxisValidatedMSE
            // 
            this.YAxisValidatedMSE.AutoSize = true;
            this.YAxisValidatedMSE.Location = new System.Drawing.Point(7, 21);
            this.YAxisValidatedMSE.Name = "YAxisValidatedMSE";
            this.YAxisValidatedMSE.Size = new System.Drawing.Size(121, 21);
            this.YAxisValidatedMSE.TabIndex = 5;
            this.YAxisValidatedMSE.TabStop = true;
            this.YAxisValidatedMSE.Text = "Validated MSE";
            this.YAxisValidatedMSE.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 601);
            this.Controls.Add(this.YAxisValue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ExtractData);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PlotTitle);
            this.Controls.Add(this.PlotXAxisName);
            this.Controls.Add(this.PlotYAxisName);
            this.Controls.Add(this.GnuPlot);
            this.Controls.Add(this.ExtractionMode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NumberOfEpochsTrained);
            this.Controls.Add(this.FileBrowser);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ExtractionMode.ResumeLayout(false);
            this.ExtractionMode.PerformLayout();
            this.YAxisValue.ResumeLayout(false);
            this.YAxisValue.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button FileBrowser;
        private System.Windows.Forms.TextBox NumberOfEpochsTrained;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox ExtractionMode;
        private System.Windows.Forms.RadioButton radio_SteepnessOutput;
        private System.Windows.Forms.RadioButton radio_SteepnessHidden;
        private System.Windows.Forms.RadioButton radio_LearningRate;
        private System.Windows.Forms.RadioButton radio_HiddenLayers;
        private System.Windows.Forms.RadioButton radio_HiddenCells;
        private System.Windows.Forms.Button ExtractData;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button GnuPlot;
        private System.Windows.Forms.TextBox PlotYAxisName;
        private System.Windows.Forms.TextBox PlotXAxisName;
        private System.Windows.Forms.TextBox PlotTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox YAxisValue;
        private System.Windows.Forms.RadioButton YAxisPercentileCorrect;
        private System.Windows.Forms.RadioButton YAxisValidatedMSE;
        private System.Windows.Forms.RadioButton YAxisLowestMSE;
        private System.Windows.Forms.RadioButton YAxisMSEDifference;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton YAxisMSEValues;
        private System.Windows.Forms.RadioButton radio_MultipleNetworks;
        private System.Windows.Forms.RadioButton radio_EpochsTrained;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}

