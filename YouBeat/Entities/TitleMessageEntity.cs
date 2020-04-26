using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {
    class TitleMessageEntity: Entity {
        private string _message;
        public TitleMessageEntity(String message, float x, float y) : base() {
            message = _message;            
            var rectGraphic = Image.CreateRectangle(400, 50, Color.Magenta);
            rectGraphic.Name = "Rect";
            rectGraphic.X = x;
            rectGraphic.Y = y;
            var textGraphic = new Text(message, @"..\..\Fonts\Exo-Medium.ttf" , 25) {
                Name = "Text",
                X = x,
                Y = y,
                Color = Color.Black,                
            };
            AddGraphic<Image>(rectGraphic);
            AddGraphic<Text>(textGraphic);
            rectGraphic.CenterOrigin();
            textGraphic.CenterOrigin();
        }
        public override void Update() {
            base.Update();           
        }
    }
}
