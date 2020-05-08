using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeatTypes {
    public enum MenuKey { None, LeftArrow, RightArrow, Confim, Cancel };
    public enum GameState { Menu, Setup, Game, Pause, GameEnding, GameOver, Init, ReturnToMenu, Title, HighScoreEntry, AwaitingLaunchpad, ReturnToTitle };
    public enum MenuState { SongSelect, DifficultySelect, NameEntry };
    public enum Difficulty { Easy, Advanced, Expert };
    public enum ComboChange { Add, Break };
    public enum ScoreVelo { Miss = 0, Bad = 7, OK = 5, Good = 9, Great = 13, Perfect = 87 }
    public enum Timing { None, Early, Late }
}
