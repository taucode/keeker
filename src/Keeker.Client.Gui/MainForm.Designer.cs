namespace Keeker.Client.Gui
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
            this.textBoxEndPoint = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewConnections = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonConnect = new System.Windows.Forms.Button();
            this.splitContainerRequestResponse = new System.Windows.Forms.SplitContainer();
            this.panelRequest = new System.Windows.Forms.Panel();
            this.panelResponse = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRequestResponse)).BeginInit();
            this.splitContainerRequestResponse.Panel1.SuspendLayout();
            this.splitContainerRequestResponse.Panel2.SuspendLayout();
            this.splitContainerRequestResponse.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxEndPoint
            // 
            this.textBoxEndPoint.Location = new System.Drawing.Point(73, 12);
            this.textBoxEndPoint.Name = "textBoxEndPoint";
            this.textBoxEndPoint.Size = new System.Drawing.Size(100, 20);
            this.textBoxEndPoint.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "End Point:";
            // 
            // listViewConnections
            // 
            this.listViewConnections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewConnections.FullRowSelect = true;
            this.listViewConnections.GridLines = true;
            this.listViewConnections.HideSelection = false;
            this.listViewConnections.Location = new System.Drawing.Point(12, 38);
            this.listViewConnections.Name = "listViewConnections";
            this.listViewConnections.Size = new System.Drawing.Size(242, 480);
            this.listViewConnections.TabIndex = 2;
            this.listViewConnections.UseCompatibleStateImageBehavior = false;
            this.listViewConnections.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Local EP";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(179, 10);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 3;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            // 
            // splitContainerRequestResponse
            // 
            this.splitContainerRequestResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerRequestResponse.Location = new System.Drawing.Point(260, 38);
            this.splitContainerRequestResponse.Name = "splitContainerRequestResponse";
            // 
            // splitContainerRequestResponse.Panel1
            // 
            this.splitContainerRequestResponse.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerRequestResponse.Panel1.Controls.Add(this.panelRequest);
            // 
            // splitContainerRequestResponse.Panel2
            // 
            this.splitContainerRequestResponse.Panel2.Controls.Add(this.panelResponse);
            this.splitContainerRequestResponse.Size = new System.Drawing.Size(622, 480);
            this.splitContainerRequestResponse.SplitterDistance = 298;
            this.splitContainerRequestResponse.TabIndex = 4;
            // 
            // panelRequest
            // 
            this.panelRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRequest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRequest.Location = new System.Drawing.Point(0, 0);
            this.panelRequest.Name = "panelRequest";
            this.panelRequest.Size = new System.Drawing.Size(298, 480);
            this.panelRequest.TabIndex = 0;
            // 
            // panelResponse
            // 
            this.panelResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelResponse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelResponse.Location = new System.Drawing.Point(0, 0);
            this.panelResponse.Name = "panelResponse";
            this.panelResponse.Size = new System.Drawing.Size(320, 480);
            this.panelResponse.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 530);
            this.Controls.Add(this.splitContainerRequestResponse);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.listViewConnections);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxEndPoint);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client Main Form";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainerRequestResponse.Panel1.ResumeLayout(false);
            this.splitContainerRequestResponse.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRequestResponse)).EndInit();
            this.splitContainerRequestResponse.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxEndPoint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listViewConnections;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.SplitContainer splitContainerRequestResponse;
        private System.Windows.Forms.Panel panelRequest;
        private System.Windows.Forms.Panel panelResponse;
    }
}

