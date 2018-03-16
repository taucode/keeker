namespace Keeker.Gui
{
    partial class RaceChartForm
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
            this.raceChart1 = new Keeker.Gui.Controls.RaceChart();
            this.SuspendLayout();
            // 
            // raceChart1
            // 
            this.raceChart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.raceChart1.BackColor = System.Drawing.Color.White;
            this.raceChart1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.raceChart1.Location = new System.Drawing.Point(12, 12);
            this.raceChart1.Name = "raceChart1";
            this.raceChart1.Size = new System.Drawing.Size(250, 222);
            this.raceChart1.TabIndex = 2;
            // 
            // RaceChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 246);
            this.Controls.Add(this.raceChart1);
            this.Name = "RaceChartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RaceChartForm";
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.RaceChart raceChart1;
    }
}