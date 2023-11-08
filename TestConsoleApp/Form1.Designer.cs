namespace Invertex
{
    partial class IceStreamForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IceStreamForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.logBox = new System.Windows.Forms.TextBox();
            this.streamUrlBox = new System.Windows.Forms.TextBox();
            this.streamUrlLabel = new System.Windows.Forms.Label();
            this.logBoxLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.startButton = new System.Windows.Forms.Button();
            this.filterWordsInput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.saveLocationInput = new System.Windows.Forms.TextBox();
            this.reconnectAttempts = new System.Windows.Forms.NumericUpDown();
            this.reconnectLabel = new System.Windows.Forms.Label();
            this.saveCurrentlyPlayingBtn = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reconnectAttempts)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 444);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(806, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "Waiting to Start...";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(118, 17);
            this.statusLabel.Text = "toolStripStatusLabel1";
            // 
            // logBox
            // 
            this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logBox.Location = new System.Drawing.Point(18, 284);
            this.logBox.MaxLength = 999999;
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(779, 152);
            this.logBox.TabIndex = 3;
            // 
            // streamUrlBox
            // 
            this.streamUrlBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.streamUrlBox.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.streamUrlBox.Location = new System.Drawing.Point(126, 18);
            this.streamUrlBox.Name = "streamUrlBox";
            this.streamUrlBox.PlaceholderText = "Enter Stream Url...";
            this.streamUrlBox.Size = new System.Drawing.Size(664, 27);
            this.streamUrlBox.TabIndex = 4;
            this.streamUrlBox.Tag = "lastPathData";
            // 
            // streamUrlLabel
            // 
            this.streamUrlLabel.AutoSize = true;
            this.streamUrlLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.streamUrlLabel.Location = new System.Drawing.Point(18, 20);
            this.streamUrlLabel.Name = "streamUrlLabel";
            this.streamUrlLabel.Size = new System.Drawing.Size(102, 21);
            this.streamUrlLabel.TabIndex = 5;
            this.streamUrlLabel.Text = "Stream URL:";
            // 
            // logBoxLabel
            // 
            this.logBoxLabel.AutoSize = true;
            this.logBoxLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.logBoxLabel.Location = new System.Drawing.Point(18, 250);
            this.logBoxLabel.Name = "logBoxLabel";
            this.logBoxLabel.Size = new System.Drawing.Size(42, 21);
            this.logBoxLabel.TabIndex = 6;
            this.logBoxLabel.Text = "Log:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(18, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 21);
            this.label2.TabIndex = 8;
            this.label2.Text = "Save Location:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(144, 51);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 29);
            this.button1.TabIndex = 9;
            this.button1.Text = "BROWSE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Browse_Clicked);
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.startButton.Location = new System.Drawing.Point(680, 250);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(117, 28);
            this.startButton.TabIndex = 11;
            this.startButton.Text = "START";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.Start_Clicked);
            // 
            // filterWordsInput
            // 
            this.filterWordsInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterWordsInput.Location = new System.Drawing.Point(18, 107);
            this.filterWordsInput.Multiline = true;
            this.filterWordsInput.Name = "filterWordsInput";
            this.filterWordsInput.PlaceholderText = "Saving all songs while no filters are set...";
            this.filterWordsInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.filterWordsInput.Size = new System.Drawing.Size(470, 140);
            this.filterWordsInput.TabIndex = 12;
            this.filterWordsInput.TextChanged += new System.EventHandler(this.FilterText_Changed);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(18, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(363, 21);
            this.label3.TabIndex = 13;
            this.label3.Text = "Only Save Songs With Any Of These Keywords:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(377, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "(new line each)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // saveLocationInput
            // 
            this.saveLocationInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveLocationInput.Enabled = false;
            this.saveLocationInput.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.saveLocationInput.Location = new System.Drawing.Point(231, 53);
            this.saveLocationInput.Name = "saveLocationInput";
            this.saveLocationInput.PlaceholderText = "Choose a save directory...";
            this.saveLocationInput.ReadOnly = true;
            this.saveLocationInput.Size = new System.Drawing.Size(559, 27);
            this.saveLocationInput.TabIndex = 16;
            this.saveLocationInput.WordWrap = false;
            // 
            // reconnectAttempts
            // 
            this.reconnectAttempts.Location = new System.Drawing.Point(613, 253);
            this.reconnectAttempts.Name = "reconnectAttempts";
            this.reconnectAttempts.Size = new System.Drawing.Size(61, 23);
            this.reconnectAttempts.TabIndex = 17;
            this.reconnectAttempts.Tag = "ReconnectMax";
            this.reconnectAttempts.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // reconnectLabel
            // 
            this.reconnectLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reconnectLabel.AutoSize = true;
            this.reconnectLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.reconnectLabel.Location = new System.Drawing.Point(494, 256);
            this.reconnectLabel.Name = "reconnectLabel";
            this.reconnectLabel.Size = new System.Drawing.Size(113, 15);
            this.reconnectLabel.TabIndex = 18;
            this.reconnectLabel.Text = "Reconnect attempts:";
            this.reconnectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // saveCurrentlyPlayingBtn
            // 
            this.saveCurrentlyPlayingBtn.Location = new System.Drawing.Point(495, 107);
            this.saveCurrentlyPlayingBtn.Name = "saveCurrentlyPlayingBtn";
            this.saveCurrentlyPlayingBtn.Size = new System.Drawing.Size(295, 42);
            this.saveCurrentlyPlayingBtn.TabIndex = 19;
            this.saveCurrentlyPlayingBtn.Text = "Save Currently Playing Song";
            this.saveCurrentlyPlayingBtn.UseVisualStyleBackColor = true;
            this.saveCurrentlyPlayingBtn.Click += new System.EventHandler(this.SaveCurrentlyPlaying_Clicked);
            // 
            // IceStreamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 466);
            this.Controls.Add(this.saveCurrentlyPlayingBtn);
            this.Controls.Add(this.reconnectLabel);
            this.Controls.Add(this.reconnectAttempts);
            this.Controls.Add(this.saveLocationInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.filterWordsInput);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.logBoxLabel);
            this.Controls.Add(this.streamUrlLabel);
            this.Controls.Add(this.streamUrlBox);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(822, 505);
            this.MinimumSize = new System.Drawing.Size(822, 505);
            this.Name = "IceStreamForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "IceStream Ripper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reconnectAttempts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.TextBox streamUrlBox;
        private System.Windows.Forms.Label streamUrlLabel;
        private System.Windows.Forms.Label logBoxLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox filterWordsInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox saveLocationInput;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.NumericUpDown reconnectAttempts;
        private System.Windows.Forms.Label reconnectLabel;
        private System.Windows.Forms.Button saveCurrentlyPlayingBtn;
    }
}