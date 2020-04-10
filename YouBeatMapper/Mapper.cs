using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YouBeatTypes;
using LaunchpadNET;

namespace YouBeatMapper {
    public partial class Mapper : Form {

        private MediaFoundationReader audioFile;
        private WaveOut wvOut;

        private string SongFolder;
        private string SongFile;
        private string AudioFileName;
        private string ImageFileName;

        private bool closing = false;       
        private MapperLPInterf launchpad;

        public Mapper() {
            InitializeComponent();
            this.FormClosing += buttonStop_Click;
            launchpad = new MapperLPInterf(); 
        }

        public Song CurrentSong { get; private set; }

        private void loadSongToolStripMenuItem_Click(object sender, EventArgs e) {
            if (fileDialogSongLoad.ShowDialog() == DialogResult.OK) {
                SongFile = fileDialogSongLoad.FileName;
                SongFolder = Path.Combine(Path.GetDirectoryName(SongFile), Path.GetFileNameWithoutExtension(SongFile));
                if (Directory.Exists(SongFolder))
                    Directory.Delete(SongFolder, true);
                ZipFile.ExtractToDirectory(SongFile, SongFolder);
                foreach (var tempFile in Directory.GetFiles(SongFolder))
                {
                    if (tempFile.EndsWith(".js"))
                        continue;
                    else if (tempFile.EndsWith(".jpg") || tempFile.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                        ImageFileName = tempFile;
                    else
                        AudioFileName = tempFile;
                }
                //var zip = ZipFile.OpenRead(SongFile);
                var file = Path.Combine(SongFolder, Path.GetFileNameWithoutExtension(fileDialogSongLoad.FileName) + ".js");
                var json = File.ReadAllText(file);
                CurrentSong = JsonConvert.DeserializeObject<Song>(json);
                pgCurrentSong.SelectedObject = CurrentSong;
                audioFile = new MediaFoundationReader(AudioFileName);
                wvOut = new WaveOut();
                wvOut.PlaybackStopped += OnPlaybackStopped;
                wvOut.Init(audioFile);
                launchpad.audioFile = audioFile;
                launchpad.wvOut = wvOut;
                launchpad.CurrentSong = CurrentSong;
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
            launchpad.UpdatePads();
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
                SongFile = String.Empty;
                SongFolder = Path.Combine(Path.GetDirectoryName(fileDialogNewSong.FileName), Path.GetFileNameWithoutExtension(fileDialogNewSong.FileName));
                Directory.CreateDirectory(SongFolder);
                File.Copy(fileDialogNewSong.FileName, Path.Combine(SongFolder, Path.GetFileName(fileDialogNewSong.FileName)), true);//drop audio into folder for later
                var tfile = TagLib.File.Create(fileDialogNewSong.FileName);
                CurrentSong = new Song() {
                    FileName = Path.GetFileName(fileDialogNewSong.FileName),
                    Beats = new List<Beat>(),
                    Title = tfile.Tag.Title,
                    Artist = tfile.Tag.FirstPerformer,
                    BPM = (int)tfile.Tag.BeatsPerMinute,    
                    SongName = Path.GetFileNameWithoutExtension(fileDialogNewSong.FileName)
                };
                pgCurrentSong.SelectedObject = CurrentSong;
                audioFile = new MediaFoundationReader(Path.Combine(SongFolder, Path.GetFileName(fileDialogNewSong.FileName)));
                wvOut = new WaveOut();
                wvOut.PlaybackStopped += OnPlaybackStopped;
                wvOut.Init(audioFile);
                launchpad.audioFile = audioFile;
                launchpad.wvOut = wvOut;
                launchpad.CurrentSong = CurrentSong;
                timer1.Enabled = true;
                trackBar1.Value = 0;
            }
        }

        private void buttonClick(object sender, EventArgs e) {
            if (CurrentSong != null) {
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

        private void saveSong()
        {
            var json = JsonConvert.SerializeObject(CurrentSong);
            File.WriteAllText(Path.Combine(SongFolder, $"{Path.GetFileNameWithoutExtension(SongFile)}.js"), json);
            File.Delete(SongFile);
            ZipFile.CreateFromDirectory(SongFolder, SongFile);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentSong != null)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    SongFile = saveFileDialog1.FileName;
                    saveSong();
                }
            } else
            {
                MessageBox.Show("Please create or load a song before saving.");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(SongFile))
                saveAsToolStripMenuItem_Click(sender, e);
            else if (CurrentSong != null)
            {
                saveSong();
            }
        }
    }
}
