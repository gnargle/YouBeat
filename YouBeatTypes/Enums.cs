using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeatTypes {
    public enum MenuKey { None, LeftArrow, RightArrow, Confim, Cancel };
    public enum GameState { Menu, Setup, Game, Pause, GameEnding, GameOver, Init, ReturnToMenu };
    public enum MenuState { SongSelect, DifficultySelect };
    public enum Difficulty { Easy, Advanced, Expert };
    public enum ComboChange { Add, Break };
}
