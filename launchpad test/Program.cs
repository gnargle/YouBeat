using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using LaunchpadNET;
using Midi;
using Midi.Enums;
using YouBeatTypes;
using static LaunchpadNET.Interface;

namespace launchpad_test {
    class Program {
        private static Timer timer;
        private static GameController gameController;
        private static long lastScore = 0;
        static void Main(string[] args) {
            Console.CursorVisible = false;
            gameController = new GameController();
            timer = new Timer(100);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;                  
            if (gameController.interf.Connected) {
                while (true) {
                    gameController.MainLoop();
                }
            } else {
                Console.Write("Failed to connect to launchpad");
                Console.ReadKey();
            }
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            if (gameController.State == GameState.Menu) {
                if (gameController.menuState == MenuState.SongSelect) {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Select a song!" + "                                          ");
                    Console.WriteLine(gameController.CurrentSong.Title + "                                          ");
                    Console.WriteLine(gameController.CurrentSong.Artist + "                                          ");
                    Console.WriteLine("High Score: " + gameController.CurrentSong.ScoreList.Scores.First().Item1 + " - " + gameController.CurrentSong.ScoreList.Scores.First().Item2.ToString() + "                                          ");
                } else if (gameController.menuState == MenuState.DifficultySelect) {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"Song: {gameController.CurrentSong.Title}" + "                                          ");
                    Console.WriteLine("Select a difficulty!" + "                                          ");
                    Console.WriteLine(gameController.SelectedDifficulty + "                                          ");
                }
            } else if  (gameController.State == GameState.Setup || gameController.State == GameState.ReturnToMenu) {
                Console.Clear();            
            } else if (gameController.State == GameState.Game || gameController.State == GameState.GameEnding || gameController.State == GameState.GameOver) {
                Console.SetCursorPosition(0, 0);
                if (gameController.State == GameState.GameOver)
                    Console.WriteLine("Game Over!" + "                                          ");
                Console.WriteLine($"Score: {gameController.Score}" + "                                          ");
                if (gameController.Score != lastScore || gameController.Score == 0 || gameController.State == GameState.GameOver)
                    Console.WriteLine($"Last note score: {gameController.Score - lastScore}" + "                                          ");
                else
                    Console.WriteLine(String.Empty);
                Console.WriteLine($"Combo: {gameController.Combo}" + "                                          ");
                Console.WriteLine($"Max Combo: {gameController.MaxCombo}" + "                                          ");
                if (gameController.State == GameState.GameOver && gameController.Combo == gameController.TotalBeats)
                    Console.WriteLine("FULL COMBO!" + "                                          ");
                lastScore = gameController.Score;
            } else if (gameController.State == GameState.Init) {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Loading...");
            } else if (gameController.State == GameState.HighScoreEntry) {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"Score: {gameController.Score}" + "                                          ");
                Console.WriteLine("Please enter your name: " + "                                          ");
                Console.WriteLine(gameController.HighScoreName + "                                          ");
            }
        }
    }
}
