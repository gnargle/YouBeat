using LaunchpadNET;
using Midi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Threading;

namespace YouBeatTypes
{
    public class Pad {
        public enum ScoreVelo { Miss = 0, Bad = 5, OK = 9, Good = 13, Great = 21, Perfect = 29 }
        public enum Timing { None, Early, Late }
        public Tuple<int, int> Location { get; set; }
        public List<Tuple<int, int>> Buttons { get; set; }
        public List<Pitch> Notes { get; set; } = new List<Pitch>();
        public List<Beat> UpcomingBeats { get; set; }
        public List<Beat> PastBeats { get; set; } = new List<Beat>();
        public Beat CurrentBeat { get; set; }
        public int MapperVelo { get; set; }
        public bool MapperPressed { get; set; } = false;
        private Interface _interf;
        private GameController _controller;
        private System.Timers.Timer _timer;
        private Timing _timing;
        private ScoreVelo _currentVelo;

        public void SetupBeat() {
            CurrentBeat = UpcomingBeats.First();
            UpcomingBeats.Remove(CurrentBeat);
            _timing = Timing.Early;
            _currentVelo = ScoreVelo.Bad;
            LightPad((int)_currentVelo);
        }

        public void LightPad(int velo) {
            int xmin, xmax, ymin, ymax;
            xmin = Buttons.Min(c => c.Item1);
            xmax = Buttons.Max(c => c.Item1);
            ymin = Buttons.Min(c => c.Item2);
            ymax = Buttons.Max(c => c.Item2);
            _interf.massUpdateLEDsRectangle(xmin, ymin, xmax, ymax, velo, LightingMode.Pulse);
        }

        public Pad(Tuple<int, int> location, GameController controller) {
            Location = location;
            _controller = controller;
            _interf = controller.interf;
            Buttons = controller.GetButtonsFromCoord(Location);
            Notes = controller.GetNotesFromButtons(Buttons);
            _timer = new System.Timers.Timer(_controller.Separation);
        }

        public Pad(Tuple<int, int> location, GameController controller, Interface interf) {
            Location = location;
            _controller = controller;
            _interf = interf;
            Buttons = controller.GetButtonsFromCoord(Location);
            Notes = controller.GetNotesFromButtons(Buttons);
            _timer = new System.Timers.Timer(_controller.Separation);
            _timer.Elapsed += NoteOffTimerElapsed;
        }

        private void ClearPad() {
            LightPad(0);
        }

        public void RegisterRelease() {
            ClearPad();
        }

        public void RegisterHit() {
            _timer.Enabled = false;
            var time = _controller.Elapsed;            
            LightPad(17);
            if (CurrentBeat != null) {
                var denom = CurrentBeat.HitTime - time;
                if (denom == 0) denom = 1; //if you someone manage to match the note exactly, get the full amount
                var score = Convert.ToInt64(Math.Abs(2500-denom*.01*(_controller.Separation * 5)));
                //scoring is STILL wrong, it gives you more points for being off rhythm
                switch (_currentVelo) {
                    case ScoreVelo.Bad:
                    case ScoreVelo.OK:
                        _controller.UpdateCombo(ComboChange.Break);
                        break;
                    default:
                        _controller.UpdateCombo(ComboChange.Add);
                        break;
                }
                _controller.AddToScore(score);
                PastBeats.Add(CurrentBeat);
                CurrentBeat = null;
            }
            _timer.AutoReset = false;
            _timer.Enabled = true;
        }

        private void ResetTimer() {
            if (_currentVelo == ScoreVelo.Perfect)
                _timer.Interval = _controller.Separation / 2;
            else
                _timer.Interval = _controller.Separation;
            _timer.Enabled = true;
        }

        private void NoteOffTimerElapsed(object sender, ElapsedEventArgs e) {
            ClearPad();
        }
        /*
        private void ScoreTimerElapsed(object sender, ElapsedEventArgs e) {
            if (CurrentBeat != null) {
                if (_timing == Timing.Early && CurrentBeat.HitTime < _controller.Elapsed)
                    _timing = Timing.Late;
                if (_timing == Timing.Early) {
                    switch (_currentVelo) {
                        case ScoreVelo.Bad:
                            _currentVelo = ScoreVelo.OK;
                            LightPad((int)_currentVelo);
                            ResetTimer();
                            break;
                        case ScoreVelo.OK:
                            _currentVelo = ScoreVelo.Good;
                            LightPad((int)_currentVelo);
                            ResetTimer();
                            break;
                        case ScoreVelo.Good:
                            _currentVelo = ScoreVelo.Great;
                            LightPad((int)_currentVelo);
                            ResetTimer();
                            break;
                        case ScoreVelo.Great:
                            _currentVelo = ScoreVelo.Perfect;
                            LightPad((int)_currentVelo);
                            ResetTimer();
                            break;
                        case ScoreVelo.Perfect:
                            LightPad((int)_currentVelo);
                            ResetTimer();
                            break;
                    }
                } else {
                    switch (_currentVelo) {
                        case ScoreVelo.Perfect:
                            if (_controller.Elapsed > CurrentBeat.HitTime + _controller.Separation) {
                                _currentVelo = ScoreVelo.Great;
                                LightPad((int)_currentVelo);
                            }
                            ResetTimer();
                            break;
                        case ScoreVelo.Great:
                            _currentVelo = ScoreVelo.Good;
                            LightPad((int)_currentVelo);
                            ResetTimer();
                            break;
                        case ScoreVelo.Good:
                            _currentVelo = ScoreVelo.OK;
                            LightPad((int)_currentVelo);
                            ResetTimer();
                            break;
                        case ScoreVelo.OK:
                            _currentVelo = ScoreVelo.Bad;
                            LightPad((int)_currentVelo);
                            ResetTimer();
                            break;
                        case ScoreVelo.Bad:
                            _currentVelo = ScoreVelo.Miss;
                            ClearPad();
                            PastBeats.Add(CurrentBeat);
                            _controller.UpdateCombo(ComboChange.Break);
                            CurrentBeat = null;
                            break;
                    }
                }
            }
        }
        */

        public void UpdateBeat() {
            if (CurrentBeat != null) {
                var time = _controller.Elapsed;
                if (_timing == Timing.Early && CurrentBeat.HitTime < time)
                    _timing = Timing.Late;
                if (_timing == Timing.Early) {
                    //each stage needs to display for 25ms, or 250 (the separation)
                    //perfect should be 125ms each side of the actual time, so:
                    //Great = hittime +- 375
                    if (time < CurrentBeat.HitTime - (_controller.HalfSep + _controller.Separation * 4))
                        _currentVelo = ScoreVelo.Bad;
                    else if (time < CurrentBeat.HitTime - (_controller.HalfSep + _controller.Separation * 3))
                        _currentVelo = ScoreVelo.OK;
                    else if (time < CurrentBeat.HitTime - (_controller.HalfSep + _controller.Separation * 2))
                        _currentVelo = ScoreVelo.Good;
                    else if (time < CurrentBeat.HitTime - (_controller.HalfSep + _controller.Separation))
                        _currentVelo = ScoreVelo.Great;
                    else if (time < CurrentBeat.HitTime - _controller.HalfSep)
                        _currentVelo = ScoreVelo.Perfect;
                } else {
                    if (time > CurrentBeat.HitTime + _controller.Separation * 5) {
                        _currentVelo = ScoreVelo.Miss;
                        ClearPad();
                        PastBeats.Add(CurrentBeat);
                        _controller.UpdateCombo(ComboChange.Break);
                        CurrentBeat = null;
                    } else if (time > CurrentBeat.HitTime + (_controller.HalfSep + _controller.Separation * 4))
                        _currentVelo = ScoreVelo.Bad;
                    else if (time > CurrentBeat.HitTime + (_controller.HalfSep + _controller.Separation * 3))
                        _currentVelo = ScoreVelo.OK;
                    else if (time > CurrentBeat.HitTime + (_controller.HalfSep + _controller.Separation * 2))
                        _currentVelo = ScoreVelo.Good;
                    else if (time > CurrentBeat.HitTime + (_controller.HalfSep + _controller.Separation))
                        _currentVelo = ScoreVelo.Great;
                }
                LightPad((int)_currentVelo);
            }
        }
        

        public bool CheckBeats() {
            if (CurrentBeat != null) {
                return true;
            } else if (UpcomingBeats == null || !UpcomingBeats.Any()) {
                return false;
            }
            if ((_controller.Elapsed > 0)  && (UpcomingBeats.First().HitTime <= _controller.Elapsed + _controller.Separation * 5)) {
                new Thread(delegate () {
                    SetupBeat();
                }).Start();
            }            
            return true;
        }
    }
}
