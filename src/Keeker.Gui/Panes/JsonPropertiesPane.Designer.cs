namespace Keeker.Gui.Panes
{
    partial class JsonPropertiesPane
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
            this.textBoxJson = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxJson
            // 
            this.textBoxJson.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxJson.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxJson.Location = new System.Drawing.Point(3, 3);
            this.textBoxJson.Multiline = true;
            this.textBoxJson.Name = "textBoxJson";
            this.textBoxJson.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxJson.Size = new System.Drawing.Size(307, 246);
            this.textBoxJson.TabIndex = 0;
            this.textBoxJson.WordWrap = false;
            // 
            // JsonPropertiesPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxJson);
            this.Name = "JsonPropertiesPane";
            this.Load += new System.EventHandler(this.JsonPropertiesPane_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxJson;
    }
}
