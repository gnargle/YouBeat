﻿using LaunchpadNET;
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
    public delegate void SongChangeHandler(Song newSong, Song prevSong);
    public delegate void HitRegHandler(ScoreVelo score);
    public delegate void ChangeMenuStateHandler(MenuState newMenuState, MenuState prevMenuState);
    public delegate void SetDifficultyHandler(Difficulty difficulty);
    public class GameController {
        private Song _currentSong;
        private string[] songZips;
        public List<Song> Songs { get; set; } = new List<Song>();
        private MediaFoundationReader audioFile;
        private WaveOutEvent outputDevice;

        private bool _lArrowHeld = false;
        private bool _rArrowHeld = false;

        private ControllerCreator _controllerCreator;

        private object ComboLock = new object();
        private object ScoreLock = new object();
        private object CountLock = new object();

        private Timer songVolumeTimer;
        private bool reachedPreviewEnd = false;

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
        public MenuState MenuState { get; set; } = MenuState.SongSelect;
        public Difficulty SelectedDifficulty { get; set; } = Difficulty.Easy;
        public Dictionary<Tuple<int, int>, Pad> Pads = new Dictionary<Tuple<int, int>, Pad>();
        public List<Beat> Beats = new List<Beat>();
        public SongChangeHandler OnSongChange { get; set; }
        public HitRegHandler OnHitReg { get; set; }
        public ChangeMenuStateHandler OnMenuStateChange { get; set; }
        public SetDifficultyHandler OnSetDifficulty { get; set; }
        public Song CurrentSong { 
            get { return _currentSong; } 
            set {
                var prevSong = _currentSong;
                _currentSong = value;
                OnSongChange?.Invoke(_currentSong, prevSong);
            } }
        public long Score { get; set; }     
        public long Combo { get; set; }
        public long MaxCombo { get; set; }
        public long TotalBeats { get { return (long)Beats?.Count; } }
        public int CurrentComboVelo { get; set; } = 0;
        public bool NewHighScore { get; set; } = false;
        public string HighScoreName { get; set; } = String.Empty;
        public bool LoopActive { get; set; } = true;
        public bool SongSelectActive { get; set; } = true;
        public bool AcceptInput { get; set; } = true;
        public int Misses { get; private set; } = 0;
        public int Bads { get; private set; } = 0;
        public int OKs { get; private set; } = 0;
        public int Goods { get; set; } = 0;
        public int Greats { get; set; } = 0;
        public int Perfects { get; set; } = 0;

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

        private void ResetCounts() {
            Misses = 0; Bads = 0; OKs = 0; Goods = 0; Greats = 0; Perfects = 0;
        }

        public void AddToHits(ScoreVelo scoreVelo) {
            lock (CountLock) {
                switch (scoreVelo) {
                    case ScoreVelo.Miss:
                        Misses++;
                        break;
                    case ScoreVelo.Bad:
                        Bads++;
                        break;
                    case ScoreVelo.OK:
                        OKs++;
                        break;
                    case ScoreVelo.Good:
                        Goods++;
                        break;
                    case ScoreVelo.Great:
                        Greats++;
                        break;
                    case ScoreVelo.Perfect:
                        Perfects++;
                        break;
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
            if (!AcceptInput)
                return;
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
            if (!AcceptInput)
                return;
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
                            if (MenuState == MenuState.SongSelect && SongSelectActive)
                                SetPrevSong();
                            else if (MenuState == MenuState.DifficultySelect)
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
                            if (MenuState == MenuState.SongSelect && SongSelectActive)
                                SetNextSong();
                            else if (MenuState == MenuState.DifficultySelect)
                                IncreaseDifficulty();
                            break;
                        case MenuKey.Confim:
                            if (MenuState == MenuState.SongSelect) {
                                MenuState = MenuState.DifficultySelect;
                                OnMenuStateChange?.Invoke(MenuState, MenuState.SongSelect);
                                SetDefaultDifficulty();
                            } else if (MenuState == MenuState.DifficultySelect)
                                State = GameState.Setup;
                            break;
                        case MenuKey.Cancel:
                            if (MenuState == MenuState.DifficultySelect) {
                                MenuState = MenuState.SongSelect;
                                OnMenuStateChange?.Invoke(MenuState, MenuState.DifficultySelect);
                            } else if (MenuState == MenuState.SongSelect)
                                State = GameState.ReturnToTitle;
                            break;
                    }
                    break;
                case GameState.Game:
                    var pad = Pads[GetCoordFromButton(x, y)];
                    pad.RegisterHit();
                    break;
                case GameState.GameOver:
                    NewHighScore = ResolveHighScore();
                    if (NewHighScore)
                        State = GameState.HighScoreEntryHold;
                    else {
                        SaveCombo();
                        State = GameState.ReturnToMenuHold;
                    }
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
                                State = GameState.ReturnToMenuHold;
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

        private void SaveCombo() {
            if (MaxCombo > CurrentSong.ScoreList.MaxCombo) {
                CurrentSong.ScoreList.MaxCombo = MaxCombo;
                var saveList = JsonConvert.SerializeObject(CurrentSong.ScoreList);
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "Songs", CurrentSong.SongName, CurrentSong.SongName + "_list.js"), saveList);
            }
        }

        private void SaveHighScore() {
            var newHighScore = new Tuple<string, long>(HighScoreName, Score);
            CurrentSong.ScoreList.Scores.Add(newHighScore);
            CurrentSong.ScoreList.Scores = CurrentSong.ScoreList.Scores.OrderByDescending(s => s.Item2).ToList();
            CurrentSong.ScoreList.Scores.RemoveAt(10);
            if (MaxCombo > CurrentSong.ScoreList.MaxCombo)
                CurrentSong.ScoreList.MaxCombo = MaxCombo;
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

        private void SetDefaultDifficulty() {
            bool change = false;
            switch (SelectedDifficulty) {
                //start by checking the currently selected difficulty. If this difficulty has beats, we can jsut ret.
                case Difficulty.Easy:
                    if (!_currentSong.EasyBeats.Any())
                        change = true;                       
                    break;
                case Difficulty.Advanced:
                    if (!_currentSong.AdvancedBeats.Any())
                        change = true;                    
                    break;
                case Difficulty.Expert:
                    if (!_currentSong.ExpertBeats.Any())
                        change = true;
                    break;
            }
            //if we get here, we need to switch to a difficulty with beats.
            if (change) {
                if (_currentSong.EasyBeats.Any())
                    SelectedDifficulty = Difficulty.Easy;
                else if (_currentSong.AdvancedBeats.Any())
                    SelectedDifficulty = Difficulty.Advanced;
                else if (_currentSong.ExpertBeats.Any())
                    SelectedDifficulty = Difficulty.Expert;
                else
                    throw new Exception("This song has no valid beatmaps. This will happen if you mess with the setup file. So, don't do that.");
            }
            OnSetDifficulty?.Invoke(SelectedDifficulty);
        }

        private void IncreaseDifficulty() {
            switch (SelectedDifficulty) {
                //start by checking the currently selected difficulty. If this difficulty has beats, we can jsut ret.
                case Difficulty.Easy:
                    if (_currentSong.AdvancedBeats.Any())
                        SelectedDifficulty = Difficulty.Advanced;
                    else if (_currentSong.ExpertBeats.Any())
                        SelectedDifficulty = Difficulty.Expert;
                    else if (_currentSong.EasyBeats.Any())
                        SelectedDifficulty = Difficulty.Easy;
                    else
                        throw new Exception("This song has no valid beatmaps. This will happen if you mess with the setup file. So, don't do that.");
                    break;
                case Difficulty.Advanced:
                    if (_currentSong.ExpertBeats.Any())
                        SelectedDifficulty = Difficulty.Expert;
                    else if (_currentSong.EasyBeats.Any())
                        SelectedDifficulty = Difficulty.Easy;
                    else if (_currentSong.AdvancedBeats.Any())
                        SelectedDifficulty = Difficulty.Advanced;
                    else
                        throw new Exception("This song has no valid beatmaps. This will happen if you mess with the setup file. So, don't do that.");
                    break;
                case Difficulty.Expert:
                    if (_currentSong.EasyBeats.Any())
                        SelectedDifficulty = Difficulty.Easy;
                    else if (_currentSong.AdvancedBeats.Any())
                        SelectedDifficulty = Difficulty.Advanced;
                    else if (_currentSong.ExpertBeats.Any())
                        SelectedDifficulty = Difficulty.Expert;
                    else
                        throw new Exception("This song has no valid beatmaps. This will happen if you mess with the setup file. So, don't do that.");
                    break;
            }
            OnSetDifficulty?.Invoke(SelectedDifficulty);
        }
        private void DecreaseDifficulty() {
            switch (SelectedDifficulty) {
                //start by checking the currently selected difficulty. If this difficulty has beats, we can jsut ret.
                case Difficulty.Easy:
                    if (_currentSong.ExpertBeats.Any())
                        SelectedDifficulty = Difficulty.Expert;
                    else if (_currentSong.AdvancedBeats.Any())
                        SelectedDifficulty = Difficulty.Advanced;
                    else if (_currentSong.EasyBeats.Any())
                        SelectedDifficulty = Difficulty.Easy;
                    else
                        throw new Exception("This song has no valid beatmaps. This will happen if you mess with the setup file. So, don't do that.");
                    break;
                case Difficulty.Advanced:
                    if (_currentSong.EasyBeats.Any())
                        SelectedDifficulty = Difficulty.Easy;
                    else if (_currentSong.ExpertBeats.Any())
                        SelectedDifficulty = Difficulty.Expert;
                    else if (_currentSong.AdvancedBeats.Any())
                        SelectedDifficulty = Difficulty.Advanced;
                    else
                        throw new Exception("This song has no valid beatmaps. This will happen if you mess with the setup file. So, don't do that.");
                    break;
                case Difficulty.Expert:
                    if (_currentSong.AdvancedBeats.Any())
                        SelectedDifficulty = Difficulty.Advanced;
                    else if (_currentSong.EasyBeats.Any())
                        SelectedDifficulty = Difficulty.Easy;
                    else if (_currentSong.ExpertBeats.Any())
                        SelectedDifficulty = Difficulty.Expert;
                    else
                        throw new Exception("This song has no valid beatmaps. This will happen if you mess with the setup file. So, don't do that.");
                    break;
            }
            OnSetDifficulty?.Invoke(SelectedDifficulty);
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

        public void ContinuousLoop() {
            while (LoopActive) {
                MainLoop();
            }
        }

        public void RemoveHold() {
            switch (State) {
                case GameState.PreGameHold:
                    SetSong(CurrentSong);
                    State = GameState.Game;
                    break;
                case GameState.HighScoreEntryHold:
                    State = GameState.HighScoreEntry;
                    break;
                case GameState.ReturnToMenuHold:
                    State = GameState.ReturnToMenu;
                    break;
            }
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
                    MenuState = MenuState.SongSelect;
                    if (_controllerCreator == ControllerCreator.Youbeat)
                        AcceptInput = false;
                    break;
                case GameState.Menu:
                    DrawMenuKeys();
                    break;
                case GameState.Setup:
                    StopSong();
                    interf.clearAllLEDs();            
                    Beats.Clear();
                    ResetCounts();
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
                    if (_controllerCreator != ControllerCreator.Youbeat) {
                        SetSong(CurrentSong);
                        State = GameState.Game;
                    } else
                        State = GameState.PreGameHold;
                    break;
                case GameState.PreGameHold: //we don't really need these, but it's nice for clarity.
                case GameState.HighScoreEntryHold:
                case GameState.ReturnToMenuHold:
                    break; //do nothing - we're waiting for the scene transition before we start.
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
                        } else {
                            State = GameState.GameOver;
                        }
                    }
                    break;
                case GameState.GameEnding:
                    foreach (var pad in Pads.Values)
                        pad.LightPad(CurrentComboVelo);
                    break;
                case GameState.HighScoreEntry:
                    MenuState = MenuState.NameEntry;
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

        ~GameController() {
            StopSong();
            if ((bool)interf?.Connected) {
                interf.SetMode(LaunchpadMode.Live);
            }
        }

        public GameController(ControllerCreator creator) {
            interf = new Interface();
            velo = 0;
            Separation = 250;
            HalfSep = Separation / 2; //save calculations later.    
            _controllerCreator = creator;
            if (creator != ControllerCreator.Mapper) { //if we're created from the mapper, the mapper is managing the launchpad interface.
                var connected = interf.getConnectedLaunchpads();
                if (connected.Count() > 0) {
                    if (interf.connect(connected[0])) {
                        interf.SetMode(LaunchpadMode.Programmer);
                        interf.OnLaunchpadKeyDown += KeyDown;
                        interf.OnLaunchpadKeyUp += KeyUp;
                        interf.OnLaunchpadCCKeyDown += CcKeyDown;
                        interf.OnLaunchpadCCKeyUp += CcKeyUp;
                        interf.clearAllLEDs();
                    }
                }
                if (!Directory.Exists("Songs"))
                    Directory.CreateDirectory("Songs");
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
        
        public String GetImageFilename(Song song) {
            if (string.IsNullOrEmpty(song.ImageFileName))
                return null;
            return Path.Combine(Directory.GetCurrentDirectory(), "Songs", song.SongName, song.ImageFileName);
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
            new System.Threading.Thread(delegate () {
                interf.setClock(CurrentSong.BPM); //this takes fucking AGES because we have to set the clock over a set period of time
            }).Start();            
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
                var endTime = CurrentSong.EndTime > 0 ? TimeSpan.FromMilliseconds(CurrentSong.EndTime) : audioFile.TotalTime;
                if (audioFile.CurrentTime >= endTime) {
                    if (outputDevice.Volume >= 0.01)
                        outputDevice.Volume -= 0.01f;
                    else {
                        State = GameState.GameOver;
                        songVolumeTimer.AutoReset = false;
                        songVolumeTimer.Enabled = false;                        
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
