using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeatTypes {
    public enum MenuKey { None, LeftArrow, RightArrow, Confim };
    public enum GameState { Menu, Setup, Game, Pause, GameEnding, GameOver, Init };
    public enum ComboChange { Add, Break };
}
