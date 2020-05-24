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
        public FullComboEntity(float x, float y) : base(x, y) {
            _fullComboGraphic = new Image(@"..\..\Sprites\FullCombo.png");
            _fullComboGraphic.CenterOrigin();
            AddGraphic<Image>(_fullComboGraphic);
            Tween(this, new { sh = _fullComboGraphic.Height, sw = _fullComboGraphic.Width }, 120).Ease(Ease.QuintInOut);
        }

        public override void Update() {
            base.Update();
            _fullComboGraphic.ScaleX = sw / (float)Graphic.Width;
            _fullComboGraphic.ScaleY = sh / (float)Graphic.Height;
        }

    }
}
