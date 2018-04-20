namespace Keeker.Test.Gui
{
    partial class MainForm
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
            this.buttonFirstLog = new System.Windows.Forms.Button();
            this.buttonFirstComponent = new System.Windows.Forms.Button();
            this.buttonSecondComponent = new System.Windows.Forms.Button();
            this.buttonSecondLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonFirstLog
            // 
            this.buttonFirstLog.Location = new System.Drawing.Point(12, 12);
            this.buttonFirstLog.Name = "buttonFirstLog";
            this.buttonFirstLog.Size = new System.Drawing.Size(109, 23);
            this.buttonFirstLog.TabIndex = 0;
            this.buttonFirstLog.Text = "First Log";
            this.buttonFirstLog.UseVisualStyleBackColor = true;
            this.buttonFirstLog.Click += new System.EventHandler(this.buttonFirstLog_Click);
            // 
            // buttonFirstComponent
            // 
            this.buttonFirstComponent.Location = new System.Drawing.Point(12, 41);
            this.buttonFirstComponent.Name = "buttonFirstComponent";
            this.buttonFirstComponent.Size = new System.Drawing.Size(109, 23);
            this.buttonFirstComponent.TabIndex = 1;
            this.buttonFirstComponent.Text = "First Component";
            this.buttonFirstComponent.UseVisualStyleBackColor = true;
            this.buttonFirstComponent.Click += new System.EventHandler(this.buttonFirstComponent_Click);
            // 
            // buttonSecondComponent
            // 
            this.buttonSecondComponent.Location = new System.Drawing.Point(189, 41);
            this.buttonSecondComponent.Name = "buttonSecondComponent";
            this.buttonSecondComponent.Size = new System.Drawing.Size(109, 23);
            this.buttonSecondComponent.TabIndex = 3;
            this.buttonSecondComponent.Text = "Second Component";
            this.buttonSecondComponent.UseVisualStyleBackColor = true;
            this.buttonSecondComponent.Click += new System.EventHandler(this.buttonSecondComponent_Click);
            // 
            // buttonSecondLog
            // 
            this.buttonSecondLog.Location = new System.Drawing.Point(189, 12);
            this.buttonSecondLog.Name = "buttonSecondLog";
            this.buttonSecondLog.Size = new System.Drawing.Size(109, 23);
            this.buttonSecondLog.TabIndex = 2;
            this.buttonSecondLog.Text = "Second Log";
            this.buttonSecondLog.UseVisualStyleBackColor = true;
            this.buttonSecondLog.Click += new System.EventHandler(this.buttonSecondLog_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonSecondComponent);
            this.Controls.Add(this.buttonSecondLog);
            this.Controls.Add(this.buttonFirstComponent);
            this.Controls.Add(this.buttonFirstLog);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test Main Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonFirstLog;
        private System.Windows.Forms.Button buttonFirstComponent;
        private System.Windows.Forms.Button buttonSecondComponent;
        private System.Windows.Forms.Button buttonSecondLog;
    }
}

