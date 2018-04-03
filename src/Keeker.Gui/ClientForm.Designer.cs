namespace Keeker.Gui
{
    partial class ClientForm
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
            this.buttonSample1 = new System.Windows.Forms.Button();
            this.textBoxResponse = new System.Windows.Forms.TextBox();
            this.buttonSample2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSample1
            // 
            this.buttonSample1.Location = new System.Drawing.Point(12, 12);
            this.buttonSample1.Name = "buttonSample1";
            this.buttonSample1.Size = new System.Drawing.Size(75, 23);
            this.buttonSample1.TabIndex = 0;
            this.buttonSample1.Text = "Sample 1";
            this.buttonSample1.UseVisualStyleBackColor = true;
            this.buttonSample1.Click += new System.EventHandler(this.buttonSample1_Click);
            // 
            // textBoxResponse
            // 
            this.textBoxResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResponse.Location = new System.Drawing.Point(93, 12);
            this.textBoxResponse.Multiline = true;
            this.textBoxResponse.Name = "textBoxResponse";
            this.textBoxResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxResponse.Size = new System.Drawing.Size(429, 325);
            this.textBoxResponse.TabIndex = 1;
            this.textBoxResponse.WordWrap = false;
            // 
            // buttonSample2
            // 
            this.buttonSample2.Location = new System.Drawing.Point(12, 41);
            this.buttonSample2.Name = "buttonSample2";
            this.buttonSample2.Size = new System.Drawing.Size(75, 23);
            this.buttonSample2.TabIndex = 2;
            this.buttonSample2.Text = "Sample 2";
            this.buttonSample2.UseVisualStyleBackColor = true;
            this.buttonSample2.Click += new System.EventHandler(this.buttonSample2_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 349);
            this.Controls.Add(this.buttonSample2);
            this.Controls.Add(this.textBoxResponse);
            this.Controls.Add(this.buttonSample1);
            this.Name = "ClientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ClientForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSample1;
        private System.Windows.Forms.TextBox textBoxResponse;
        private System.Windows.Forms.Button buttonSample2;
    }
}