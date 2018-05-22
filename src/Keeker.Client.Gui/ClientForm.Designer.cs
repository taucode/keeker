namespace Keeker.Client.Gui
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.splitContainerRequestResponse = new System.Windows.Forms.SplitContainer();
            this.panelRequest = new System.Windows.Forms.Panel();
            this.tabControlRequest = new System.Windows.Forms.TabControl();
            this.tabPagePrepare = new System.Windows.Forms.TabPage();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonRecent = new System.Windows.Forms.Button();
            this.buttonSaveAs = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
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
            this.textBoxRequestText = new System.Windows.Forms.TextBox();
            this.buttonSendText = new System.Windows.Forms.Button();
            this.buttonApplyText = new System.Windows.Forms.Button();
            this.tabPageBinary = new System.Windows.Forms.TabPage();
            this.panelResponse = new System.Windows.Forms.Panel();
            this.tabControlCommunication = new System.Windows.Forms.TabControl();
            this.tabPageByteStream = new System.Windows.Forms.TabPage();
            this.binaryViewPacket = new Keeker.UI.BinaryView();
            this.raceChartPackets = new Keeker.UI.RaceChart();
            this.tabPageDataStream = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRequestResponse)).BeginInit();
            this.splitContainerRequestResponse.Panel1.SuspendLayout();
            this.splitContainerRequestResponse.Panel2.SuspendLayout();
            this.splitContainerRequestResponse.SuspendLayout();
            this.panelRequest.SuspendLayout();
            this.tabControlRequest.SuspendLayout();
            this.tabPagePrepare.SuspendLayout();
            this.tabPageText.SuspendLayout();
            this.panelResponse.SuspendLayout();
            this.tabControlCommunication.SuspendLayout();
            this.tabPageByteStream.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerRequestResponse
            // 
            this.splitContainerRequestResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerRequestResponse.Location = new System.Drawing.Point(12, 12);
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
            this.splitContainerRequestResponse.Size = new System.Drawing.Size(1053, 506);
            this.splitContainerRequestResponse.SplitterDistance = 503;
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
            this.panelRequest.Size = new System.Drawing.Size(503, 506);
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
            this.tabControlRequest.Size = new System.Drawing.Size(496, 499);
            this.tabControlRequest.TabIndex = 0;
            // 
            // tabPagePrepare
            // 
            this.tabPagePrepare.Controls.Add(this.buttonApply);
            this.tabPagePrepare.Controls.Add(this.buttonRecent);
            this.tabPagePrepare.Controls.Add(this.buttonSaveAs);
            this.tabPagePrepare.Controls.Add(this.buttonSave);
            this.tabPagePrepare.Controls.Add(this.buttonOpen);
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
            this.tabPagePrepare.Size = new System.Drawing.Size(488, 473);
            this.tabPagePrepare.TabIndex = 0;
            this.tabPagePrepare.Text = "Prepare";
            this.tabPagePrepare.UseVisualStyleBackColor = true;
            // 
            // buttonApply
            // 
            this.buttonApply.Image = ((System.Drawing.Image)(resources.GetObject("buttonApply.Image")));
            this.buttonApply.Location = new System.Drawing.Point(199, 4);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(27, 25);
            this.buttonApply.TabIndex = 14;
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonRecent
            // 
            this.buttonRecent.Image = ((System.Drawing.Image)(resources.GetObject("buttonRecent.Image")));
            this.buttonRecent.Location = new System.Drawing.Point(331, 4);
            this.buttonRecent.Name = "buttonRecent";
            this.buttonRecent.Size = new System.Drawing.Size(27, 25);
            this.buttonRecent.TabIndex = 13;
            this.buttonRecent.UseVisualStyleBackColor = true;
            // 
            // buttonSaveAs
            // 
            this.buttonSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("buttonSaveAs.Image")));
            this.buttonSaveAs.Location = new System.Drawing.Point(265, 4);
            this.buttonSaveAs.Name = "buttonSaveAs";
            this.buttonSaveAs.Size = new System.Drawing.Size(27, 25);
            this.buttonSaveAs.TabIndex = 12;
            this.buttonSaveAs.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Image = ((System.Drawing.Image)(resources.GetObject("buttonSave.Image")));
            this.buttonSave.Location = new System.Drawing.Point(232, 4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(27, 25);
            this.buttonSave.TabIndex = 11;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Image = ((System.Drawing.Image)(resources.GetObject("buttonOpen.Image")));
            this.buttonOpen.Location = new System.Drawing.Point(298, 4);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(27, 25);
            this.buttonOpen.TabIndex = 10;
            this.buttonOpen.UseVisualStyleBackColor = true;
            // 
            // comboBoxUri
            // 
            this.comboBoxUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxUri.FormattingEnabled = true;
            this.comboBoxUri.Location = new System.Drawing.Point(72, 33);
            this.comboBoxUri.Name = "comboBoxUri";
            this.comboBoxUri.Size = new System.Drawing.Size(410, 21);
            this.comboBoxUri.TabIndex = 9;
            this.comboBoxUri.TextChanged += new System.EventHandler(this.comboBoxUri_TextChanged);
            // 
            // buttonDeleteHeader
            // 
            this.buttonDeleteHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteHeader.Location = new System.Drawing.Point(407, 136);
            this.buttonDeleteHeader.Name = "buttonDeleteHeader";
            this.buttonDeleteHeader.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteHeader.TabIndex = 8;
            this.buttonDeleteHeader.Text = "Delete";
            this.buttonDeleteHeader.UseVisualStyleBackColor = true;
            // 
            // buttonEditHeader
            // 
            this.buttonEditHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEditHeader.Location = new System.Drawing.Point(407, 107);
            this.buttonEditHeader.Name = "buttonEditHeader";
            this.buttonEditHeader.Size = new System.Drawing.Size(75, 23);
            this.buttonEditHeader.TabIndex = 7;
            this.buttonEditHeader.Text = "Edit...";
            this.buttonEditHeader.UseVisualStyleBackColor = true;
            // 
            // buttonAddHeader
            // 
            this.buttonAddHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddHeader.Location = new System.Drawing.Point(407, 78);
            this.buttonAddHeader.Name = "buttonAddHeader";
            this.buttonAddHeader.Size = new System.Drawing.Size(75, 23);
            this.buttonAddHeader.TabIndex = 6;
            this.buttonAddHeader.Text = "Add...";
            this.buttonAddHeader.UseVisualStyleBackColor = true;
            this.buttonAddHeader.Click += new System.EventHandler(this.buttonAddHeader_Click);
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
            this.listViewHeaders.Size = new System.Drawing.Size(395, 173);
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
            this.tabPageText.Controls.Add(this.textBoxRequestText);
            this.tabPageText.Controls.Add(this.buttonSendText);
            this.tabPageText.Controls.Add(this.buttonApplyText);
            this.tabPageText.Location = new System.Drawing.Point(4, 22);
            this.tabPageText.Name = "tabPageText";
            this.tabPageText.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageText.Size = new System.Drawing.Size(488, 473);
            this.tabPageText.TabIndex = 1;
            this.tabPageText.Text = "Text";
            this.tabPageText.UseVisualStyleBackColor = true;
            // 
            // textBoxRequestText
            // 
            this.textBoxRequestText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRequestText.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxRequestText.Location = new System.Drawing.Point(6, 35);
            this.textBoxRequestText.Multiline = true;
            this.textBoxRequestText.Name = "textBoxRequestText";
            this.textBoxRequestText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxRequestText.Size = new System.Drawing.Size(476, 432);
            this.textBoxRequestText.TabIndex = 2;
            this.textBoxRequestText.WordWrap = false;
            // 
            // buttonSendText
            // 
            this.buttonSendText.Location = new System.Drawing.Point(87, 6);
            this.buttonSendText.Name = "buttonSendText";
            this.buttonSendText.Size = new System.Drawing.Size(75, 23);
            this.buttonSendText.TabIndex = 1;
            this.buttonSendText.Text = "Send";
            this.buttonSendText.UseVisualStyleBackColor = true;
            this.buttonSendText.Click += new System.EventHandler(this.buttonSendText_Click);
            // 
            // buttonApplyText
            // 
            this.buttonApplyText.Location = new System.Drawing.Point(6, 6);
            this.buttonApplyText.Name = "buttonApplyText";
            this.buttonApplyText.Size = new System.Drawing.Size(75, 23);
            this.buttonApplyText.TabIndex = 0;
            this.buttonApplyText.Text = "Apply";
            this.buttonApplyText.UseVisualStyleBackColor = true;
            // 
            // tabPageBinary
            // 
            this.tabPageBinary.Location = new System.Drawing.Point(4, 22);
            this.tabPageBinary.Name = "tabPageBinary";
            this.tabPageBinary.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBinary.Size = new System.Drawing.Size(488, 473);
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
            this.panelResponse.Controls.Add(this.tabControlCommunication);
            this.panelResponse.Location = new System.Drawing.Point(0, 0);
            this.panelResponse.Name = "panelResponse";
            this.panelResponse.Size = new System.Drawing.Size(546, 506);
            this.panelResponse.TabIndex = 0;
            // 
            // tabControlCommunication
            // 
            this.tabControlCommunication.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlCommunication.Controls.Add(this.tabPageByteStream);
            this.tabControlCommunication.Controls.Add(this.tabPageDataStream);
            this.tabControlCommunication.Location = new System.Drawing.Point(3, 3);
            this.tabControlCommunication.Name = "tabControlCommunication";
            this.tabControlCommunication.SelectedIndex = 0;
            this.tabControlCommunication.Size = new System.Drawing.Size(538, 498);
            this.tabControlCommunication.TabIndex = 0;
            // 
            // tabPageByteStream
            // 
            this.tabPageByteStream.Controls.Add(this.binaryViewPacket);
            this.tabPageByteStream.Controls.Add(this.raceChartPackets);
            this.tabPageByteStream.Location = new System.Drawing.Point(4, 22);
            this.tabPageByteStream.Name = "tabPageByteStream";
            this.tabPageByteStream.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByteStream.Size = new System.Drawing.Size(530, 472);
            this.tabPageByteStream.TabIndex = 0;
            this.tabPageByteStream.Text = "Byte Stream";
            this.tabPageByteStream.UseVisualStyleBackColor = true;
            // 
            // binaryViewPacket
            // 
            this.binaryViewPacket.AddressMargin = 0;
            this.binaryViewPacket.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.binaryViewPacket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.binaryViewPacket.Bytes = new byte[0];
            this.binaryViewPacket.DumpByteMargin = 16;
            this.binaryViewPacket.HalfLineMargin = 16;
            this.binaryViewPacket.HexMargin = 8;
            this.binaryViewPacket.LineMargin = 16;
            this.binaryViewPacket.Location = new System.Drawing.Point(187, 6);
            this.binaryViewPacket.Name = "binaryViewPacket";
            this.binaryViewPacket.Size = new System.Drawing.Size(337, 460);
            this.binaryViewPacket.TabIndex = 1;
            // 
            // raceChartPackets
            // 
            this.raceChartPackets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.raceChartPackets.BackColor = System.Drawing.Color.White;
            this.raceChartPackets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.raceChartPackets.Location = new System.Drawing.Point(6, 6);
            this.raceChartPackets.Name = "raceChartPackets";
            this.raceChartPackets.Size = new System.Drawing.Size(175, 460);
            this.raceChartPackets.TabIndex = 0;
            this.raceChartPackets.ItemSelected += new Keeker.UI.ItemEventHandler(this.raceChartPackets_ItemSelected);
            this.raceChartPackets.ItemUnselected += new System.EventHandler(this.raceChartPackets_ItemUnselected);
            // 
            // tabPageDataStream
            // 
            this.tabPageDataStream.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataStream.Name = "tabPageDataStream";
            this.tabPageDataStream.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataStream.Size = new System.Drawing.Size(530, 472);
            this.tabPageDataStream.TabIndex = 1;
            this.tabPageDataStream.Text = "Data Stream";
            this.tabPageDataStream.UseVisualStyleBackColor = true;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 530);
            this.Controls.Add(this.splitContainerRequestResponse);
            this.Name = "ClientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client Main Form";
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.splitContainerRequestResponse.Panel1.ResumeLayout(false);
            this.splitContainerRequestResponse.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRequestResponse)).EndInit();
            this.splitContainerRequestResponse.ResumeLayout(false);
            this.panelRequest.ResumeLayout(false);
            this.tabControlRequest.ResumeLayout(false);
            this.tabPagePrepare.ResumeLayout(false);
            this.tabPagePrepare.PerformLayout();
            this.tabPageText.ResumeLayout(false);
            this.tabPageText.PerformLayout();
            this.panelResponse.ResumeLayout(false);
            this.tabControlCommunication.ResumeLayout(false);
            this.tabPageByteStream.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
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
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonSaveAs;
        private System.Windows.Forms.Button buttonRecent;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.TextBox textBoxRequestText;
        private System.Windows.Forms.Button buttonSendText;
        private System.Windows.Forms.Button buttonApplyText;
        private System.Windows.Forms.TabControl tabControlCommunication;
        private System.Windows.Forms.TabPage tabPageByteStream;
        private System.Windows.Forms.TabPage tabPageDataStream;
        private UI.BinaryView binaryViewPacket;
        private UI.RaceChart raceChartPackets;
    }
}

