using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeat.Entities;
using YouBeatTypes;

namespace YouBeat {
    class TitleScene : BaseScene {
        private TitleEntity titleEntity;
        private ExitEntity exitEntity;
        private TitleMessageEntity messageEntity;
        private bool DetectingLaunchpad;
        //float alpha;

        private void SetupTitleScreen() {
            AddGraphic<Image>(new Image(@"..\..\Backgrounds\bg.png"));
            titleEntity = new TitleEntity(Game.Instance.Width / 2, Game.Instance.Height / 2);
            exitEntity = new ExitEntity(10, 10);
            if (_controller.interf.Connected)
                messageEntity = new TitleMessageEntity("Press green to start!", titleEntity.X, titleEntity.Y + 150);
            else {
                messageEntity = new TitleMessageEntity("No Launchpad detected. Please restart the game with a Launchpad plugged in.", titleEntity.X, titleEntity.Y + 150);
                DetectingLaunchpad = true;
            }
            Add(messageEntity);
            Add(titleEntity);
            Add(exitEntity);
        }

        public TitleScene(GameController gameController) : base(gameController) {
            SetupTitleScreen();
        }

        public TitleScene() : base() {
            SetupTitleScreen();
        }

        public override void Update() {
            base.Update();
            if (Input.KeyDown(Key.Escape)) {
                Game.Close();
            }
            if (_controller.State == GameState.Menu) {                
                if (!titleEntity.Transitioning) {
                    titleEntity.Transitioning = true;
                    messageEntity.Transitioning = true;
                    exitEntity.Transitioning = true;
                } else if (titleEntity.Transitioned)
                    Game.SwitchScene(new MenuScene(_controller));
            } else if (DetectingLaunchpad && _controller.State != GameState.AwaitingLaunchpad) {
                DetectingLaunchpad = false;
                Remove(messageEntity);
                messageEntity = new TitleMessageEntity("Press green to start!", titleEntity.X, titleEntity.Y + 150);
                Add(messageEntity);
            }
        }
        public override void End() {            
            base.End();
        }
    }
}
