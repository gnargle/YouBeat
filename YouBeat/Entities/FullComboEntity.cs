using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {
    class FullComboEntity : Entity {
        private Image _fullComboGraphic;
        private float sh = 0, sw = 0;
        private float alpha = 0;
        private bool _transitioning = false;
        public bool Transitioning {
            get { return _transitioning; }
            set {
                if (value == true) {
                    Tween(this, new { alpha = 0 }, 120).Ease(Ease.QuadOut);
                }
                _transitioning = value;
            }
        }
        public FullComboEntity(float x, float y) : base(x, y) {
            _fullComboGraphic = new Image(@"..\..\Sprites\FullCombo.png");
            _fullComboGraphic.CenterOrigin();
            alpha = 1;
            AddGraphic<Image>(_fullComboGraphic);
            Tween(this, new { sh = _fullComboGraphic.Height, sw = _fullComboGraphic.Width }, 120).Ease(Ease.QuintInOut);
        }

        public override void Update() {
            base.Update();
            _fullComboGraphic.ScaleX = sw / (float)Graphic.Width;
            _fullComboGraphic.ScaleY = sh / (float)Graphic.Height;
            _fullComboGraphic.Alpha = alpha;
        }

    }
}
