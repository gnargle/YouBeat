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
        private Beat SelectedBeat;
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
                try {
                    Cursor.Current = Cursors.WaitCursor;
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
                        SongName = Path.GetFileNameWithoutExtension(fileDialogNewSong.FileName),
                        LeadInTime = 1
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
                    tbMusic.Value = 0;
                } finally {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void LoadSong(string filename) {
            try {
                Cursor.Current = Cursors.WaitCursor;
                SongFile = filename;
                SongFolder = Path.Combine(Path.GetDirectoryName(SongFile), Path.GetFileNameWithoutExtension(SongFile));
                if (Directory.Exists(SongFolder))
                    Directory.Delete(SongFolder, true);
                ZipFile.ExtractToDirectory(SongFile, SongFolder);
                foreach (var tempFile in Directory.GetFiles(SongFolder)) {
                    if (tempFile.EndsWith(".js"))
                        continue;
                    else if (tempFile.EndsWith(".jpg") || tempFile.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                        ImageFileName = tempFile;
                    else
                        AudioFileName = tempFile;
                }
                //var zip = ZipFile.OpenRead(SongFile);
                var file = Path.Combine(SongFolder, Path.GetFileNameWithoutExtension(filename) + ".js");
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
                tbMusic.Value = 0;
                timer1.Enabled = true;
            } finally {
                Cursor.Current = Cursors.Default;
            }
        }

        private void LoadSongToolStripMenuItem_Click(object sender, EventArgs e) {
            if (fileDialogSongLoad.ShowDialog() == DialogResult.OK) {
                LoadSong(fileDialogSongLoad.FileName);
            }
        }

        private void UpdatePosition()
        {
            tbMusic.Value = Math.Min((int)((tbMusic.Maximum * audioFile.Position) / audioFile.Length), tbMusic.Maximum);
        }

        private void UpdateGrid()
        {
            if (CurrentSong != null && CurrentBeats != null)
            {
                //these aren't right rn
                var currTime = Convert.ToInt64(audioFile.CurrentTime.TotalMilliseconds);
                var activeBeats = CurrentBeats.Where(b => (b.HitTime <= currTime + MapperLPInterf.BEAT_DUR) && (b.HitTime >= currTime - MapperLPInterf.BEAT_DUR));
                var inactiveBeats = CurrentBeats.Where(b => (b.HitTime <= currTime - MapperLPInterf.BEAT_DUR) ||  (b.HitTime >= currTime + MapperLPInterf.BEAT_DUR));
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
            lTimeUpdater.Text = audioFile?.CurrentTime.TotalMilliseconds.ToString();
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                audioFile.Position = (tbMusic.Value * audioFile.Length) / tbMusic.Maximum;
            }
            UpdateGrid();
        }
        
        private void AssignBeat(Beat beat) {
            SelectedBeat = beat;
            pgCurrentBeat.SelectedObject = beat;
        }

        private void ButtonClick(object sender, EventArgs e) {
            if (CurrentSong != null) {
                var currTime = Convert.ToInt64(audioFile.CurrentTime.TotalMilliseconds);
                var coords = tableLayoutPanel1.GetPositionFromControl((Control)sender);
                var existingBeat = CurrentBeats.Where(b => (b.HitTime <= currTime + 125) && (b.HitTime >= currTime - 125) && b.x == coords.Row && b.y == coords.Column).FirstOrDefault();
                if (existingBeat != null) {
                    AssignBeat(existingBeat);               
                } else {
                    var newBeat = new Beat(currTime, coords.Row, coords.Column);
                    AssignBeat(newBeat);
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
            if (CurrentSong.LeadInTime < 1) {
                throw new SaveValidationException("You must have a lead-in time of at least a second.");
            }
        }

        private void SaveSong()
        {
            try {
                Cursor.Current = Cursors.WaitCursor;
                bool reload = false;
                ValidateSong();
                //might need to generate the lead in time here and add the lead in time to every note...
                if (!CurrentSong.LeadInTimeGenerated) {
                    audioFile.Position = 0; //reset position else we'll just offset everything to where it left playing
                    var offset = new OffsetSampleProvider(audioFile.ToSampleProvider());
                    if (CurrentSong.LastLeadInTime != -1) {
                        var lastLeadInMS = CurrentSong.LastLeadInTime * 1000;
                        //need to remove the delay then readdd.
                        offset.SkipOver = TimeSpan.FromSeconds(CurrentSong.LastLeadInTime);
                        //audioFile.Position = CurrentSong.LastLeadInTime * 1000; //this skips the file forward by the amount of time in the last delay.
                        foreach (var beat in CurrentSong.EasyBeats) {
                            beat.HitTime -= lastLeadInMS;
                        }
                        foreach (var beat in CurrentSong.AdvancedBeats) {
                            beat.HitTime -= lastLeadInMS;
                        }
                        foreach (var beat in CurrentSong.ExpertBeats) {
                            beat.HitTime -= lastLeadInMS;
                        }
                        if (CurrentSong.PreviewStart > lastLeadInMS) CurrentSong.PreviewStart -= lastLeadInMS;
                        if (CurrentSong.PreviewEnd > lastLeadInMS) CurrentSong.PreviewEnd -= lastLeadInMS;
                    }
                    offset.DelayBy = TimeSpan.FromSeconds(CurrentSong.LeadInTime);
                    var tempFileName = Path.Combine(SongFolder, Path.GetFileNameWithoutExtension(CurrentSong.FileName) + "extend" + Path.GetExtension(CurrentSong.FileName));
                    WaveFileWriter.CreateWaveFile(tempFileName, offset.ToWaveProvider());
                    File.Delete(Path.Combine(SongFolder, CurrentSong.FileName));
                    File.Move(tempFileName, Path.Combine(SongFolder, CurrentSong.FileName));
                    var leadInMS = CurrentSong.LeadInTime * 1000;
                    foreach (var beat in CurrentSong.EasyBeats) {
                        beat.HitTime += leadInMS;
                    }
                    foreach (var beat in CurrentSong.AdvancedBeats) {
                        beat.HitTime += leadInMS;
                    }
                    foreach (var beat in CurrentSong.ExpertBeats) {
                        beat.HitTime += leadInMS;
                    }
                    if (CurrentSong.PreviewStart > 0)
                        CurrentSong.PreviewStart += leadInMS;
                    if (CurrentSong.PreviewEnd > 0)
                        CurrentSong.PreviewEnd += leadInMS;
                    CurrentSong.LeadInTimeGenerated = true;
                    reload = true; //need to reload to account for the new offset.
                }
                var json = JsonConvert.SerializeObject(CurrentSong);
                File.WriteAllText(Path.Combine(SongFolder, $"{Path.GetFileNameWithoutExtension(SongFile)}.js"), json);
                File.Delete(SongFile);
                ZipFile.CreateFromDirectory(SongFolder, SongFile);
                if (reload) {
                    LoadSong(SongFile);
                }
            } catch (SaveValidationException e) {
                MessageBox.Show("Your song has not been saved due to an error: " + e.Message, "Save validation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                Cursor.Current = Cursors.Default;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentSong != null)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    SongFile = saveFileDialog1.FileName;
                    CurrentSong.SongName = Path.GetFileNameWithoutExtension(SongFile);
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

        private void bDeleteBeat_Click(object sender, EventArgs e) {
            if (SelectedBeat != null) {
                CurrentBeats.Remove(SelectedBeat);
                pgCurrentBeat.SelectedObject = null;
            }
        }
    }
}
