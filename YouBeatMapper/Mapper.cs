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
using System.Reflection;

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
            ChangeDescriptionHeight(pgCurrentBeat, 100);
            ChangeDescriptionHeight(pgCurrentSong, 100);
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
                    var img = tfile.Tag.Pictures.FirstOrDefault();
                    if (img != null) {                        
                        var loadedBmp = (Bitmap)Image.FromStream(new MemoryStream(img.Data.Data));
                        var bmp = new Bitmap(loadedBmp, new Size(pImage.Width, pImage.Height));
                        pImage.BackgroundImage = bmp;                        
                        SaveImageData(loadedBmp, CurrentSong.SongName + ".jpg", CurrentSong);
                    }
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
                ImageFileName = "";
                AudioFileName = "";
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
                if (!String.IsNullOrWhiteSpace(CurrentSong.ImageData)) {                    
                    var imgBytes = Convert.FromBase64String(CurrentSong.ImageData);
                    var loadedBmp = (Bitmap)Image.FromStream(new MemoryStream(imgBytes));
                    var bmp = new Bitmap(loadedBmp, new Size(pImage.Width, pImage.Height));
                    pImage.BackgroundImage = bmp;
                } else if (!String.IsNullOrWhiteSpace(ImageFileName)) {
                    var loadedBmp = (Bitmap)Image.FromFile(ImageFileName);
                    var bmp = new Bitmap(loadedBmp, new Size(pImage.Width, pImage.Height));
                    pImage.BackgroundImage = bmp;
                } else {
                    pImage.BackgroundImage = null;
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

        private void SaveSong(Song songToSave, bool skipValidation = false)
        {
            try {
                Cursor.Current = Cursors.WaitCursor;
                bool reload = false;
                if (!skipValidation)
                    ValidateSong();
                //might need to generate the lead in time here and add the lead in time to every note...
                if (!songToSave.LeadInTimeGenerated) {
                    audioFile.Position = 0; //reset position else we'll just offset everything to where it left playing
                    var offset = new OffsetSampleProvider(audioFile.ToSampleProvider());
                    if (songToSave.LastLeadInTime != -1) {
                        var lastLeadInMS = songToSave.LastLeadInTime * 1000;
                        //need to remove the delay then readdd.
                        offset.SkipOver = TimeSpan.FromSeconds(songToSave.LastLeadInTime);
                        //audioFile.Position = CurrentSong.LastLeadInTime * 1000; //this skips the file forward by the amount of time in the last delay.
                        foreach (var beat in songToSave.EasyBeats) {
                            beat.HitTime -= lastLeadInMS;
                        }
                        foreach (var beat in songToSave.AdvancedBeats) {
                            beat.HitTime -= lastLeadInMS;
                        }
                        foreach (var beat in songToSave.ExpertBeats) {
                            beat.HitTime -= lastLeadInMS;
                        }
                        if (songToSave.PreviewStart > lastLeadInMS) songToSave.PreviewStart -= lastLeadInMS;
                        if (songToSave.PreviewEnd > lastLeadInMS) songToSave.PreviewEnd -= lastLeadInMS;
                    }
                    offset.DelayBy = TimeSpan.FromSeconds(songToSave.LeadInTime);
                    var tempFileName = Path.Combine(SongFolder, Path.GetFileNameWithoutExtension(songToSave.FileName) + "extend" + Path.GetExtension(songToSave.FileName));
                    WaveFileWriter.CreateWaveFile(tempFileName, offset.ToWaveProvider());
                    File.Delete(Path.Combine(SongFolder, songToSave.FileName));
                    File.Move(tempFileName, Path.Combine(SongFolder, songToSave.FileName));
                    var leadInMS = songToSave.LeadInTime * 1000;
                    foreach (var beat in songToSave.EasyBeats) {
                        beat.HitTime += leadInMS;
                    }
                    foreach (var beat in songToSave.AdvancedBeats) {
                        beat.HitTime += leadInMS;
                    }
                    foreach (var beat in songToSave.ExpertBeats) {
                        beat.HitTime += leadInMS;
                    }
                    if (songToSave.PreviewStart > 0)
                        songToSave.PreviewStart += leadInMS;
                    if (songToSave.PreviewEnd > 0)
                        songToSave.PreviewEnd += leadInMS;
                    songToSave.LeadInTimeGenerated = true;
                    reload = true; //need to reload to account for the new offset.
                }
                if (!String.IsNullOrWhiteSpace(songToSave.ImageData)) {
                    File.WriteAllBytes(Path.Combine(SongFolder, songToSave.ImageFileName), Convert.FromBase64String(songToSave.ImageData));
                    songToSave.ImageData = String.Empty;
                }
                var json = JsonConvert.SerializeObject(songToSave);
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
                    SaveSong(CurrentSong);
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
                SaveSong(CurrentSong);
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
        private static void ChangeDescriptionHeight(PropertyGrid grid, int height) {
            if (grid == null) throw new ArgumentNullException("grid");

            foreach (Control control in grid.Controls)
                if (control.GetType().Name == "DocComment") {
                    FieldInfo fieldInfo = control.GetType().BaseType.GetField("userSized",
                      BindingFlags.Instance |
                      BindingFlags.NonPublic);
                    fieldInfo.SetValue(control, true);
                    control.Height = height;
                    return;
                }
        }

        private void SaveImageData(Bitmap bmp, string ImageFileName, Song song){
            if (CurrentSong != null) {
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] imageBytes = stream.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                song.ImageData = base64String;
                song.ImageFileName = Path.GetFileName(ImageFileName);
            }
        }

        private void bLoadImage_Click(object sender, EventArgs e) {
            if (fileDialogImageLoad.ShowDialog() == DialogResult.OK) {
                var loadedBmp = (Bitmap)Image.FromFile(fileDialogImageLoad.FileName);
                var bmp = new Bitmap(loadedBmp, new Size(pImage.Width, pImage.Height));
                pImage.BackgroundImage = bmp;
                SaveImageData(loadedBmp, fileDialogImageLoad.FileName, CurrentSong);
                pgCurrentSong.Refresh();
            }
        }

        private void ConvertJubeatFormat(string mp3, Song newSong) {
            SongFolder = Path.Combine(Path.GetDirectoryName(mp3), Path.GetFileNameWithoutExtension(mp3));
            Directory.CreateDirectory(SongFolder);
            File.Copy(mp3, Path.Combine(SongFolder, Path.GetFileName(mp3)), true);//drop audio into folder for later
            var songToConvert = Path.GetFileNameWithoutExtension(mp3);
            var directory = Path.GetDirectoryName(mp3);
            newSong.LeadInTime = 1;
            newSong.FileName = Path.GetFileName(mp3);
            var tfile = TagLib.File.Create(mp3);
            newSong.Artist = tfile.Tag.FirstPerformer;
            string search = String.Empty;
            if (int.TryParse(songToConvert.Substring(0, 2), out int i))
                search = i.ToString() + "*";
            else
                search = $"{songToConvert.Replace(' ', '_')}*";
            foreach (var file in Directory.GetFiles(directory, search)) {
                if (!Path.GetExtension(file).Equals(".txt", StringComparison.InvariantCultureIgnoreCase))
                    continue;
                bool readNotes = false;
                var fileWithoutExt = Path.GetFileNameWithoutExtension(file);
                var lines = File.ReadAllLines(file).ToList();
                int offset = 0;
                List<Beat> beats = new List<Beat>();
                foreach (var line in lines) {
                    if (readNotes) {
                        var data = line.Split(',');
                        var pad = Convert.ToInt32(data[1]);
                        int x = -1; int y = -1;
                        switch (pad) {
                            case 1: x = 0; y = 0; break;
                            case 2: x = 0; y = 1; break;
                            case 3: x = 0; y = 2; break;
                            case 4: x = 0; y = 3; break;
                            case 5: x = 1; y = 0; break;
                            case 6: x = 1; y = 1; break;
                            case 7: x = 1; y = 2; break;
                            case 8: x = 1; y = 3; break;
                            case 9: x = 2; y = 0; break;
                            case 10: x = 2; y = 1; break;
                            case 11: x = 2; y = 2; break;
                            case 12: x = 2; y = 3; break;
                            case 13: x = 3; y = 0; break;
                            case 14: x = 3; y = 1; break;
                            case 15: x = 3; y = 2; break;
                            case 16: x = 3; y = 3; break;
                        }
                        beats.Add(new Beat(Convert.ToInt64(data[0]) + offset, x, y)); //the measuring is slightly off from naudio, need to bump it a bit
                    } else if (line.StartsWith("#")) {
                        //metadata line
                        var data = line.Split(':');
                        if (data[0] == "#TITLE_NAME") {
                            newSong.Title = data[1];
                            newSong.SongName = data[1];
                        } else if (data[0] == "#BPM")
                            newSong.BPM = (int)Convert.ToDouble(data[1]) / 100;
                        else if (data[0] == "#MUSIC_TIME")
                            newSong.EndTime = Convert.ToInt64(data[1]);
                        else if (data[0] == "#AUDIO_FILE")
                            newSong.FileName = data[1];
                        else if (data[0] == "#OFFSET")
                            offset = Convert.ToInt32(data[1]);
                    } else if (line.Equals("[Notes]", StringComparison.InvariantCultureIgnoreCase)) {
                        readNotes = true;
                        continue;
                    }
                }
                if (fileWithoutExt.EndsWith("bsc", StringComparison.InvariantCultureIgnoreCase))
                    newSong.EasyBeats = beats;
                else if (fileWithoutExt.EndsWith("adv", StringComparison.InvariantCultureIgnoreCase))
                    newSong.AdvancedBeats = beats;
                else if (fileWithoutExt.EndsWith("ext", StringComparison.InvariantCultureIgnoreCase))
                    newSong.ExpertBeats = beats;
            }
            var img = tfile.Tag.Pictures.FirstOrDefault();
            if (img != null) {
                var loadedBmp = (Bitmap)Image.FromStream(new MemoryStream(img.Data.Data));
                var bmp = new Bitmap(loadedBmp, new Size(pImage.Width, pImage.Height));
                pImage.BackgroundImage = bmp;
                SaveImageData(loadedBmp, newSong.SongName + ".jpg", newSong);
            }
            CurrentSong = newSong;
            cbDifficulty.SelectedIndex = 1;
            cbDifficulty_SelectedIndexChanged(cbDifficulty, null);
            pgCurrentSong.SelectedObject = newSong;
            audioFile = new MediaFoundationReader(Path.Combine(SongFolder, Path.GetFileName(mp3)));
            wvOut = new WaveOut();
            wvOut.PlaybackStopped += OnPlaybackStopped;
            wvOut.Init(audioFile);
            launchpad.AudioFile = audioFile;
            launchpad.WvOut = wvOut;
            launchpad.CurrentSong = CurrentSong;
            tbMusic.Value = 0;
            timer1.Enabled = true;
        }

        private void convertJubeatFormatToolStripMenuItem_Click(object sender, EventArgs e) {
            if (fileDialogNewSong.ShowDialog() == DialogResult.OK) {
                CurrentSong = new Song();
                ConvertJubeatFormat(fileDialogNewSong.FileName, CurrentSong);
            }
        }

        private void convertJubeatFolderToolStripMenuItem_Click(object sender, EventArgs e) {
            if (folderBrowserDialogJubeatFolder.ShowDialog() == DialogResult.OK) {
                var saveDir = Path.Combine(folderBrowserDialogJubeatFolder.SelectedPath, "YoubeatGenerated");
                Directory.CreateDirectory(saveDir);
                List<String> failedSongs = new List<string>();
                String prevSongFolder = String.Empty;
                foreach (var file in Directory.GetFiles(folderBrowserDialogJubeatFolder.SelectedPath, "*.mp3")) {
                    try {
                        var newSong = new Song();
                        ConvertJubeatFormat(file, newSong);
                        SongFile = Path.Combine(saveDir, newSong.SongName + ".trk");
                        prevSongFolder = SongFolder;
                        SaveSong(newSong, true);                        
                    } catch {
                        failedSongs.Add(Path.GetFileName(file));
                    } finally {
                        if (!String.IsNullOrWhiteSpace(prevSongFolder) && Directory.Exists(prevSongFolder)) {
                            foreach (var genFile in Directory.GetFiles(prevSongFolder)) {
                                File.Delete(genFile);
                            }
                            Directory.Delete(prevSongFolder);
                        }
                    }
                }
                string msg = "Your songs have been saved in " + saveDir;
                if (failedSongs.Any()) {
                    msg += Environment.NewLine;
                    msg += "These songs failed to import:" + Environment.NewLine;
                    foreach (var failedSong in failedSongs) {
                        msg += failedSong + Environment.NewLine;
                    }
                    msg += "You should try and import these tracks manually.";
                }
                MessageBox.Show(msg);
            }
        }
    }
}
