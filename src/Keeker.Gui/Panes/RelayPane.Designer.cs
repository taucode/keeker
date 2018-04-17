namespace Keeker.Gui.Panes
{
    partial class RelayPane
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.raceChartPackets = new Keeker.Gui.Controls.RaceChart();
            this.SuspendLayout();
            // 
            // raceChartPackets
            // 
            this.raceChartPackets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.raceChartPackets.BackColor = System.Drawing.Color.White;
            this.raceChartPackets.Location = new System.Drawing.Point(3, 3);
            this.raceChartPackets.Name = "raceChartPackets";
            this.raceChartPackets.Size = new System.Drawing.Size(192, 452);
            this.raceChartPackets.TabIndex = 0;
            // 
            // RelayPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.raceChartPackets);
            this.Name = "RelayPane";
            this.Size = new System.Drawing.Size(803, 458);
            this.Load += new System.EventHandler(this.RelayPane_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.RaceChart raceChartPackets;
    }
}
