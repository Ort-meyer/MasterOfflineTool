namespace Questionnaire
{
    partial class Combine_answers
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
            this.combine = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // combine
            // 
            this.combine.Location = new System.Drawing.Point(463, 553);
            this.combine.Name = "combine";
            this.combine.Size = new System.Drawing.Size(75, 23);
            this.combine.TabIndex = 0;
            this.combine.Text = "Combine data";
            this.combine.UseVisualStyleBackColor = true;
            this.combine.Click += new System.EventHandler(this.combine_Click);
            // 
            // Combine_answers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 588);
            this.Controls.Add(this.combine);
            this.Name = "Combine_answers";
            this.Text = "Combine_answers";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button combine;
    }
}