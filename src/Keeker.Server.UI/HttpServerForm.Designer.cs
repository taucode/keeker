namespace Keeker.Server.UI
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
            this.textBoxEndPoint = new System.Windows.Forms.TextBox();
            this.listViewConnections = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControlCommunication = new System.Windows.Forms.TabControl();
            this.tabPageByteStream = new System.Windows.Forms.TabPage();
            this.tabPageDataStream = new System.Windows.Forms.TabPage();
            this.raceChartPackets = new Keeker.UI.RaceChart();
            this.binaryViewPacket = new Keeker.UI.BinaryView();
            this.tabControlCommunication.SuspendLayout();
            this.tabPageByteStream.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(143, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonDispose
            // 
            this.buttonDispose.Location = new System.Drawing.Point(224, 12);
            this.buttonDispose.Name = "buttonDispose";
            this.buttonDispose.Size = new System.Drawing.Size(75, 23);
            this.buttonDispose.TabIndex = 1;
            this.buttonDispose.Text = "Dispose";
            this.buttonDispose.UseVisualStyleBackColor = true;
            this.buttonDispose.Click += new System.EventHandler(this.buttonDispose_Click);
            // 
            // textBoxEndPoint
            // 
            this.textBoxEndPoint.Location = new System.Drawing.Point(12, 14);
            this.textBoxEndPoint.Name = "textBoxEndPoint";
            this.textBoxEndPoint.ReadOnly = true;
            this.textBoxEndPoint.Size = new System.Drawing.Size(125, 20);
            this.textBoxEndPoint.TabIndex = 2;
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
            this.listViewConnections.Location = new System.Drawing.Point(12, 40);
            this.listViewConnections.Name = "listViewConnections";
            this.listViewConnections.Size = new System.Drawing.Size(287, 252);
            this.listViewConnections.TabIndex = 3;
            this.listViewConnections.UseCompatibleStateImageBehavior = false;
            this.listViewConnections.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "End Point";
            // 
            // tabControlCommunication
            // 
            this.tabControlCommunication.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlCommunication.Controls.Add(this.tabPageByteStream);
            this.tabControlCommunication.Controls.Add(this.tabPageDataStream);
            this.tabControlCommunication.Location = new System.Drawing.Point(305, 14);
            this.tabControlCommunication.Name = "tabControlCommunication";
            this.tabControlCommunication.SelectedIndex = 0;
            this.tabControlCommunication.Size = new System.Drawing.Size(417, 278);
            this.tabControlCommunication.TabIndex = 4;
            // 
            // tabPageByteStream
            // 
            this.tabPageByteStream.Controls.Add(this.binaryViewPacket);
            this.tabPageByteStream.Controls.Add(this.raceChartPackets);
            this.tabPageByteStream.Location = new System.Drawing.Point(4, 22);
            this.tabPageByteStream.Name = "tabPageByteStream";
            this.tabPageByteStream.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByteStream.Size = new System.Drawing.Size(409, 252);
            this.tabPageByteStream.TabIndex = 0;
            this.tabPageByteStream.Text = "Byte Stream";
            this.tabPageByteStream.UseVisualStyleBackColor = true;
            // 
            // tabPageDataStream
            // 
            this.tabPageDataStream.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataStream.Name = "tabPageDataStream";
            this.tabPageDataStream.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataStream.Size = new System.Drawing.Size(409, 252);
            this.tabPageDataStream.TabIndex = 1;
            this.tabPageDataStream.Text = "Data Stream";
            this.tabPageDataStream.UseVisualStyleBackColor = true;
            // 
            // raceChartPackets
            // 
            this.raceChartPackets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.raceChartPackets.BackColor = System.Drawing.Color.White;
            this.raceChartPackets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.raceChartPackets.Location = new System.Drawing.Point(6, 6);
            this.raceChartPackets.Name = "raceChartPackets";
            this.raceChartPackets.Size = new System.Drawing.Size(175, 240);
            this.raceChartPackets.TabIndex = 0;
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
            this.binaryViewPacket.Size = new System.Drawing.Size(216, 240);
            this.binaryViewPacket.TabIndex = 1;
            // 
            // HttpServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 304);
            this.Controls.Add(this.tabControlCommunication);
            this.Controls.Add(this.listViewConnections);
            this.Controls.Add(this.textBoxEndPoint);
            this.Controls.Add(this.buttonDispose);
            this.Controls.Add(this.buttonStart);
            this.Name = "HttpServerForm";
            this.Text = "HttpServerForm";
            this.Load += new System.EventHandler(this.HttpServerForm_Load);
            this.tabControlCommunication.ResumeLayout(false);
            this.tabPageByteStream.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonDispose;
        private System.Windows.Forms.TextBox textBoxEndPoint;
        private System.Windows.Forms.ListView listViewConnections;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TabControl tabControlCommunication;
        private System.Windows.Forms.TabPage tabPageByteStream;
        private System.Windows.Forms.TabPage tabPageDataStream;
        private Keeker.UI.RaceChart raceChartPackets;
        private Keeker.UI.BinaryView binaryViewPacket;
    }
}