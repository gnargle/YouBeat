using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {
    class HighScoreTitleEntity : Entity {
        public HighScoreTitleEntity(float x, float y) : base(x, y) {
            var titleLabel = new Text("New High Score!", @"..\..\Fonts\AstronBoyWonder.ttf", Globals.FontSz) {
                Color = Color.Black
            };
            var enterNameLabel = new Text("Please enter your name.", @"..\..\Fonts\AstronBoyWonder.ttf", 56) {
                Color = Color.Black,
                Y = Globals.FontSz
            };
        }
    }
}
