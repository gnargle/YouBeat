using LaunchpadNET;
using Midi.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.IO.Compression;
using System.Xml.Serialization;
using static LaunchpadNET.Interface;
using Newtonsoft.Json;
using NAudio.Wave;

namespace YouBeatTypes {    
    public class GameController {
        private string[] songManifests;
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

        public Stopwatch GlobalStopwatch { get; set; }
        public long Elapsed { get; set; }
        public long Separation { get; set; }        
        public GameState state = GameState.Init;
        public Dictionary<Tuple<int, int>, Pad> Pads = new Dictionary<Tuple<int, int>, Pad>();
        public List<Beat> Beats = new List<Beat>();
        
        public Song CurrentSong { get; set; }
        public long Score { get; set; }     
        public long Combo { get; set; }
        public int CurrentComboVelo { get; set; } = 0;

        private object ComboLock = new object();
        private object ScoreLock = new object();

        private Timer _leadInTimer;

        public void AddToScore(long moreScore) {
            lock (ScoreLock) {
                Score += moreScore;
            }
        }

        public void UpdateCombo (ComboChange change) {
            lock (ComboLock) {
                switch (change) {
                    case ComboChange.Add:
                        Combo++;
                        break;
                    case ComboChange.Break:
                        Combo = 0;
                        break;
                }
                if (Combo == 0) CurrentComboVelo = 0;
                else {
                    var veloDecider = Beats.Count / Combo;
                    switch (veloDecider) {
                        case 3:
                            CurrentComboVelo = 19;
                            break;
                        case 2:
                            CurrentComboVelo = 43;
                            break;
                        case 1:
                            CurrentComboVelo = 59;
                            break;
                        case 0:
                            CurrentComboVelo = 72;
                            break;
                        default:
                            CurrentComboVelo = 0;
                            break;
                    }
                }
            }
        }
        
        private MenuKey keyInMenuObject(int x, int y) {
            var leftArrow = new List<Pitch>() { Pitch.A5, Pitch.ASharp5, Pitch.B5, Pitch.C6, Pitch.CSharp6, Pitch.B4, Pitch.C5, Pitch.CSharp4 };
            var rightArrow = new List<Pitch>() { Pitch.D0, Pitch.DSharp0, Pitch.E0, Pitch.F0, Pitch.FSharp0, Pitch.DSharp1, Pitch.E1, Pitch.D2 };
            var confirm = new List<Pitch>() { Pitch.A1, Pitch.ASharp1, Pitch.B1, Pitch.C2,Pitch.G2, Pitch.GSharp2, Pitch.A2, Pitch.ASharp2,
                                                   Pitch.F3, Pitch.FSharp3, Pitch.G3, Pitch.GSharp3, Pitch.DSharp4, Pitch.E4, Pitch.F4, Pitch.FSharp4};
            var note = interf.ledToMidiNote(x, y);
            if (leftArrow.Contains(note)) {
                return MenuKey.LeftArrow;
            } else if (rightArrow.Contains(note)) {
                return MenuKey.RightArrow;
            } else if (confirm.Contains(note)) {
                return MenuKey.Confim;
            }
            return MenuKey.None;
        }

        private void changeMenuColour(int velo) {
            interf.massUpdateLEDsRectangle(0, 0, 8, 8, velo);
            drawMenuKeys();
        }

        private void keyUp(object source, LaunchpadKeyEventArgs e) {
            int x, y;
            x = e.GetX(); y = e.GetY();
            switch (state) {
                case GameState.Menu:
                    var menu = keyInMenuObject(x, y);
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

        private void keyDown(object sender, LaunchpadKeyEventArgs e) {
            int x, y;
            x = e.GetX(); y = e.GetY();
            switch (state) {
                case GameState.Menu:
                    var menu = keyInMenuObject(x, y);
                    switch (menu) {
                        case MenuKey.LeftArrow:
                            if (velo == 0) {
                                velo = 127;
                            } else {
                                velo--;
                            }
                            changeMenuColour(velo);
                            _lArrowHeld = true;
                            paintLArrowHeld();
                            SetPrevSong();
                            break;
                        case MenuKey.RightArrow:
                            if (velo == 127) {
                                velo = 0;
                            } else {
                                velo++;
                            }
                            changeMenuColour(velo);
                            _rArrowHeld = true;
                            paintRArrowHeld();                            
                            SetNextSong();
                            break;
                        case MenuKey.Confim:
                            state = GameState.Setup;
                            break;
                    }
                    break;
                case GameState.Game:
                    var pad = Pads[GetCoordFromButton(x, y)];
                    pad.RegisterHit();
                    break;
            }
        }

        private void SetPrevSong() {
            var idx = Songs.IndexOf(CurrentSong);
            if (idx == 0) {
                idx = Songs.Count -1;
            } else
                idx--;
            SetSong(Songs[idx]);
        }

        private void SetNextSong() {
            var idx = Songs.IndexOf(CurrentSong);
            if (idx == Songs.Count - 1) {
                idx = 0;
            } else
                idx++;
            SetSong(Songs[idx]);
        }

        private void paintRArrowHeld() {
            //right arrow  
            List<int> xs = new List<int>() { 7, 7, 7, 7, 7, 6, 6, 5 };
            List<int> ys = new List<int>() { 3, 4, 5, 6, 7, 6, 7, 7 };            
            interf.massUpdateLEDs(xs, ys, ARROW_HELD_VELO, LightingMode.Pulse);
        }

        private void paintLArrowHeld() {
            //left arrow
            List<int> xs = new List<int>() { 0, 0, 0, 0, 0, 1, 1, 2 };
            List<int> ys = new List<int>() { 0, 1, 2, 3, 4, 0, 1, 0 };            
            interf.massUpdateLEDs(xs, ys, ARROW_HELD_VELO, LightingMode.Pulse);
        }

        private void drawMenuKeys() {
            //left arrow
            List<int> xs = new List<int>() { 0, 0, 0, 0, 0, 1, 1, 2 };
            List<int> ys = new List<int>() { 0, 1, 2, 3, 4, 0, 1, 0 };            
            if (_lArrowHeld)
                interf.massUpdateLEDs(xs, ys, ARROW_HELD_VELO, LightingMode.Pulse);     
            else
                interf.massUpdateLEDs(xs, ys, ARROW_VELO, LightingMode.Pulse);
            //right arrow      
            xs = new List<int>() { 7, 7, 7, 7, 7, 6, 6, 5 }; 
            ys = new List<int>() { 3, 4, 5, 6, 7, 6, 7, 7 };            
            if (_rArrowHeld)
                interf.massUpdateLEDs(xs, ys, ARROW_HELD_VELO, LightingMode.Pulse);
            else
                interf.massUpdateLEDs(xs, ys, ARROW_VELO, LightingMode.Pulse);
            //confirm
            interf.massUpdateLEDsRectangle(2, 2, 5, 5, CONFIRM_VELO, LightingMode.Pulse);
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
            switch (state) {
                case GameState.Init:
                    songManifests = Directory.GetFiles($"Songs", "*.js");
                    foreach (var songFile in songManifests) {
                        var json = File.ReadAllText(songFile);
                        var song = JsonConvert.DeserializeObject<Song>(json);
                        Songs.Add(song);
                    };
                    SetSong(Songs.First());
                    state = GameState.Menu;
                    break;
                case GameState.Menu:
                    drawMenuKeys();

                    break;
                case GameState.Setup:
                    StopSong();
                    interf.clearAllLEDs();
                    CreatePads();
                    Beats.AddRange(CurrentSong.Beats);
                    foreach (var coord in Pads.Keys) {
                        var pad = Pads[coord];
                        pad.UpcomingBeats = Beats.Where(b => b.x == coord.Item1 && b.y == coord.Item2).ToList<Beat>();
                    }
                    GlobalStopwatch = new Stopwatch();
                    _leadInTimer = new Timer(5500) {
                        AutoReset = false                        
                    };
                    _leadInTimer.Elapsed += _leadInTimer_Elapsed;
                    _leadInTimer.Start();
                    GlobalStopwatch.Start();                    
                    state = GameState.Game;
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
                        }
                    }
                    if (!moreBeats) {
                        state = GameState.GameEnding;
                    }
                    break;
                case GameState.GameEnding:
                    foreach (var pad in Pads.Values)
                        pad.LightPad(CurrentComboVelo);
                    state = GameState.GameOver;
                    break;
                case GameState.GameOver:
                    foreach (var pad in Pads.Values)
                        pad.LightPad(CurrentComboVelo);
                    break;
            }
        }

        private void _leadInTimer_Elapsed(object sender, ElapsedEventArgs e) {
            SetSong(CurrentSong);
        }

        public void CreatePads() {
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

        public GameController(bool FromMapper = false) {
            interf = new Interface();
            velo = 0;
            Separation = 250;
            if (!FromMapper) { //if we're created from the mapper, the mapper is managing the launchpad interface.
                var connected = interf.getConnectedLaunchpads();
                if (connected.Count() > 0) {
                    if (interf.connect(connected[0])) {
                        interf.OnLaunchpadKeyDown += keyDown;
                        interf.OnLaunchpadKeyUp += keyUp;
                        interf.OnLaunchpadCCKeyDown += ccKeyDown;
                        interf.OnLaunchpadCCKeyUp += ccKeyUp;
                        interf.clearAllLEDs();
                    }
                }
                songManifests = Directory.GetFiles($"Songs", "*.trk");
                foreach (var SongFile in songManifests) {
                    var SongName = Path.GetFileNameWithoutExtension(SongFile);
                    var SongFolder = Path.Combine(Path.GetDirectoryName(SongFile), SongName);
                    if (Directory.Exists(SongFolder))
                        Directory.Delete(SongFolder, true);
                    ZipFile.ExtractToDirectory(SongFile, SongFolder);
                    var json = File.ReadAllText($@"{SongFolder}\{SongName}.js");
                    var song = JsonConvert.DeserializeObject<Song>(json);
                    Songs.Add(song);
                };
            }            
        }        

        private void ccKeyUp(object source, LaunchpadCCKeyEventArgs e) {
            
        }

        private void ccKeyDown(object source, LaunchpadCCKeyEventArgs e) {
            
        }

        private void StopSong() {
            if (outputDevice != null) {
                outputDevice.PlaybackStopped -= OutputDevice_PlaybackStopped; //don't fire the event if we're stopping ourselves.
                outputDevice.Stop();
                outputDevice.Dispose();
            }
            if (audioFile != null) {
                audioFile.CurrentTime = TimeSpan.FromTicks(0);
                audioFile.Dispose();
            }
        }

        private void SetSong(Song newSong) {
            StopSong();
            CurrentSong = newSong;
            interf.setClock(CurrentSong.BPM);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Songs", CurrentSong.SongName, CurrentSong.FileName);
            audioFile = new MediaFoundationReader(path);
            outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();
            outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped;
        }

        private void OutputDevice_PlaybackStopped(object sender, StoppedEventArgs e) {
            outputDevice.Play();
        }
    }
}
