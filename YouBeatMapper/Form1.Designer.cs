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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileDialogSongLoad = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel17 = new System.Windows.Forms.Panel();
            this.waveViewer1 = new NAudio.Gui.WaveViewer();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.b00 = new System.Windows.Forms.Button();
            this.b10 = new System.Windows.Forms.Button();
            this.b20 = new System.Windows.Forms.Button();
            this.b30 = new System.Windows.Forms.Button();
            this.b01 = new System.Windows.Forms.Button();
            this.b11 = new System.Windows.Forms.Button();
            this.b21 = new System.Windows.Forms.Button();
            this.b31 = new System.Windows.Forms.Button();
            this.b02 = new System.Windows.Forms.Button();
            this.b12 = new System.Windows.Forms.Button();
            this.b22 = new System.Windows.Forms.Button();
            this.b33 = new System.Windows.Forms.Button();
            this.b03 = new System.Windows.Forms.Button();
            this.b13 = new System.Windows.Forms.Button();
            this.b23 = new System.Windows.Forms.Button();
            this.b3 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
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
            // newSongToolStripMenuItem
            // 
            this.newSongToolStripMenuItem.Name = "newSongToolStripMenuItem";
            this.newSongToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.newSongToolStripMenuItem.Text = "New Song";
            // 
            // loadSongToolStripMenuItem
            // 
            this.loadSongToolStripMenuItem.Name = "loadSongToolStripMenuItem";
            this.loadSongToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.loadSongToolStripMenuItem.Text = "Load Song";
            this.loadSongToolStripMenuItem.Click += new System.EventHandler(this.loadSongToolStripMenuItem_Click);
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
            this.tableLayoutPanel1.Controls.Add(this.b3, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.b23, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.b13, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.b03, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.b33, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.b22, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.b12, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.b02, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.b31, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.b21, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.b11, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.b01, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.b30, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.b20, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.b10, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.b00, 0, 0);
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
            // panel17
            // 
            this.panel17.BackColor = System.Drawing.SystemColors.Control;
            this.panel17.Controls.Add(this.trackBar1);
            this.panel17.Controls.Add(this.waveViewer1);
            this.panel17.Controls.Add(this.buttonStop);
            this.panel17.Controls.Add(this.buttonPlay);
            this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel17.Location = new System.Drawing.Point(0, 24);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(1243, 64);
            this.panel17.TabIndex = 2;
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
            // b00
            // 
            this.b00.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b00.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b00.Location = new System.Drawing.Point(3, 3);
            this.b00.Name = "b00";
            this.b00.Size = new System.Drawing.Size(136, 136);
            this.b00.TabIndex = 0;
            this.b00.UseVisualStyleBackColor = false;
            // 
            // b10
            // 
            this.b10.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b10.Location = new System.Drawing.Point(145, 3);
            this.b10.Name = "b10";
            this.b10.Size = new System.Drawing.Size(136, 136);
            this.b10.TabIndex = 1;
            this.b10.UseVisualStyleBackColor = false;
            // 
            // b20
            // 
            this.b20.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b20.Location = new System.Drawing.Point(287, 3);
            this.b20.Name = "b20";
            this.b20.Size = new System.Drawing.Size(136, 136);
            this.b20.TabIndex = 2;
            this.b20.UseVisualStyleBackColor = false;
            // 
            // b30
            // 
            this.b30.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b30.Location = new System.Drawing.Point(429, 3);
            this.b30.Name = "b30";
            this.b30.Size = new System.Drawing.Size(137, 136);
            this.b30.TabIndex = 3;
            this.b30.UseVisualStyleBackColor = false;
            // 
            // b01
            // 
            this.b01.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b01.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b01.Location = new System.Drawing.Point(3, 145);
            this.b01.Name = "b01";
            this.b01.Size = new System.Drawing.Size(136, 136);
            this.b01.TabIndex = 4;
            this.b01.UseVisualStyleBackColor = false;
            // 
            // b11
            // 
            this.b11.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b11.Location = new System.Drawing.Point(145, 145);
            this.b11.Name = "b11";
            this.b11.Size = new System.Drawing.Size(136, 136);
            this.b11.TabIndex = 5;
            this.b11.UseVisualStyleBackColor = false;
            // 
            // b21
            // 
            this.b21.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b21.Location = new System.Drawing.Point(287, 145);
            this.b21.Name = "b21";
            this.b21.Size = new System.Drawing.Size(136, 136);
            this.b21.TabIndex = 6;
            this.b21.UseVisualStyleBackColor = false;
            // 
            // b31
            // 
            this.b31.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b31.Location = new System.Drawing.Point(429, 145);
            this.b31.Name = "b31";
            this.b31.Size = new System.Drawing.Size(137, 136);
            this.b31.TabIndex = 7;
            this.b31.UseVisualStyleBackColor = false;
            // 
            // b02
            // 
            this.b02.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b02.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b02.Location = new System.Drawing.Point(3, 287);
            this.b02.Name = "b02";
            this.b02.Size = new System.Drawing.Size(136, 136);
            this.b02.TabIndex = 8;
            this.b02.UseVisualStyleBackColor = false;
            // 
            // b12
            // 
            this.b12.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b12.Location = new System.Drawing.Point(145, 287);
            this.b12.Name = "b12";
            this.b12.Size = new System.Drawing.Size(136, 136);
            this.b12.TabIndex = 9;
            this.b12.UseVisualStyleBackColor = false;
            // 
            // b22
            // 
            this.b22.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b22.Location = new System.Drawing.Point(287, 287);
            this.b22.Name = "b22";
            this.b22.Size = new System.Drawing.Size(136, 136);
            this.b22.TabIndex = 10;
            this.b22.UseVisualStyleBackColor = false;
            // 
            // b33
            // 
            this.b33.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b33.Location = new System.Drawing.Point(429, 287);
            this.b33.Name = "b33";
            this.b33.Size = new System.Drawing.Size(137, 136);
            this.b33.TabIndex = 11;
            this.b33.UseVisualStyleBackColor = false;
            // 
            // b03
            // 
            this.b03.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b03.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b03.Location = new System.Drawing.Point(3, 429);
            this.b03.Name = "b03";
            this.b03.Size = new System.Drawing.Size(136, 137);
            this.b03.TabIndex = 12;
            this.b03.UseVisualStyleBackColor = false;
            // 
            // b13
            // 
            this.b13.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b13.Location = new System.Drawing.Point(145, 429);
            this.b13.Name = "b13";
            this.b13.Size = new System.Drawing.Size(136, 137);
            this.b13.TabIndex = 13;
            this.b13.UseVisualStyleBackColor = false;
            // 
            // b23
            // 
            this.b23.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b23.Location = new System.Drawing.Point(287, 429);
            this.b23.Name = "b23";
            this.b23.Size = new System.Drawing.Size(136, 137);
            this.b23.TabIndex = 14;
            this.b23.UseVisualStyleBackColor = false;
            // 
            // b3
            // 
            this.b3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b3.Location = new System.Drawing.Point(429, 429);
            this.b3.Name = "b3";
            this.b3.Size = new System.Drawing.Size(137, 137);
            this.b3.TabIndex = 15;
            this.b3.UseVisualStyleBackColor = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(134, 3);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(1097, 45);
            this.trackBar1.TabIndex = 3;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
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
            this.panel17.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
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
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonPlay;
        private NAudio.Gui.WaveViewer waveViewer1;
        private System.Windows.Forms.Button b3;
        private System.Windows.Forms.Button b23;
        private System.Windows.Forms.Button b13;
        private System.Windows.Forms.Button b03;
        private System.Windows.Forms.Button b33;
        private System.Windows.Forms.Button b22;
        private System.Windows.Forms.Button b12;
        private System.Windows.Forms.Button b02;
        private System.Windows.Forms.Button b31;
        private System.Windows.Forms.Button b21;
        private System.Windows.Forms.Button b11;
        private System.Windows.Forms.Button b01;
        private System.Windows.Forms.Button b30;
        private System.Windows.Forms.Button b20;
        private System.Windows.Forms.Button b10;
        private System.Windows.Forms.Button b00;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TrackBar trackBar1;
    }
}

