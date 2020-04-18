using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace YouBeat {
    class Program {
        static void Main(string[] args) {
            
            var game = new Game("YouBeat", 1920, 1080, 60, false);
            //game.Start();
            game.Color = Color.Grey;            
            game.Start(new TitleScene());
        }
    }
}
