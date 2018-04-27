namespace Keeker.Client.Gui
{
    partial class HttpServerForm
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
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonDispose = new System.Windows.Forms.Button();
            this.textBoxListening = new System.Windows.Forms.TextBox();
            this.textBoxHosts = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(12, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonDispose
            // 
            this.buttonDispose.Location = new System.Drawing.Point(93, 12);
            this.buttonDispose.Name = "buttonDispose";
            this.buttonDispose.Size = new System.Drawing.Size(75, 23);
            this.buttonDispose.TabIndex = 1;
            this.buttonDispose.Text = "Dispose";
            this.buttonDispose.UseVisualStyleBackColor = true;
            // 
            // textBoxListening
            // 
            this.textBoxListening.Location = new System.Drawing.Point(78, 41);
            this.textBoxListening.Name = "textBoxListening";
            this.textBoxListening.ReadOnly = true;
            this.textBoxListening.Size = new System.Drawing.Size(186, 20);
            this.textBoxListening.TabIndex = 2;
            // 
            // textBoxHosts
            // 
            this.textBoxHosts.Location = new System.Drawing.Point(78, 67);
            this.textBoxHosts.Name = "textBoxHosts";
            this.textBoxHosts.ReadOnly = true;
            this.textBoxHosts.Size = new System.Drawing.Size(186, 20);
            this.textBoxHosts.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Listening:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Hosts:";
            // 
            // HttpServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 416);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxHosts);
            this.Controls.Add(this.textBoxListening);
            this.Controls.Add(this.buttonDispose);
            this.Controls.Add(this.buttonStart);
            this.Name = "HttpServerForm";
            this.Text = "HttpServerForm";
            this.Load += new System.EventHandler(this.HttpServerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonDispose;
        private System.Windows.Forms.TextBox textBoxListening;
        private System.Windows.Forms.TextBox textBoxHosts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}