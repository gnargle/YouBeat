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
            Song lastSong = null;
            Difficulty lastDifficulty = Difficulty.Expert;
            MenuState lastMenuState = MenuState.SongSelect;
            var gameController = new GameController();
            if (gameController.interf.Connected) {
                while (true) {
                    gameController.MainLoop();
                    if (gameController.State == GameState.Menu) {
                        if (gameController.menuState == MenuState.SongSelect) {
                            if (gameController.menuState != lastMenuState || gameController.CurrentSong != lastSong) {
                                Console.Clear();
                                Console.WriteLine("Select a song!");
                                Console.WriteLine(gameController.CurrentSong.Title);
                                Console.WriteLine(gameController.CurrentSong.Artist);
                                lastSong = gameController.CurrentSong;                                
                            }
                        } else if (gameController.menuState == MenuState.DifficultySelect) {
                            if (gameController.menuState != lastMenuState || lastDifficulty != gameController.SelectedDifficulty) {
                                Console.Clear();
                                Console.WriteLine($"Song: {gameController.CurrentSong.Title}");
                                Console.WriteLine("Select a difficulty!");
                                Console.WriteLine(gameController.SelectedDifficulty);
                                lastDifficulty = gameController.SelectedDifficulty;
                            }
                        }
                        lastMenuState = gameController.menuState;
                    } else if (gameController.State == GameState.Game || gameController.State == GameState.GameEnding || gameController.State == GameState.GameOver) {
                        if (lastScore != gameController.Score || lastCombo != gameController.Combo) {
                            Console.Clear();
                            Console.WriteLine($"Score: {gameController.Score}");
                            Console.WriteLine($"Last note score: {gameController.Score - lastScore}");
                            Console.WriteLine($"Combo: {gameController.Combo}");
                            Console.WriteLine($"Max Combo: {gameController.MaxCombo}");
                            if (gameController.Combo == gameController.TotalBeats)
                                Console.WriteLine("FULL COMBO!");
                            lastCombo = gameController.Combo;
                            lastScore = gameController.Score;
                        }
                    }
                }
            } else {
                Console.Write("Failed to connect to launchpad");
                Console.ReadKey();
            }
        }            
    }
}
