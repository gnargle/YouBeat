using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchpadNET;
using Midi;
using Midi.Enums;
using YouBeatTypes;
using static LaunchpadNET.Interface;

namespace launchpad_test {
    class Program {       
        static void Main(string[] args) {
            long lastScore = 0;
            long lastCombo = 0;
            var gameController = new GameController();
            if (gameController.interf.Connected) {
                while (true) {
                    gameController.MainLoop();
                    if (lastScore != gameController.Score || lastCombo != gameController.Combo) {
                        Console.Clear();
                        Console.WriteLine($"Score: {gameController.Score}");
                        Console.WriteLine($"Combo: {gameController.Combo}");
                        lastCombo = gameController.Combo;
                        lastScore = gameController.Score;
                    }
                }
            } else {
                Console.Write("Failed to connect to launchpad");
                Console.ReadKey();
            }
        }            
    }
}
