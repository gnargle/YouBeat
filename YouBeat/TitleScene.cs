using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeat.Entities;

namespace YouBeat {
    class TitleScene : BaseScene {
        private TitleEntity titleEntity;
        private TitleMessageEntity messageEntity;
        //float alpha;
        public TitleScene() : base() {
            AddGraphic<Image>(new Image(@"..\..\Backgrounds\bg.png"));
            titleEntity = new TitleEntity(Game.Instance.Width/2, Game.Instance.Height/2);
            if (_controller.interf.Connected)
                messageEntity = new TitleMessageEntity("Press green to start", titleEntity.X, titleEntity.Y+150);
            else
                messageEntity = new TitleMessageEntity("No launchpad detected", titleEntity.X, titleEntity.Y+150);
            Add(messageEntity);
            Add(titleEntity);
            
        }

        public override void Update() {
            base.Update();
            if (_controller.State == YouBeatTypes.GameState.Menu) {                
                if (!titleEntity.Transitioning) {
                    titleEntity.Transitioning = true;
                } else if (titleEntity.Transitioned)
                    Game.SwitchScene(new MenuScene(_controller));
            }
        }
        public override void End() {            
            base.End();
        }
    }
}
