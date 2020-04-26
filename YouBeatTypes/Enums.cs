using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeatTypes {
    public enum MenuKey { None, LeftArrow, RightArrow, Confim, Cancel };
    public enum GameState { Menu, Setup, Game, Pause, GameEnding, GameOver, Init, ReturnToMenu, Title, HighScoreEntry };
    public enum MenuState { SongSelect, DifficultySelect, NameEntry };
    public enum Difficulty { Easy, Advanced, Expert };
    public enum ComboChange { Add, Break };
}
