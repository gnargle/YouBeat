using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {
    class TitleEntity : Entity {
        // Tween this value to determine the color later.
        float alpha;

        public TitleEntity(float x, float y) : base(x, y) {
            // Add a simple circle graphic.
            AddGraphic<Image>(Image.CreateCircle(100, Color.Red));
            Graphic.CenterOrigin();
            Graphic.Alpha = 0;
            // Tween the hue from 0 to 1 and repeat it forever over 360 frames.
            Tween(this, new { alpha = 1 }, 360);
        }

        public override void Update() {
            base.Update();
            // Update the Color every update by using the tweened hue value.
            Graphic.Alpha = alpha;
        }
    }
}
