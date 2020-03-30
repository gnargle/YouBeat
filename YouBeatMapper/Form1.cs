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
    public partial class Form1 : Form {

        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        private WaveOut wvOut;

        private string SongFolder;

        private bool closing = false;

        public Form1() {
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
                trackBar1.Value = 0;
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
                var activeBeats = CurrentSong.Beats.Where(b => (b.HitTime <= audioFile.Position + 125) && (b.HitTime >= audioFile.Position - 125));
                var inactiveBeats = CurrentSong.Beats.Where(b => (b.HitTime >= audioFile.Position + 125) && (b.HitTime <= audioFile.Position + 250));
                foreach (var beat in inactiveBeats)
                {
                    var ctl = (Button)tableLayoutPanel1.GetControlFromPosition(beat.x, beat.y);
                    ctl.BackColor = Color.White;
                }
                foreach (var beat in activeBeats)
                {
                    var ctl = (Button)tableLayoutPanel1.GetControlFromPosition(beat.x, beat.y);
                    ctl.BackColor = Color.Red;
                }                
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e) {
            if (CurrentSong != null) {
                if (audioFile == null)
                {
                    audioFile = new AudioFileReader(Path.Combine(SongFolder, "Tracks", CurrentSong.FileName));
                }                
                if (wvOut == null)
                {
                    wvOut = new WaveOut();
                    wvOut.PlaybackStopped += OnPlaybackStopped;
                    wvOut.Init(audioFile);
                }
                /*if (outputDevice == null) {
                    outputDevice = new WaveOutEvent();
                    outputDevice.PlaybackStopped += OnPlaybackStopped;
                }*/                
                timer1.Enabled = true;
                wvOut.Play();
                //outputDevice.Play();
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e) {
            if (closing) { outputDevice.Dispose(); audioFile.Dispose(); }
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
    }
}
