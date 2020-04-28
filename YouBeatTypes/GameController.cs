using LaunchpadNET;
using Midi.Enums;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Timers;
using static LaunchpadNET.Interface;

namespace YouBeatTypes {
    public class GameController {
        private string[] songZips;
        private List<Song> Songs = new List<Song>();
        private MediaFoundationReader audioFile;
        private WaveOutEvent outputDevice;

        private bool _lArrowHeld = false;
        private bool _rArrowHeld = false;        

        public Interface interf;
        public int velo;

        public const int ARROW_VELO = 37;
        public const int ARROW_HELD_VELO = 57;
        public const int CONFIRM_VELO = 21;
        public const int CANCEL_VELO = 5;
        public long Elapsed { get; set; }
        public long Separation { get; set; }      
        public long HalfSep { get; set; }
        public GameState State { get; set; } = GameState.Init;
        public MenuState menuState { get; set; } = MenuState.SongSelect;
        public Difficulty SelectedDifficulty { get; set; } = Difficulty.Easy;
        public Dictionary<Tuple<int, int>, Pad> Pads = new Dictionary<Tuple<int, int>, Pad>();
        public List<Beat> Beats = new List<Beat>();
        
        public Song CurrentSong { get; set; }
        public long Score { get; set; }     
        public long Combo { get; set; }
        public long MaxCombo { get; set; }
        public long TotalBeats { get { return (long)Beats?.Count; } }
        public int CurrentComboVelo { get; set; } = 0;
        public bool NewHighScore { get; set; } = false;
        public string HighScoreName { get; set; } = String.Empty;

        private object ComboLock = new object();
        private object ScoreLock = new object();

        private System.Media.SoundPlayer hitSound = new System.Media.SoundPlayer("FX\\Blip_Perfect.wav");

        private Timer songVolumeTimer;
        private bool reachedPreviewEnd = false;       

        public void AddToScore(long moreScore) {
            lock (ScoreLock) {
                Score += moreScore;
            }
        }

        public void PlayHitSound() {
            hitSound.Play();
        }

        public void UpdateCombo (ComboChange change) {
            lock (ComboLock) {
                switch (change) {
                    case ComboChange.Add:
                        Combo++;
                        if (Combo > MaxCombo)
                            MaxCombo = Combo;
                        break;
                    case ComboChange.Break:
                        Combo = 0;
                        break;
                }                
                if (Combo == 0) CurrentComboVelo = 0;                
                else {
                    var veloDecider = Beats.Count / Combo;
                    switch (veloDecider) {
                        case 5:
                            CurrentComboVelo = 37;
                            break;
                        case 4:
                            CurrentComboVelo = 45;
                            break;
                        case 3:
                            CurrentComboVelo = 53;
                            break;
                        case 2:
                            CurrentComboVelo = 118;
                            break;
                        case 1:
                            CurrentComboVelo = 12;
                            break;
                        default:
                            CurrentComboVelo = 0;
                            break;
                    }
                }
            }
        }
        
        private MenuKey KeyInMenuObject(int x, int y) {
            var leftArrow = new List<Pitch>() { Pitch.A5, Pitch.ASharp5, Pitch.B5, Pitch.C6, Pitch.CSharp6, Pitch.B4, Pitch.C5, Pitch.CSharp4 };
            var rightArrow = new List<Pitch>() { Pitch.D0, Pitch.DSharp0, Pitch.E0, Pitch.F0, Pitch.FSharp0, Pitch.DSharp1, Pitch.E1, Pitch.D2 };
            var confirm = new List<Pitch>() { Pitch.A1, Pitch.ASharp1, Pitch.B1, Pitch.C2,Pitch.G2, Pitch.GSharp2, Pitch.A2, Pitch.ASharp2,
                                                   Pitch.F3, Pitch.FSharp3, Pitch.G3, Pitch.GSharp3, Pitch.DSharp4, Pitch.E4, Pitch.F4, Pitch.FSharp4};
            var cancel = new List<Pitch>() { Pitch.BNeg1, Pitch.C0, Pitch.A0, Pitch.ASharp0, Pitch.F5, Pitch.FSharp5, Pitch.DSharp6, Pitch.E6}; 
            var note = interf.ledToMidiNote(x, y);
            if (leftArrow.Contains(note)) {
                return MenuKey.LeftArrow;
            } else if (rightArrow.Contains(note)) {
                return MenuKey.RightArrow;
            } else if (confirm.Contains(note)) {
                return MenuKey.Confim;
            } else if (cancel.Contains(note)) {
                return MenuKey.Cancel;
            }
            return MenuKey.None;
        }

        private void ChangeMenuColour(int velo) {
            interf.massUpdateLEDsRectangle(0, 0, 8, 8, velo);
            DrawMenuKeys();
        }

        private void KeyUp(object source, LaunchpadKeyEventArgs e) {
            int x, y;
            x = e.GetX(); y = e.GetY();
            switch (State) {
                case GameState.Menu:
                    var menu = KeyInMenuObject(x, y);
                    switch (menu) {
                        case MenuKey.LeftArrow:
                            _lArrowHeld = false;
                            break;                            
                        case MenuKey.RightArrow:
                            _rArrowHeld = false;
                            break;
                    }
                    break;
                case GameState.Game:
                    var pad = Pads[GetCoordFromButton(x, y)];
                    pad.RegisterRelease();
                    break;
            }
        }

        private void KeyDown(object sender, LaunchpadKeyEventArgs e) {
            int x, y;
            x = e.GetX(); y = e.GetY();
            switch (State) {
                case GameState.Title:
                    if (KeyInMenuObject(x, y) == MenuKey.Confim)
                        State = GameState.ReturnToMenu;
                    break;
                case GameState.Menu:
                    var menu = KeyInMenuObject(x, y);
                    switch (menu) {
                        case MenuKey.LeftArrow:
                            if (velo == 0) {
                                velo = 127;
                            } else {
                                velo--;
                            }
                            ChangeMenuColour(velo);
                            _lArrowHeld = true;
                            PaintLArrowHeld();
                            if (menuState == MenuState.SongSelect)
                                SetPrevSong();
                            else if (menuState == MenuState.DifficultySelect)
                                DecreaseDifficulty();
                            break;
                        case MenuKey.RightArrow:
                            if (velo == 127) {
                                velo = 0;
                            } else {
                                velo++;
                            }
                            ChangeMenuColour(velo);
                            _rArrowHeld = true;
                            PaintRArrowHeld();
                            if (menuState == MenuState.SongSelect)
                                SetNextSong();
                            else if (menuState == MenuState.DifficultySelect)
                                IncreaseDifficulty();
                            break;
                        case MenuKey.Confim:
                            if (menuState == MenuState.SongSelect)
                                menuState = MenuState.DifficultySelect;
                            else if (menuState == MenuState.DifficultySelect)
                                State = GameState.Setup;
                            break;
                        case MenuKey.Cancel:
                            if (menuState == MenuState.DifficultySelect)
                                menuState = MenuState.SongSelect;
                            else if (menuState == MenuState.SongSelect)
                                State = GameState.ReturnToTitle;
                            break;
                    }
                    break;
                case GameState.Game:
                    var pad = Pads[GetCoordFromButton(x, y)];
                    pad.RegisterHit();
                    break;
                case GameState.GameOver:
                    if (NewHighScore)
                        State = GameState.HighScoreEntry;
                    else
                        State = GameState.ReturnToMenu;
                    break;
                case GameState.HighScoreEntry:
                    var menuKey = KeyInMenuObject(x, y);
                    switch (menuKey) {
                        case MenuKey.LeftArrow:
                            if (String.IsNullOrWhiteSpace(HighScoreName))
                                HighScoreName = "Z";
                            else {
                                char currChar = HighScoreName[HighScoreName.Length - 1];
                                HighScoreName = HighScoreName.Remove(HighScoreName.Length - 1);
                                HighScoreName = HighScoreName.Insert(HighScoreName.Length, ((char)(currChar - (char)1)).ToString());
                                if (HighScoreName.EndsWith("@")) {
                                    HighScoreName = HighScoreName.Remove(HighScoreName.Length - 1);
                                    HighScoreName = HighScoreName.Insert(HighScoreName.Length, "["); //go to the "END" character
                                }
                            }
                            break;
                        case MenuKey.RightArrow:
                            if (String.IsNullOrWhiteSpace(HighScoreName))
                                HighScoreName = "A";
                            else {
                                char currChar = HighScoreName[HighScoreName.Length - 1];
                                HighScoreName = HighScoreName.Remove(HighScoreName.Length - 1);
                                HighScoreName = HighScoreName.Insert(HighScoreName.Length, ((char)(currChar + (char)1)).ToString());
                                if (HighScoreName.EndsWith("[")) {
                                    //this maps to end
                                } else if (HighScoreName.EndsWith("\\")) {
                                    HighScoreName = HighScoreName.Remove(HighScoreName.Length - 1);
                                    HighScoreName = HighScoreName.Insert(HighScoreName.Length, "A"); //loop
                                }
                            }
                            break;
                        case MenuKey.Confim:
                            if (HighScoreName.EndsWith("[")) {
                                HighScoreName = HighScoreName.Remove(HighScoreName.Length - 1);
                                SaveHighScore();
                                State = GameState.ReturnToMenu;
                            } else {
                                HighScoreName = String.Concat(HighScoreName, "A"); //insert new character.
                            }
                            break;
                        case MenuKey.Cancel:
                            HighScoreName = HighScoreName.Remove(HighScoreName.Length - 1);
                            break;
                    }
                    break;
            }
        }

        private void SaveHighScore() {
            var newHighScore = new Tuple<string, long>(HighScoreName, Score);
            CurrentSong.ScoreList.Scores.Add(newHighScore);
            CurrentSong.ScoreList.Scores = CurrentSong.ScoreList.Scores.OrderByDescending(s => s.Item2).ToList();
            CurrentSong.ScoreList.Scores.RemoveAt(10);
            var saveList = JsonConvert.SerializeObject(CurrentSong.ScoreList);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "Songs", CurrentSong.SongName, CurrentSong.SongName + "_list.js"), saveList);
        }

        private bool ResolveHighScore() {
            bool ret = false;
            foreach (var score in CurrentSong.ScoreList.Scores) {
                if (Score > score.Item2) {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        private void IncreaseDifficulty() {
            SelectedDifficulty = SelectedDifficulty.Next();
        }
        private void DecreaseDifficulty() {
            SelectedDifficulty = SelectedDifficulty.Previous();
        }

        private void SetPrevSong() {
            var idx = Songs.IndexOf(CurrentSong);
            if (idx == 0) {
                idx = Songs.Count -1;
            } else
                idx--;
            SetSong(Songs[idx], true);
        }

        private void SetNextSong() {
            var idx = Songs.IndexOf(CurrentSong);
            if (idx == Songs.Count - 1) {
                idx = 0;
            } else
                idx++;
            SetSong(Songs[idx], true);
        }

        private void PaintRArrowHeld() {
            //right arrow  
            List<int> xs = new List<int>() { 7, 7, 7, 7, 7, 6, 6, 5 };
            List<int> ys = new List<int>() { 3, 4, 5, 6, 7, 6, 7, 7 };            
            interf.massUpdateLEDs(xs, ys, ARROW_HELD_VELO, LightingMode.Pulse);
        }

        private void PaintLArrowHeld() {
            //left arrow
            List<int> xs = new List<int>() { 0, 0, 0, 0, 0, 1, 1, 2 };
            List<int> ys = new List<int>() { 0, 1, 2, 3, 4, 0, 1, 0 };            
            interf.massUpdateLEDs(xs, ys, ARROW_HELD_VELO, LightingMode.Pulse);
        }

        private void DrawLArrow() {
            //left arrow
            List<int> xs = new List<int>() { 0, 0, 0, 0, 0, 1, 1, 2 };
            List<int> ys = new List<int>() { 0, 1, 2, 3, 4, 0, 1, 0 };
            if (_lArrowHeld)
                interf.massUpdateLEDs(xs, ys, ARROW_HELD_VELO, LightingMode.Pulse);
            else
                interf.massUpdateLEDs(xs, ys, ARROW_VELO, LightingMode.Pulse);
        }

        private void DrawRArrow() {
            //right arrow      
            List<int> xs = new List<int>() { 7, 7, 7, 7, 7, 6, 6, 5 };
            List<int> ys = new List<int>() { 3, 4, 5, 6, 7, 6, 7, 7 };
            if (_rArrowHeld)
                interf.massUpdateLEDs(xs, ys, ARROW_HELD_VELO, LightingMode.Pulse);
            else
                interf.massUpdateLEDs(xs, ys, ARROW_VELO, LightingMode.Pulse);
        }

        private void DrawConfirm() {
            //confirm
            if (interf.Connected)
                interf.massUpdateLEDsRectangle(2, 2, 5, 5, CONFIRM_VELO, LightingMode.Pulse);
        }

        private void DrawMenuKeys() {
            if (interf.Connected) {
                DrawLArrow();
                DrawRArrow();
                DrawConfirm();
                interf.massUpdateLEDsRectangle(6, 0, 7, 1, CANCEL_VELO, LightingMode.Pulse);
                interf.massUpdateLEDsRectangle(0, 6, 1, 7, CANCEL_VELO, LightingMode.Pulse);
            }
        }
        public List<Pitch> GetNotesFromButtons(List<Tuple<int, int>> buttons) {
            List<Pitch> result = new List<Pitch>();
            foreach (var coord in buttons) {
                result.Add(interf.ledToMidiNote(coord.Item1, coord.Item2));
            }
            return result;
        }
        public List<Tuple<int, int>> GetButtonsFromCoord(Tuple<int, int> location) {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>() {
                new Tuple<int, int>(location.Item1*2, location.Item2*2),
                new Tuple<int, int>(location.Item1 * 2, (location.Item2*2)+1),
                new Tuple<int, int>((location.Item1*2)+1, location.Item2*2),
                new Tuple<int, int>((location.Item1*2)+1, (location.Item2*2)+1) };
            return result;
        }

        public Tuple<int, int> GetCoordFromButton(int x, int y) {
            return new Tuple<int, int>(x / 2, y / 2);
        }

        public void MainLoop() {
            switch (State) {
                case GameState.Init:
                    songZips = Directory.GetFiles($"Songs", "*.js", SearchOption.AllDirectories).Where(f => !f.EndsWith("_list.js")).ToArray();
                    foreach (var songFile in songZips) {
                        var json = File.ReadAllText(songFile);
                        var song = JsonConvert.DeserializeObject<Song>(json);
                        if (File.Exists(songFile.Replace(".js", "_list.js"))) {
                            var tempScoreList = JsonConvert.DeserializeObject<ScoreList>(File.ReadAllText(songFile.Replace(".js", "_list.js")));
                            tempScoreList.Scores = tempScoreList.Scores.OrderByDescending(s => s.Item2).ToList();
                            song.ScoreList = tempScoreList;
                        } else {
                            var tempScoreList = new ScoreList();
                            for (var i = 0; i < 10; i++) {
                                tempScoreList.Scores.Add(new Tuple<string, long>("AAA", 0));
                            }
                            song.ScoreList = tempScoreList;
                            File.WriteAllText(songFile.Replace(".js", "_list.js"), JsonConvert.SerializeObject(tempScoreList));
                        }
                        Songs.Add(song);
                    };
                    CreatePads();
                    if (interf.Connected) {
                        interf.clearAllLEDs();
                        State = GameState.Title;
                    } else
                        State = GameState.AwaitingLaunchpad;
                    //SetSong(Songs.First(), true);                    
                    break;
                case GameState.AwaitingLaunchpad:
                    //this will never actually get out of here because the connected devices doesn't update until a restart.
                    //but it's a useful holding pattern.
                    var connected = interf.getConnectedLaunchpads();
                    if (connected.Count() > 0) {
                        if (interf.connect(connected[0])) {
                            interf.OnLaunchpadKeyDown += KeyDown;
                            interf.OnLaunchpadKeyUp += KeyUp;
                            interf.OnLaunchpadCCKeyDown += CcKeyDown;
                            interf.OnLaunchpadCCKeyUp += CcKeyUp;
                            interf.clearAllLEDs();
                            State = GameState.Title;
                        }
                    }
                    break;
                case GameState.ReturnToTitle:
                    StopSong();
                    State = GameState.Title;
                    break;
                case GameState.Title:
                    DrawConfirm();
                    break;
                case GameState.ReturnToMenu:
                    SetSong(Songs.First(), true);
                    State = GameState.Menu;
                    menuState = MenuState.SongSelect;
                    break;
                case GameState.Menu:
                    DrawMenuKeys();
                    break;
                case GameState.Setup:
                    StopSong();
                    interf.clearAllLEDs();                    
                    if (SelectedDifficulty == Difficulty.Easy)
                        Beats.AddRange(CurrentSong.EasyBeats);
                    else if (SelectedDifficulty == Difficulty.Advanced)
                        Beats.AddRange(CurrentSong.AdvancedBeats);
                    else if (SelectedDifficulty == Difficulty.Expert)
                        Beats.AddRange(CurrentSong.ExpertBeats);
                    foreach (var coord in Pads.Keys) {
                        var pad = Pads[coord];
                        pad.UpcomingBeats = Beats.Where(b => b.x == coord.Item1 && b.y == coord.Item2).ToList<Beat>();
                    }
                    Combo = 0;
                    MaxCombo = 0;
                    Score = 0;
                    NewHighScore = false;
                    SetSong(CurrentSong);  
                    State = GameState.Game;
                    break;
                case GameState.Game:
                    if (audioFile == null || audioFile.CurrentTime == null)
                        Elapsed = 0;
                    else
                        Elapsed = Convert.ToInt64(audioFile.CurrentTime.TotalMilliseconds);
                    bool moreBeats = false;
                    foreach (var pad in Pads.Values) {                        
                        moreBeats = pad.CheckBeats() || moreBeats;
                        if (pad.CurrentBeat == null) {
                            pad.LightPad(CurrentComboVelo);
                        } else {
                            //get the timer we were using tae fuck
                            pad.UpdateBeat();
                        }
                    }
                    if (!moreBeats || audioFile.Position >= audioFile.Length) {
                        State = GameState.GameEnding;
                        if (CurrentSong.EndTime > 0) {
                            if (songVolumeTimer != null) {
                                songVolumeTimer.Enabled = false;
                                songVolumeTimer.Dispose();
                            }
                            songVolumeTimer = new Timer(100);
                            songVolumeTimer.Elapsed += SongVolumeTimer_Elapsed;
                            songVolumeTimer.AutoReset = true;
                            songVolumeTimer.Enabled = true;
                        }
                    }
                    break;
                case GameState.GameEnding:
                    foreach (var pad in Pads.Values)
                        pad.LightPad(CurrentComboVelo);
                    break;
                case GameState.HighScoreEntry:
                    menuState = MenuState.NameEntry;
                    DrawMenuKeys();
                    break;                
                case GameState.GameOver:
                    foreach (var pad in Pads.Values)
                        pad.LightPad(CurrentComboVelo);
                    break;
            }
        }

        public void CreatePads() {
            if (!Pads.Any()) {
                int velo = 33;
                for (int x = 0; x < 4; x++) {
                    for (int y = 0; y < 4; y++) {
                        var pad = new Pad(new Tuple<int, int>(x, y), this);
                        Pads.Add(new Tuple<int, int>(x, y), pad);
                        pad.MapperVelo = velo;
                        velo += 3;
                    }
                }
            }
        }

        public GameController(bool FromMapper = false) {
            interf = new Interface();
            velo = 0;
            Separation = 250;
            HalfSep = Separation / 2; //save calculations later.            
            if (!FromMapper) { //if we're created from the mapper, the mapper is managing the launchpad interface.
                hitSound.Load();
                var connected = interf.getConnectedLaunchpads();
                if (connected.Count() > 0) {
                    if (interf.connect(connected[0])) {
                        interf.OnLaunchpadKeyDown += KeyDown;
                        interf.OnLaunchpadKeyUp += KeyUp;
                        interf.OnLaunchpadCCKeyDown += CcKeyDown;
                        interf.OnLaunchpadCCKeyUp += CcKeyUp;
                        interf.clearAllLEDs();
                    }
                }
                songZips = Directory.GetFiles($"Songs", "*.trk");
                foreach (var SongFile in songZips) {
                    var SongName = Path.GetFileNameWithoutExtension(SongFile);
                    var SongFolder = Path.Combine(Path.GetDirectoryName(SongFile), SongName);
                    if (Directory.Exists(SongFolder))
                        Directory.Delete(SongFolder, true);
                    ZipFile.ExtractToDirectory(SongFile, SongFolder);
                    if (File.Exists(Path.ChangeExtension(SongFile, ".arc")))
                        File.Delete(Path.ChangeExtension(SongFile, ".arc"));
                    File.Move(SongFile, Path.ChangeExtension(SongFile, ".arc"));
                };
                foreach (var archiveSong in Directory.GetFiles($"Songs", "*.arc")) {
                    if (File.GetLastAccessTime(archiveSong) < DateTime.Now.AddDays(-7)) {
                        File.Delete(archiveSong);//clean up after ourselves after a suitable length of time.
                    }
                }

            }            
        }        

        private void CcKeyUp(object source, LaunchpadCCKeyEventArgs e) {
            
        }

        private void CcKeyDown(object source, LaunchpadCCKeyEventArgs e) {
            
        }

        private void StopSong() {
            if (outputDevice != null) {
                outputDevice.PlaybackStopped -= OutputDevice_PlaybackStopped; //don't fire the event if we're stopping ourselves.
                outputDevice.Stop();
                outputDevice.Dispose();
            }
            if (audioFile != null) {
                audioFile.CurrentTime = TimeSpan.FromTicks(0);
                try {
                    audioFile.Dispose();
                } catch (Exception) {
                    // don't die if the dispose races - it still succeeds.
                }
            }
        }

        private void SetSong(Song newSong, bool preview = false) {
            if (songVolumeTimer != null) {
                songVolumeTimer.Enabled = false;
                songVolumeTimer.Dispose();
            }
            StopSong();
            CurrentSong = newSong;
            interf.setClock(CurrentSong.BPM);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Songs", CurrentSong.SongName, CurrentSong.FileName);
            audioFile = new MediaFoundationReader(path);            
            outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Volume = 1;
            if (preview && CurrentSong.PreviewStart > 0 && CurrentSong.PreviewEnd > 0) {
                audioFile.CurrentTime = TimeSpan.FromMilliseconds(CurrentSong.PreviewStart);
                outputDevice.Volume = 0;
                reachedPreviewEnd = false;
                songVolumeTimer = new Timer(100);
                songVolumeTimer.Elapsed += PreviewVolumeTimer_Elapsed;
                songVolumeTimer.AutoReset = true;
                songVolumeTimer.Enabled = true;
            }
            outputDevice.Play();            
            outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped;
        }

        private void SongVolumeTimer_Elapsed(object sender, ElapsedEventArgs e) {
            if (State == GameState.GameEnding && audioFile != null && outputDevice != null) {
                if (audioFile.CurrentTime >= TimeSpan.FromMilliseconds(CurrentSong.EndTime)) {
                    if (outputDevice.Volume >= 0.01)
                        outputDevice.Volume -= 0.01f;
                    else {
                        State = GameState.GameOver;
                        songVolumeTimer.AutoReset = false;
                        songVolumeTimer.Enabled = false;
                        NewHighScore = ResolveHighScore();
                    }
                }
            } else {
                //we're done here.
                songVolumeTimer.AutoReset = false;
                songVolumeTimer.Enabled = false;
            }
        }

        private void PreviewVolumeTimer_Elapsed(object sender, ElapsedEventArgs e) {
            if (State == GameState.Menu && audioFile != null && outputDevice != null) {
                if (!reachedPreviewEnd) {
                    if (outputDevice.Volume < 0.99)
                        outputDevice.Volume += 0.01f;
                    reachedPreviewEnd = audioFile.CurrentTime >= TimeSpan.FromMilliseconds(CurrentSong.PreviewEnd);
                } else {
                    if (outputDevice.Volume >= 0.01)
                        outputDevice.Volume -= 0.01f;
                    else {
                        audioFile.CurrentTime = TimeSpan.FromMilliseconds(CurrentSong.PreviewStart);
                        reachedPreviewEnd = false;
                    }
                }
            } else {
                //we're done in the menu, get outta here.
                songVolumeTimer.AutoReset = false;
                songVolumeTimer.Enabled = false;
            }
        }

        private void OutputDevice_PlaybackStopped(object sender, StoppedEventArgs e) {
            outputDevice.Play();
        }
    }
}
