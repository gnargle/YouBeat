using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YouBeatTypes;

namespace YouBeatMapper {
    public partial class Mapper : Form {

        private MediaFoundationReader audioFile;
        private WaveOut wvOut;

        private string SongFolder;

        private bool closing = false;

        public Mapper() {
            InitializeComponent();
            fileDialogSongLoad.InitialDirectory = Directory.GetCurrentDirectory();
            this.FormClosing += buttonStop_Click;
        }

        public Song CurrentSong { get; private set; }

        private void loadSongToolStripMenuItem_Click(object sender, EventArgs e) {
            if (fileDialogSongLoad.ShowDialog() == DialogResult.OK) {
                SongFolder = Path.GetDirectoryName(fileDialogSongLoad.FileName);
                var file = fileDialogSongLoad.FileName;
                var json = File.ReadAllText(file);
                CurrentSong = JsonConvert.DeserializeObject<Song>(json);
                audioFile = new MediaFoundationReader(Path.Combine(SongFolder, "Tracks", CurrentSong.FileName));
                wvOut = new WaveOut();
                wvOut.PlaybackStopped += OnPlaybackStopped;
                wvOut.Init(audioFile);
                trackBar1.Value = 0;
                timer1.Enabled = true;
            }
        }

        private void UpdatePosition()
        {
            trackBar1.Value = Math.Min((int)((trackBar1.Maximum * audioFile.Position) / audioFile.Length), trackBar1.Maximum);
        }

        private void UpdateGrid()
        {
            if (CurrentSong != null)
            {
                //these aren't right rn
                var currTime = Convert.ToInt64(audioFile.CurrentTime.TotalMilliseconds);
                var activeBeats = CurrentSong.Beats.Where(b => (b.HitTime <= currTime + 125) && (b.HitTime >= currTime - 125));
                var inactiveBeats = CurrentSong.Beats.Where(b => (b.HitTime <= currTime - 125) ||  (b.HitTime >= currTime + 125));
                List<Button> ctlsSeen = new List<Button>();
                foreach (var beat in activeBeats) {
                    var ctl = (Button)tableLayoutPanel1.GetControlFromPosition(beat.x, beat.y);
                    if (!ctlsSeen.Contains(ctl)) {
                        ctl.BackColor = Color.Red;
                        ctlsSeen.Add(ctl);
                    }
                }
                foreach (var beat in inactiveBeats)
                {
                    var ctl = (Button)tableLayoutPanel1.GetControlFromPosition(beat.x, beat.y);
                    if (!ctlsSeen.Contains(ctl)) {
                        ctl.BackColor = Color.White;
                        ctlsSeen.Add(ctl);
                    }
                }     
                foreach (Button ctl in tableLayoutPanel1.Controls) {
                    if (!ctlsSeen.Contains(ctl)) {
                        ctl.BackColor = Color.White;
                        ctlsSeen.Add(ctl);
                    }
                }
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e) {
            if (CurrentSong != null) {
                if (audioFile == null)
                {
                    audioFile = new MediaFoundationReader(Path.Combine(SongFolder, "Tracks", CurrentSong.FileName));
                }                
                if (wvOut == null)
                {
                    wvOut = new WaveOut();
                    wvOut.PlaybackStopped += OnPlaybackStopped;
                    wvOut.Init(audioFile);
                }
                wvOut.Play();
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e) {
            if (closing) { wvOut.Dispose(); audioFile.Dispose(); }
        }

        private void buttonStop_Click(object sender, EventArgs e) {
            wvOut?.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                UpdatePosition();
            }
            UpdateGrid();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                audioFile.Position = (trackBar1.Value * audioFile.Length) / trackBar1.Maximum;
            }
            UpdateGrid();
        }

        private void newSongToolStripMenuItem_Click(object sender, EventArgs e) {
            if (fileDialogNewSong.ShowDialog() == DialogResult.OK) {
                SongFolder = Path.GetDirectoryName(fileDialogNewSong.FileName);
                var tfile = TagLib.File.Create(fileDialogNewSong.FileName);
                CurrentSong = new Song() {
                    FileName = Path.GetFileName(fileDialogNewSong.FileName),
                    Beats = new List<Beat>(),
                    Title = tfile.Tag.Title,
                    Artist = tfile.Tag.FirstPerformer,
                    BPM = (int)tfile.Tag.BeatsPerMinute,                  
                };
                audioFile = new MediaFoundationReader(fileDialogNewSong.FileName);
                wvOut = new WaveOut();
                wvOut.PlaybackStopped += OnPlaybackStopped;
                wvOut.Init(audioFile);
                timer1.Enabled = true;
                trackBar1.Value = 0;
            }
        }

        private void buttonClick(object sender, EventArgs e) {
            var currTime = Convert.ToInt64(audioFile.CurrentTime.TotalMilliseconds);
            var coords = tableLayoutPanel1.GetPositionFromControl((Control)sender);
            var existingBeat = CurrentSong.Beats.Where(b => (b.HitTime <= currTime + 125) && (b.HitTime >= currTime - 125) && b.x == coords.Column && b.y == coords.Row).FirstOrDefault();
            if (existingBeat != null) {
                CurrentSong.Beats.Remove(existingBeat);
            } else {
                var newBeat = new Beat(currTime, coords.Column, coords.Row);
                CurrentSong.Beats.Add(newBeat);
            }
        }
    }
}
