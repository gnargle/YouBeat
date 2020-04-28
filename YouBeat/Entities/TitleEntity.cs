using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {
    class TitleEntity : Entity {
        // Tween this value to determine the color later.
        private float alpha = 0;
        private float _y;
        private bool _transitioning = false;
        public bool Transitioned = false;
        public bool Transitioning { get { return _transitioning; } set {
                if (value == true) {
                    Tween(this, new { alpha = 0 }, 120).Ease(Ease.QuadOut);
                }
                _transitioning = value;
            }
        }

        public TitleEntity(float x, float y) : base(x, y) {
            AddGraphic<Image>(new Image(@"..\..\Sprites\logo.png"));
            Graphic.CenterOrigin();
            Graphic.Alpha = 0;
            Tween(this, new { alpha = 1 }, 120).Ease(Ease.QuadIn);
            Tween(this, new { _y = 15 }, 30).Ease(Ease.SineInOut).Reflect().Repeat();
        }

        public override void Update() {
            base.Update();
            Graphic.Alpha = alpha;
            if (Transitioning && Graphic.Alpha == 0) {
                Transitioned = true;
            }
            Graphic.Y = _y;
        }
        public override void SceneEnd() {
            base.SceneEnd();

        }
    }
}
