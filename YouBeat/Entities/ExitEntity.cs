using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {    
    class ExitEntity : Entity {

        private bool _transitioning = false;
        private float alpha = 0;
        public bool Transitioning {
            get { return _transitioning; }
            set {
                if (value == true) {
                    Tween(this, new { alpha = 0 }, 120).Ease(Ease.QuadOut);
                }
                _transitioning = value;
            }
        }
        
        public ExitEntity(float x, float y) : base(x, y) {
            AddGraphic<Image>(new Image(@"..\..\Sprites\Exit.png"));
            //Graphic.CenterOrigin();
            Graphic.Alpha = 0;
            Tween(this, new { alpha = 1 }, 240, 250).Ease(Ease.QuadIn).Reflect().Repeat();
        }
        public override void Update() {
            base.Update();
            Graphic.Alpha = alpha;
        }
    }
}
