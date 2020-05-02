using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {
    class TitleMessageEntity: Entity {
        private float alpha = 0;
        private bool _transitioning = false;
        private string _message;
        private bool appeared = false;
        public bool Transitioning {
            get { return _transitioning; }
            set {
                if (value == true) {
                    Tween(this, new { alpha = 0 }, 120).Ease(Ease.QuadOut);
                }
                _transitioning = value;
            }
        }

        public TitleMessageEntity(String message, float x, float y) : base() {
            _message = message;         
            var textGraphic = new Text(message, Globals.GeneralFont , 25) {
                Name = "Text",
                X = x,
                Y = y,
                Color = Color.Black,                
            };
            var rectGraphic = Image.CreateRectangle(textGraphic.Width + 50, 50, Color.FromBytes(251, 116, 170));            
            rectGraphic.Name = "Rect";
            rectGraphic.X = x;
            rectGraphic.Y = y;
            rectGraphic.CenterOrigin();
            var circLeft = new Image(@"..\..\Sprites\messageend.png") {
                X = rectGraphic.Left - 12f,
                Y = y
            };
            circLeft.CenterOrigin();
            var circRight = new Image(@"..\..\Sprites\messageend.png") {
                X = rectGraphic.Right + 12.5f,
                Y = y,
                Angle = 180f
            };
            circRight.CenterOrigin();
            AddGraphic<Image>(rectGraphic);
            AddGraphic<Image>(circRight);
            AddGraphic<Image>(circLeft);
            AddGraphic<Text>(textGraphic); 
            textGraphic.CenterTextOrigin(); 
            Tween(this, new { alpha = 1 }, 240, 250).Ease(Ease.QuadIn);
        }
        public override void Update() {
            base.Update();
            foreach (var graph in Graphics)
                graph.Alpha = alpha;
            if (appeared == false && alpha == 1) {
                appeared = true;
                Tween(this, new { alpha = 0.5 }, 120).Ease(Ease.SineInOut).Reflect().Repeat();
            }
        }
    }
}
