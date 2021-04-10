using LaunchpadNET;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeatTypes;

namespace YouBeatMapper {
    class MapperLPInterf {

        private GameController controller;
        public Dictionary<Tuple<int, int>, Pad> Pads = new Dictionary<Tuple<int, int>, Pad>();
        private const int PRESSED_VELO = 72;
        private Interface interf;

        public Song CurrentSong { get; set; }
        public List<Beat> CurrentBeats { get; set; }
        public MediaFoundationReader AudioFile { get; set; }
        public WaveOut WvOut { get; set; }
        public const int BEAT_DUR = 250;

        ~MapperLPInterf() {
            if (interf != null && interf.Connected)
                interf.SetMode(LaunchpadMode.Live);
        }

        public MapperLPInterf() {
            controller = new GameController(ControllerCreator.Mapper);
            interf = new Interface();
            var connected = interf.getConnectedLaunchpads();
            if (connected.Count() > 0) {
                if (interf.connect(connected[0])) {
                    interf.OnLaunchpadKeyDown += KeyDown;
                    interf.OnLaunchpadKeyUp += KeyUp;
                    interf.SetMode(LaunchpadMode.Programmer);
                    interf.clearAllLEDs();
                }
                int velo = 33;
                for (int x = 0; x < 4; x++) {
                    for (int y = 0; y < 4; y++) {
                        var pad = new Pad(new Tuple<int, int>(x, y), controller, interf);
                        Pads.Add(new Tuple<int, int>(x, y), pad);
                        pad.MapperVelo = velo;
                        velo += 3;
                    }
                }
                UpdatePads();
            }
        }

        private void KeyUp(object source, Interface.LaunchpadKeyEventArgs e) {
            int x, y;
            x = e.GetX(); y = e.GetY();
            var pad = Pads[controller.GetCoordFromButton(x, y)];
            pad.MapperPressed = false;
        }

        private void KeyDown(object source, Interface.LaunchpadKeyEventArgs e) {
            int x, y;
            x = e.GetX(); y = e.GetY();
            var coords = controller.GetCoordFromButton(x, y);
            var pad = Pads[coords];
            pad.MapperPressed = true;
            if (CurrentSong != null) {
                var currTime = Convert.ToInt64(AudioFile.CurrentTime.TotalMilliseconds);
                var existingBeat = CurrentBeats.Where(b => (b.HitTime <= currTime + BEAT_DUR) && (b.HitTime >= currTime - BEAT_DUR) && b.x == coords.Item1 && b.y == coords.Item2).FirstOrDefault();
                if (existingBeat == null) {
                    //check for debounce, don't accidentally double up on beats.
                    var newBeat = new Beat(currTime, coords.Item1, coords.Item2);
                    CurrentBeats.Add(newBeat);
                }
            }
        }

        public void UpdatePads() {
            foreach (var key in Pads.Keys) {
                var pad = Pads[key];
                if (pad.MapperPressed)
                    pad.LightPad(87);
                else
                    pad.LightPad(pad.MapperVelo);
            }
        }        
    }
}
