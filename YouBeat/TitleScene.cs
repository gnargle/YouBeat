using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeat.Entities;

namespace YouBeat {
    class TitleScene : BaseScene {
        private Entity titleEntity;
        //float alpha;
        public TitleScene() : base() {
            AddGraphic<Image>(new Image("..\\..\\Backgrounds\\bg.png"));
            titleEntity = new TitleEntity(Game.Instance.Width/2, Game.Instance.Height/2);           
            Add(titleEntity);
        }

        public override void Update() {
            base.Update();
        }
    }
}
