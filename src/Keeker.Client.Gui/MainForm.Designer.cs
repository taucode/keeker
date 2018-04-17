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
            this.tabControlRequest = new System.Windows.Forms.TabControl();
            this.tabPagePrepare = new System.Windows.Forms.TabPage();
            this.comboBoxUri = new System.Windows.Forms.ComboBox();
            this.buttonDeleteHeader = new System.Windows.Forms.Button();
            this.buttonEditHeader = new System.Windows.Forms.Button();
            this.buttonAddHeader = new System.Windows.Forms.Button();
            this.listViewHeaders = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxMethod = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPageText = new System.Windows.Forms.TabPage();
            this.tabPageBinary = new System.Windows.Forms.TabPage();
            this.panelResponse = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRequestResponse)).BeginInit();
            this.splitContainerRequestResponse.Panel1.SuspendLayout();
            this.splitContainerRequestResponse.Panel2.SuspendLayout();
            this.splitContainerRequestResponse.SuspendLayout();
            this.panelRequest.SuspendLayout();
            this.tabControlRequest.SuspendLayout();
            this.tabPagePrepare.SuspendLayout();
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
            this.splitContainerRequestResponse.Size = new System.Drawing.Size(805, 480);
            this.splitContainerRequestResponse.SplitterDistance = 385;
            this.splitContainerRequestResponse.TabIndex = 4;
            // 
            // panelRequest
            // 
            this.panelRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRequest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRequest.Controls.Add(this.tabControlRequest);
            this.panelRequest.Location = new System.Drawing.Point(0, 0);
            this.panelRequest.Name = "panelRequest";
            this.panelRequest.Size = new System.Drawing.Size(385, 480);
            this.panelRequest.TabIndex = 0;
            // 
            // tabControlRequest
            // 
            this.tabControlRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlRequest.Controls.Add(this.tabPagePrepare);
            this.tabControlRequest.Controls.Add(this.tabPageText);
            this.tabControlRequest.Controls.Add(this.tabPageBinary);
            this.tabControlRequest.Location = new System.Drawing.Point(3, 3);
            this.tabControlRequest.Name = "tabControlRequest";
            this.tabControlRequest.SelectedIndex = 0;
            this.tabControlRequest.Size = new System.Drawing.Size(378, 473);
            this.tabControlRequest.TabIndex = 0;
            // 
            // tabPagePrepare
            // 
            this.tabPagePrepare.Controls.Add(this.comboBoxUri);
            this.tabPagePrepare.Controls.Add(this.buttonDeleteHeader);
            this.tabPagePrepare.Controls.Add(this.buttonEditHeader);
            this.tabPagePrepare.Controls.Add(this.buttonAddHeader);
            this.tabPagePrepare.Controls.Add(this.listViewHeaders);
            this.tabPagePrepare.Controls.Add(this.label4);
            this.tabPagePrepare.Controls.Add(this.label3);
            this.tabPagePrepare.Controls.Add(this.comboBoxMethod);
            this.tabPagePrepare.Controls.Add(this.label2);
            this.tabPagePrepare.Location = new System.Drawing.Point(4, 22);
            this.tabPagePrepare.Name = "tabPagePrepare";
            this.tabPagePrepare.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePrepare.Size = new System.Drawing.Size(370, 447);
            this.tabPagePrepare.TabIndex = 0;
            this.tabPagePrepare.Text = "Prepare";
            this.tabPagePrepare.UseVisualStyleBackColor = true;
            // 
            // comboBoxUri
            // 
            this.comboBoxUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxUri.FormattingEnabled = true;
            this.comboBoxUri.Location = new System.Drawing.Point(72, 33);
            this.comboBoxUri.Name = "comboBoxUri";
            this.comboBoxUri.Size = new System.Drawing.Size(292, 21);
            this.comboBoxUri.TabIndex = 9;
            this.comboBoxUri.TextChanged += new System.EventHandler(this.comboBoxUri_TextChanged);
            // 
            // buttonDeleteHeader
            // 
            this.buttonDeleteHeader.Location = new System.Drawing.Point(168, 257);
            this.buttonDeleteHeader.Name = "buttonDeleteHeader";
            this.buttonDeleteHeader.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteHeader.TabIndex = 8;
            this.buttonDeleteHeader.Text = "Delete";
            this.buttonDeleteHeader.UseVisualStyleBackColor = true;
            // 
            // buttonEditHeader
            // 
            this.buttonEditHeader.Location = new System.Drawing.Point(87, 257);
            this.buttonEditHeader.Name = "buttonEditHeader";
            this.buttonEditHeader.Size = new System.Drawing.Size(75, 23);
            this.buttonEditHeader.TabIndex = 7;
            this.buttonEditHeader.Text = "Edit...";
            this.buttonEditHeader.UseVisualStyleBackColor = true;
            // 
            // buttonAddHeader
            // 
            this.buttonAddHeader.Location = new System.Drawing.Point(6, 257);
            this.buttonAddHeader.Name = "buttonAddHeader";
            this.buttonAddHeader.Size = new System.Drawing.Size(75, 23);
            this.buttonAddHeader.TabIndex = 6;
            this.buttonAddHeader.Text = "Add...";
            this.buttonAddHeader.UseVisualStyleBackColor = true;
            // 
            // listViewHeaders
            // 
            this.listViewHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewHeaders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.listViewHeaders.FullRowSelect = true;
            this.listViewHeaders.GridLines = true;
            this.listViewHeaders.HideSelection = false;
            this.listViewHeaders.Location = new System.Drawing.Point(6, 78);
            this.listViewHeaders.Name = "listViewHeaders";
            this.listViewHeaders.Size = new System.Drawing.Size(358, 173);
            this.listViewHeaders.TabIndex = 5;
            this.listViewHeaders.UseCompatibleStateImageBehavior = false;
            this.listViewHeaders.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 114;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Value";
            this.columnHeader4.Width = 123;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Headers:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "URI:";
            // 
            // comboBoxMethod
            // 
            this.comboBoxMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMethod.FormattingEnabled = true;
            this.comboBoxMethod.Location = new System.Drawing.Point(72, 6);
            this.comboBoxMethod.Name = "comboBoxMethod";
            this.comboBoxMethod.Size = new System.Drawing.Size(121, 21);
            this.comboBoxMethod.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Method:";
            // 
            // tabPageText
            // 
            this.tabPageText.Location = new System.Drawing.Point(4, 22);
            this.tabPageText.Name = "tabPageText";
            this.tabPageText.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageText.Size = new System.Drawing.Size(370, 447);
            this.tabPageText.TabIndex = 1;
            this.tabPageText.Text = "Text";
            this.tabPageText.UseVisualStyleBackColor = true;
            // 
            // tabPageBinary
            // 
            this.tabPageBinary.Location = new System.Drawing.Point(4, 22);
            this.tabPageBinary.Name = "tabPageBinary";
            this.tabPageBinary.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBinary.Size = new System.Drawing.Size(370, 447);
            this.tabPageBinary.TabIndex = 2;
            this.tabPageBinary.Text = "Binary";
            this.tabPageBinary.UseVisualStyleBackColor = true;
            // 
            // panelResponse
            // 
            this.panelResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelResponse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelResponse.Location = new System.Drawing.Point(0, 0);
            this.panelResponse.Name = "panelResponse";
            this.panelResponse.Size = new System.Drawing.Size(416, 480);
            this.panelResponse.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 530);
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
            this.panelRequest.ResumeLayout(false);
            this.tabControlRequest.ResumeLayout(false);
            this.tabPagePrepare.ResumeLayout(false);
            this.tabPagePrepare.PerformLayout();
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
        private System.Windows.Forms.TabControl tabControlRequest;
        private System.Windows.Forms.TabPage tabPagePrepare;
        private System.Windows.Forms.TabPage tabPageText;
        private System.Windows.Forms.ComboBox comboBoxMethod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView listViewHeaders;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonDeleteHeader;
        private System.Windows.Forms.Button buttonEditHeader;
        private System.Windows.Forms.Button buttonAddHeader;
        private System.Windows.Forms.TabPage tabPageBinary;
        private System.Windows.Forms.ComboBox comboBoxUri;
    }
}

