namespace YouBeatMapper {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileDialogSongLoad = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.panel15 = new System.Windows.Forms.Panel();
            this.panel16 = new System.Windows.Forms.Panel();
            this.panel17 = new System.Windows.Forms.Panel();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.waveViewer1 = new NAudio.Gui.WaveViewer();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel17.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1243, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSongToolStripMenuItem,
            this.loadSongToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadSongToolStripMenuItem
            // 
            this.loadSongToolStripMenuItem.Name = "loadSongToolStripMenuItem";
            this.loadSongToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadSongToolStripMenuItem.Text = "Load Song";
            this.loadSongToolStripMenuItem.Click += new System.EventHandler(this.loadSongToolStripMenuItem_Click);
            // 
            // newSongToolStripMenuItem
            // 
            this.newSongToolStripMenuItem.Name = "newSongToolStripMenuItem";
            this.newSongToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newSongToolStripMenuItem.Text = "New Song";
            // 
            // fileDialogSongLoad
            // 
            this.fileDialogSongLoad.Filter = "Track Files|*.trk|All files|*.*";
            this.fileDialogSongLoad.Title = "Load Song";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.panel16, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel15, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel14, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel13, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel12, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel11, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel10, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel8, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel7, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel6, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(352, 102);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(569, 569);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(136, 136);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(145, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(136, 136);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(287, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(136, 136);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(429, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(137, 136);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 145);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(136, 136);
            this.panel5.TabIndex = 4;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(145, 145);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(136, 136);
            this.panel6.TabIndex = 5;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(287, 145);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(136, 136);
            this.panel7.TabIndex = 6;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(429, 145);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(137, 136);
            this.panel8.TabIndex = 7;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 287);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(136, 136);
            this.panel9.TabIndex = 8;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(145, 287);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(136, 136);
            this.panel10.TabIndex = 9;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(287, 287);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(136, 136);
            this.panel11.TabIndex = 10;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(429, 287);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(137, 136);
            this.panel12.TabIndex = 11;
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel13.Location = new System.Drawing.Point(3, 429);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(136, 137);
            this.panel13.TabIndex = 12;
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel14.Location = new System.Drawing.Point(145, 429);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(136, 137);
            this.panel14.TabIndex = 13;
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel15.Location = new System.Drawing.Point(287, 429);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(136, 137);
            this.panel15.TabIndex = 14;
            // 
            // panel16
            // 
            this.panel16.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel16.Location = new System.Drawing.Point(429, 429);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(137, 137);
            this.panel16.TabIndex = 15;
            // 
            // panel17
            // 
            this.panel17.BackColor = System.Drawing.SystemColors.Control;
            this.panel17.Controls.Add(this.waveViewer1);
            this.panel17.Controls.Add(this.buttonStop);
            this.panel17.Controls.Add(this.buttonPlay);
            this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel17.Location = new System.Drawing.Point(0, 24);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(1243, 64);
            this.panel17.TabIndex = 2;
            // 
            // buttonPlay
            // 
            this.buttonPlay.BackColor = System.Drawing.SystemColors.Control;
            this.buttonPlay.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonPlay.Location = new System.Drawing.Point(0, 0);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(64, 64);
            this.buttonPlay.TabIndex = 0;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = false;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.SystemColors.Control;
            this.buttonStop.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonStop.Location = new System.Drawing.Point(64, 0);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(64, 64);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // waveViewer1
            // 
            this.waveViewer1.Dock = System.Windows.Forms.DockStyle.Left;
            this.waveViewer1.Location = new System.Drawing.Point(128, 0);
            this.waveViewer1.Name = "waveViewer1";
            this.waveViewer1.SamplesPerPixel = 128;
            this.waveViewer1.Size = new System.Drawing.Size(1097, 64);
            this.waveViewer1.StartPosition = ((long)(0));
            this.waveViewer1.TabIndex = 2;
            this.waveViewer1.WaveStream = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1243, 683);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel17);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel17.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSongToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSongToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog fileDialogSongLoad;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonPlay;
        private NAudio.Gui.WaveViewer waveViewer1;
    }
}

