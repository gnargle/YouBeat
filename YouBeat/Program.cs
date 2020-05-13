using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using YouBeat.Scenes;

namespace YouBeat {
    class Program {
        static void Main(string[] args) {
            //var game = new Game("YouBeat", 1920, 1080, 60, false);
            var game = new Game("YouBeat", 1920, 1080, 60, true);
            if (game.Debugger != null) {
                game.Debugger.ToggleKey = Key.End;
                game.Debugger.ShowPerformance(5);
            }
            game.Color = Color.Grey;
            game.Start(new TitleScene());            
        }        
    }    
}
