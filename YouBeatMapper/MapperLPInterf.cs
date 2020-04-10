﻿using LaunchpadNET;
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
        public MediaFoundationReader audioFile { get; set; }
        public WaveOut wvOut { get; set; }

        public MapperLPInterf() {
            controller = new GameController(true);
            interf = new Interface();
            var connected = interf.getConnectedLaunchpads();
            if (connected.Count() > 0) {
                if (interf.connect(connected[0])) {
                    interf.OnLaunchpadKeyDown += keyDown;
                    interf.OnLaunchpadKeyUp += keyUp;
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

        private void keyUp(object source, Interface.LaunchpadKeyEventArgs e) {
            int x, y;
            x = e.GetX(); y = e.GetY();
            var pad = Pads[controller.GetCoordFromButton(x, y)];
            pad.MapperPressed = false;
        }

        private void keyDown(object source, Interface.LaunchpadKeyEventArgs e) {
            int x, y;
            x = e.GetX(); y = e.GetY();
            var coords = controller.GetCoordFromButton(x, y);
            var pad = Pads[coords];
            pad.MapperPressed = true;
            if (CurrentSong != null) {
                var currTime = Convert.ToInt64(audioFile.CurrentTime.TotalMilliseconds);
                var existingBeat = CurrentSong.Beats.Where(b => (b.HitTime <= currTime + 125) && (b.HitTime >= currTime - 125) && b.x == coords.Item2 && b.y == coords.Item1).FirstOrDefault();
                if (existingBeat != null) {
                    CurrentSong.Beats.Remove(existingBeat);
                } else {
                    var newBeat = new Beat(currTime, coords.Item2, coords.Item1);
                    CurrentSong.Beats.Add(newBeat);
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
