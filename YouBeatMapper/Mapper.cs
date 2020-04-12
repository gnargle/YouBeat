﻿using NAudio.Wave;
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
using NAudio.Wave.SampleProviders;

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
        private List<Beat> CurrentBeats;
        public Song CurrentSong { get; private set; }

        public Mapper() {
            InitializeComponent();
            this.FormClosing += ButtonStop_Click;
            launchpad = new MapperLPInterf();
            cbDifficulty.DataSource = Enum.GetValues(typeof(Difficulty));
            cbDifficulty.SelectedIndex = 1;
        }
        
        private void NewSongToolStripMenuItem_Click(object sender, EventArgs e) {
            if (fileDialogNewSong.ShowDialog() == DialogResult.OK) {
                SongFile = String.Empty;
                SongFolder = Path.Combine(Path.GetDirectoryName(fileDialogNewSong.FileName), Path.GetFileNameWithoutExtension(fileDialogNewSong.FileName));
                Directory.CreateDirectory(SongFolder);
                File.Copy(fileDialogNewSong.FileName, Path.Combine(SongFolder, Path.GetFileName(fileDialogNewSong.FileName)), true);//drop audio into folder for later
                var tfile = TagLib.File.Create(fileDialogNewSong.FileName);
                CurrentSong = new Song() {
                    FileName = Path.GetFileName(fileDialogNewSong.FileName),
                    EasyBeats = new List<Beat>(),
                    AdvancedBeats = new List<Beat>(),
                    ExpertBeats = new List<Beat>(),
                    Title = tfile.Tag.Title,
                    Artist = tfile.Tag.FirstPerformer,
                    BPM = (int)tfile.Tag.BeatsPerMinute,
                    SongName = Path.GetFileNameWithoutExtension(fileDialogNewSong.FileName)
                };
                cbDifficulty.SelectedIndex = 1;
                cbDifficulty_SelectedIndexChanged(cbDifficulty, null);
                CurrentBeats = CurrentSong.AdvancedBeats;
                pgCurrentSong.SelectedObject = CurrentSong;
                audioFile = new MediaFoundationReader(Path.Combine(SongFolder, Path.GetFileName(fileDialogNewSong.FileName)));
                wvOut = new WaveOut();
                wvOut.PlaybackStopped += OnPlaybackStopped;
                wvOut.Init(audioFile);
                launchpad.AudioFile = audioFile;
                launchpad.WvOut = wvOut;
                launchpad.CurrentSong = CurrentSong;
                timer1.Enabled = true;
                trackBar1.Value = 0;
            }
        }

        private void LoadSongToolStripMenuItem_Click(object sender, EventArgs e) {
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
                if (CurrentSong.EasyBeats == null) {
                    CurrentSong.EasyBeats = new List<Beat>();
                }
                if (CurrentSong.AdvancedBeats == null) {
                    CurrentSong.AdvancedBeats = new List<Beat>();
                }
                if (CurrentSong.ExpertBeats == null) {
                    CurrentSong.ExpertBeats = new List<Beat>();
                }
                cbDifficulty.SelectedIndex = 1;
                cbDifficulty_SelectedIndexChanged(cbDifficulty, null);
                pgCurrentSong.SelectedObject = CurrentSong;
                audioFile = new MediaFoundationReader(AudioFileName);
                wvOut = new WaveOut();
                wvOut.PlaybackStopped += OnPlaybackStopped;
                wvOut.Init(audioFile);
                launchpad.AudioFile = audioFile;
                launchpad.WvOut = wvOut;
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
            if (CurrentSong != null && CurrentBeats != null)
            {
                //these aren't right rn
                var currTime = Convert.ToInt64(audioFile.CurrentTime.TotalMilliseconds);
                var activeBeats = CurrentBeats.Where(b => (b.HitTime <= currTime + 125) && (b.HitTime >= currTime - 125));
                var inactiveBeats = CurrentBeats.Where(b => (b.HitTime <= currTime - 125) ||  (b.HitTime >= currTime + 125));
                List<Button> ctlsSeen = new List<Button>();
                foreach (var beat in activeBeats) {
                    var ctl = (Button)tableLayoutPanel1.GetControlFromPosition(beat.y, beat.x);
                    if (!ctlsSeen.Contains(ctl)) {
                        ctl.BackColor = Color.Red;
                        ctlsSeen.Add(ctl);
                    }
                }
                try {
                    foreach (var beat in inactiveBeats) {
                        var ctl = (Button)tableLayoutPanel1.GetControlFromPosition(beat.y, beat.x);
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
                } catch { } //ignore any exceptions, grid will be updated if we're changing the beats anyway
            }
        }

        private void ButtonPlay_Click(object sender, EventArgs e) {
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

        private void ButtonStop_Click(object sender, EventArgs e) {
            wvOut?.Stop();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                UpdatePosition();
            }
            UpdateGrid();
            launchpad.UpdatePads();
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                audioFile.Position = (trackBar1.Value * audioFile.Length) / trackBar1.Maximum;
            }
            UpdateGrid();
        }        

        private void ButtonClick(object sender, EventArgs e) {
            if (CurrentSong != null) {
                var currTime = Convert.ToInt64(audioFile.CurrentTime.TotalMilliseconds);
                var coords = tableLayoutPanel1.GetPositionFromControl((Control)sender);
                var existingBeat = CurrentBeats.Where(b => (b.HitTime <= currTime + 125) && (b.HitTime >= currTime - 125) && b.x == coords.Row && b.y == coords.Column).FirstOrDefault();
                if (existingBeat != null) {
                    CurrentBeats.Remove(existingBeat);
                } else {
                    var newBeat = new Beat(currTime, coords.Row, coords.Column);
                    CurrentBeats.Add(newBeat);
                }
            }
        }

        private void ValidateSong() {
            if (CurrentSong == null) 
                throw new SaveValidationException("No song is loaded!");            
            if (CurrentSong.BPM < 40 || CurrentSong.BPM > 240)
                throw new SaveValidationException("BPM set to an invalid value. BPM of song must be between 40 and 240 for the launchpad light pulse to function.");
            if (!CurrentBeats.Any())
                throw new SaveValidationException("No beats in current difficulty track.");
        }

        private void SaveSong()
        {
            try {
                ValidateSong();
                //might need to generate the lead in time here and add the lead in time to every note...
                /*if (!CurrentSong.LeadInTimeGenerated) {
                    var offset = new OffsetSampleProvider(audioFile.ToSampleProvider());
                    offset.DelayBy = TimeSpan.FromSeconds(CurrentSong.LeadInTime);
                    audioFile = (MediaFoundationReader)offset.ToWaveProvider();
                }*/
                var json = JsonConvert.SerializeObject(CurrentSong);
                File.WriteAllText(Path.Combine(SongFolder, $"{Path.GetFileNameWithoutExtension(SongFile)}.js"), json);
                File.Delete(SongFile);
                ZipFile.CreateFromDirectory(SongFolder, SongFile);
            } catch (SaveValidationException e) {
                MessageBox.Show("Your song has not been saved due to an error: " + e.Message, "Save validation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentSong != null)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    SongFile = saveFileDialog1.FileName;
                    SaveSong();
                }
            } else
            {
                MessageBox.Show("Please create or load a song before saving.");
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(SongFile) || SongFile.EndsWith(".arc"))
                SaveAsToolStripMenuItem_Click(sender, e);
            else if (CurrentSong != null)
            {
                SaveSong();
            }
        }

        private void cbDifficulty_SelectedIndexChanged(object sender, EventArgs e) {
            if (CurrentSong != null) {
                if (cbDifficulty.SelectedIndex == 0) 
                    CurrentBeats = CurrentSong.EasyBeats;                    
                else if (cbDifficulty.SelectedIndex == 1)
                    CurrentBeats = CurrentSong.AdvancedBeats;
                else
                    CurrentBeats = CurrentSong.ExpertBeats;
                launchpad.CurrentBeats = CurrentBeats;
            }
        }
    }
}
