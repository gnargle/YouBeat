namespace YouBeatMapper {
    partial class Mapper {
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
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileDialogSongLoad = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.b33 = new System.Windows.Forms.Button();
            this.b23 = new System.Windows.Forms.Button();
            this.b13 = new System.Windows.Forms.Button();
            this.b03 = new System.Windows.Forms.Button();
            this.b32 = new System.Windows.Forms.Button();
            this.b22 = new System.Windows.Forms.Button();
            this.b12 = new System.Windows.Forms.Button();
            this.b02 = new System.Windows.Forms.Button();
            this.b31 = new System.Windows.Forms.Button();
            this.b21 = new System.Windows.Forms.Button();
            this.b11 = new System.Windows.Forms.Button();
            this.b01 = new System.Windows.Forms.Button();
            this.b30 = new System.Windows.Forms.Button();
            this.b20 = new System.Windows.Forms.Button();
            this.b10 = new System.Windows.Forms.Button();
            this.b00 = new System.Windows.Forms.Button();
            this.panel17 = new System.Windows.Forms.Panel();
            this.tbMusic = new System.Windows.Forms.TrackBar();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.fileDialogNewSong = new System.Windows.Forms.OpenFileDialog();
            this.pgCurrentSong = new System.Windows.Forms.PropertyGrid();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.flpSettings1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flpOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.lDifficulty = new System.Windows.Forms.Label();
            this.cbDifficulty = new System.Windows.Forms.ComboBox();
            this.flpSong = new System.Windows.Forms.FlowLayoutPanel();
            this.lSongProp = new System.Windows.Forms.Label();
            this.flpSettings2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flpBeat = new System.Windows.Forms.FlowLayoutPanel();
            this.lBeatProp = new System.Windows.Forms.Label();
            this.pgCurrentBeat = new System.Windows.Forms.PropertyGrid();
            this.bDeleteBeat = new System.Windows.Forms.Button();
            this.lTicks = new System.Windows.Forms.Label();
            this.lTimeUpdater = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbMusic)).BeginInit();
            this.flpSettings1.SuspendLayout();
            this.flpOptions.SuspendLayout();
            this.flpSong.SuspendLayout();
            this.flpSettings2.SuspendLayout();
            this.flpBeat.SuspendLayout();
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
            this.loadSongToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newSongToolStripMenuItem
            // 
            this.newSongToolStripMenuItem.Name = "newSongToolStripMenuItem";
            this.newSongToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newSongToolStripMenuItem.Text = "New Song";
            this.newSongToolStripMenuItem.Click += new System.EventHandler(this.NewSongToolStripMenuItem_Click);
            // 
            // loadSongToolStripMenuItem
            // 
            this.loadSongToolStripMenuItem.Name = "loadSongToolStripMenuItem";
            this.loadSongToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadSongToolStripMenuItem.Text = "Load Song";
            this.loadSongToolStripMenuItem.Click += new System.EventHandler(this.LoadSongToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // fileDialogSongLoad
            // 
            this.fileDialogSongLoad.Filter = "Track Files|*.trk;*.arc";
            this.fileDialogSongLoad.Title = "Load Song";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.b33, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.b23, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.b13, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.b03, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.b32, 3, 2);
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
            // b33
            // 
            this.b33.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b33.Location = new System.Drawing.Point(429, 429);
            this.b33.Name = "b33";
            this.b33.Size = new System.Drawing.Size(137, 137);
            this.b33.TabIndex = 15;
            this.b33.UseVisualStyleBackColor = false;
            this.b33.Click += new System.EventHandler(this.ButtonClick);
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
            this.b23.Click += new System.EventHandler(this.ButtonClick);
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
            this.b13.Click += new System.EventHandler(this.ButtonClick);
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
            this.b03.Click += new System.EventHandler(this.ButtonClick);
            // 
            // b32
            // 
            this.b32.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.b32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b32.Location = new System.Drawing.Point(429, 287);
            this.b32.Name = "b32";
            this.b32.Size = new System.Drawing.Size(137, 136);
            this.b32.TabIndex = 11;
            this.b32.UseVisualStyleBackColor = false;
            this.b32.Click += new System.EventHandler(this.ButtonClick);
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
            this.b22.Click += new System.EventHandler(this.ButtonClick);
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
            this.b12.Click += new System.EventHandler(this.ButtonClick);
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
            this.b02.Click += new System.EventHandler(this.ButtonClick);
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
            this.b31.Click += new System.EventHandler(this.ButtonClick);
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
            this.b21.Click += new System.EventHandler(this.ButtonClick);
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
            this.b11.Click += new System.EventHandler(this.ButtonClick);
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
            this.b01.Click += new System.EventHandler(this.ButtonClick);
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
            this.b30.Click += new System.EventHandler(this.ButtonClick);
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
            this.b20.Click += new System.EventHandler(this.ButtonClick);
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
            this.b10.Click += new System.EventHandler(this.ButtonClick);
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
            this.b00.Click += new System.EventHandler(this.ButtonClick);
            // 
            // panel17
            // 
            this.panel17.BackColor = System.Drawing.SystemColors.Control;
            this.panel17.Controls.Add(this.lTimeUpdater);
            this.panel17.Controls.Add(this.lTicks);
            this.panel17.Controls.Add(this.tbMusic);
            this.panel17.Controls.Add(this.buttonStop);
            this.panel17.Controls.Add(this.buttonPlay);
            this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel17.Location = new System.Drawing.Point(0, 24);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(1243, 64);
            this.panel17.TabIndex = 2;
            // 
            // tbMusic
            // 
            this.tbMusic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMusic.Location = new System.Drawing.Point(134, 3);
            this.tbMusic.Maximum = 1000;
            this.tbMusic.Name = "tbMusic";
            this.tbMusic.Size = new System.Drawing.Size(971, 45);
            this.tbMusic.TabIndex = 3;
            this.tbMusic.Scroll += new System.EventHandler(this.TrackBar1_Scroll);
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
            this.buttonStop.Click += new System.EventHandler(this.ButtonStop_Click);
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
            this.buttonPlay.Click += new System.EventHandler(this.ButtonPlay_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // fileDialogNewSong
            // 
            this.fileDialogNewSong.Filter = "Music files|*.mp3";
            this.fileDialogNewSong.Title = "Select a track...";
            // 
            // pgCurrentSong
            // 
            this.pgCurrentSong.Location = new System.Drawing.Point(3, 33);
            this.pgCurrentSong.Name = "pgCurrentSong";
            this.pgCurrentSong.Size = new System.Drawing.Size(332, 282);
            this.pgCurrentSong.TabIndex = 3;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Track Files|*.trk";
            this.saveFileDialog1.Title = "Save song...";
            // 
            // flpSettings1
            // 
            this.flpSettings1.BackColor = System.Drawing.SystemColors.Control;
            this.flpSettings1.Controls.Add(this.flpOptions);
            this.flpSettings1.Controls.Add(this.flpSong);
            this.flpSettings1.Dock = System.Windows.Forms.DockStyle.Left;
            this.flpSettings1.Location = new System.Drawing.Point(0, 88);
            this.flpSettings1.Name = "flpSettings1";
            this.flpSettings1.Size = new System.Drawing.Size(346, 595);
            this.flpSettings1.TabIndex = 4;
            // 
            // flpOptions
            // 
            this.flpOptions.Controls.Add(this.lDifficulty);
            this.flpOptions.Controls.Add(this.cbDifficulty);
            this.flpOptions.Location = new System.Drawing.Point(3, 3);
            this.flpOptions.Name = "flpOptions";
            this.flpOptions.Size = new System.Drawing.Size(335, 36);
            this.flpOptions.TabIndex = 4;
            // 
            // lDifficulty
            // 
            this.lDifficulty.AutoSize = true;
            this.lDifficulty.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDifficulty.Location = new System.Drawing.Point(3, 0);
            this.lDifficulty.Name = "lDifficulty";
            this.lDifficulty.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lDifficulty.Size = new System.Drawing.Size(84, 30);
            this.lDifficulty.TabIndex = 0;
            this.lDifficulty.Text = "Difficulty";
            this.lDifficulty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbDifficulty
            // 
            this.cbDifficulty.DropDownHeight = 120;
            this.cbDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDifficulty.FormattingEnabled = true;
            this.cbDifficulty.IntegralHeight = false;
            this.cbDifficulty.Location = new System.Drawing.Point(93, 8);
            this.cbDifficulty.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cbDifficulty.Name = "cbDifficulty";
            this.cbDifficulty.Size = new System.Drawing.Size(231, 21);
            this.cbDifficulty.TabIndex = 1;
            this.cbDifficulty.SelectedIndexChanged += new System.EventHandler(this.cbDifficulty_SelectedIndexChanged);
            // 
            // flpSong
            // 
            this.flpSong.Controls.Add(this.lSongProp);
            this.flpSong.Controls.Add(this.pgCurrentSong);
            this.flpSong.Location = new System.Drawing.Point(3, 45);
            this.flpSong.Name = "flpSong";
            this.flpSong.Size = new System.Drawing.Size(335, 369);
            this.flpSong.TabIndex = 5;
            // 
            // lSongProp
            // 
            this.lSongProp.AutoSize = true;
            this.lSongProp.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lSongProp.Location = new System.Drawing.Point(3, 0);
            this.lSongProp.Name = "lSongProp";
            this.lSongProp.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lSongProp.Size = new System.Drawing.Size(152, 30);
            this.lSongProp.TabIndex = 4;
            this.lSongProp.Text = "Song Properties";
            this.lSongProp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpSettings2
            // 
            this.flpSettings2.BackColor = System.Drawing.SystemColors.Control;
            this.flpSettings2.Controls.Add(this.flpBeat);
            this.flpSettings2.Dock = System.Windows.Forms.DockStyle.Right;
            this.flpSettings2.Location = new System.Drawing.Point(927, 88);
            this.flpSettings2.Name = "flpSettings2";
            this.flpSettings2.Size = new System.Drawing.Size(316, 595);
            this.flpSettings2.TabIndex = 5;
            // 
            // flpBeat
            // 
            this.flpBeat.Controls.Add(this.lBeatProp);
            this.flpBeat.Controls.Add(this.bDeleteBeat);
            this.flpBeat.Controls.Add(this.pgCurrentBeat);
            this.flpBeat.Location = new System.Drawing.Point(3, 3);
            this.flpBeat.Name = "flpBeat";
            this.flpBeat.Size = new System.Drawing.Size(313, 369);
            this.flpBeat.TabIndex = 5;
            // 
            // lBeatProp
            // 
            this.lBeatProp.AutoSize = true;
            this.lBeatProp.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lBeatProp.Location = new System.Drawing.Point(3, 0);
            this.lBeatProp.Name = "lBeatProp";
            this.lBeatProp.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lBeatProp.Size = new System.Drawing.Size(145, 30);
            this.lBeatProp.TabIndex = 4;
            this.lBeatProp.Text = "Beat Properties";
            this.lBeatProp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pgCurrentBeat
            // 
            this.pgCurrentBeat.Location = new System.Drawing.Point(3, 36);
            this.pgCurrentBeat.Name = "pgCurrentBeat";
            this.pgCurrentBeat.Size = new System.Drawing.Size(310, 282);
            this.pgCurrentBeat.TabIndex = 3;
            // 
            // bDeleteBeat
            // 
            this.bDeleteBeat.Location = new System.Drawing.Point(154, 3);
            this.bDeleteBeat.Name = "bDeleteBeat";
            this.bDeleteBeat.Size = new System.Drawing.Size(145, 27);
            this.bDeleteBeat.TabIndex = 5;
            this.bDeleteBeat.Text = "Delete Beat";
            this.bDeleteBeat.UseVisualStyleBackColor = true;
            this.bDeleteBeat.Click += new System.EventHandler(this.bDeleteBeat_Click);
            // 
            // lTicks
            // 
            this.lTicks.AutoSize = true;
            this.lTicks.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTicks.Location = new System.Drawing.Point(1098, 0);
            this.lTicks.Name = "lTicks";
            this.lTicks.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lTicks.Size = new System.Drawing.Size(132, 30);
            this.lTicks.TabIndex = 5;
            this.lTicks.Text = "Current Time:";
            this.lTicks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lTimeUpdater
            // 
            this.lTimeUpdater.AutoSize = true;
            this.lTimeUpdater.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTimeUpdater.Location = new System.Drawing.Point(1098, 30);
            this.lTimeUpdater.Name = "lTimeUpdater";
            this.lTimeUpdater.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lTimeUpdater.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lTimeUpdater.Size = new System.Drawing.Size(23, 30);
            this.lTimeUpdater.TabIndex = 6;
            this.lTimeUpdater.Text = "0";
            this.lTimeUpdater.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Mapper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1243, 683);
            this.Controls.Add(this.flpSettings2);
            this.Controls.Add(this.flpSettings1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel17);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Mapper";
            this.Text = "YouBeat Mapper";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbMusic)).EndInit();
            this.flpSettings1.ResumeLayout(false);
            this.flpOptions.ResumeLayout(false);
            this.flpOptions.PerformLayout();
            this.flpSong.ResumeLayout(false);
            this.flpSong.PerformLayout();
            this.flpSettings2.ResumeLayout(false);
            this.flpBeat.ResumeLayout(false);
            this.flpBeat.PerformLayout();
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
        private System.Windows.Forms.Button b33;
        private System.Windows.Forms.Button b23;
        private System.Windows.Forms.Button b13;
        private System.Windows.Forms.Button b03;
        private System.Windows.Forms.Button b32;
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
        private System.Windows.Forms.TrackBar tbMusic;
        private System.Windows.Forms.OpenFileDialog fileDialogNewSong;
        private System.Windows.Forms.PropertyGrid pgCurrentSong;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FlowLayoutPanel flpSettings1;
        private System.Windows.Forms.FlowLayoutPanel flpOptions;
        private System.Windows.Forms.Label lDifficulty;
        private System.Windows.Forms.ComboBox cbDifficulty;
        private System.Windows.Forms.FlowLayoutPanel flpSong;
        private System.Windows.Forms.Label lSongProp;
        private System.Windows.Forms.FlowLayoutPanel flpSettings2;
        private System.Windows.Forms.FlowLayoutPanel flpBeat;
        private System.Windows.Forms.Label lBeatProp;
        private System.Windows.Forms.PropertyGrid pgCurrentBeat;
        private System.Windows.Forms.Button bDeleteBeat;
        private System.Windows.Forms.Label lTimeUpdater;
        private System.Windows.Forms.Label lTicks;
    }
}

