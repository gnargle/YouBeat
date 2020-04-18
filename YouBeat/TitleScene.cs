using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat {
    class TitleScene : BaseScene {
        private Entity TitleEntity;
        public TitleScene() : base() {
            AddGraphic<Image>(new Image("..\\..\\Backgrounds\\bg.png"));
            TitleEntity = new Entity(350, 240);
            TitleEntity.AddGraphic<Image>(Image.CreateCircle(100, Color.Red));
            Add(TitleEntity);
            TitleEntity.Graphic.Alpha = 0;
        }

        public override void Update() {
            base.Update();
            if (TitleEntity.Graphic.Alpha < 0.999) {
                TitleEntity.Graphic.Alpha += 0.001f;
            }
        }
    }
}
