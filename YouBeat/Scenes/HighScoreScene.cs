using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeat.Entities;
using YouBeatTypes;

namespace YouBeat.Scenes {
    class HighScoreScene : BaseScene{
        private HighScoreTextEntity text;        
        public HighScoreScene(GameController gameController) : base(gameController) {
            SetupScoreEntry();
            _controller.RemoveHold();
        }

        public void SetupScoreEntry() {
            AddGraphic<Image>(new Image(@"..\..\Backgrounds\bg.png"));
            text = new HighScoreTextEntity(Game.Instance.HalfWidth, Game.Instance.HalfHeight - 300);
            Add(text);
        }

        public override void Update() {
            base.Update();
            text.UpdateName(_controller.HighScoreName);
            if (text.Transitioning && text.Transitioned) {
                _controller.RemoveHold();
                _controller.MainLoop(); //we need to do this so that we're all set up to transition to the menu scene.
                Game.SwitchScene(new MenuScene(_controller));
            } else if (!text.Transitioning && _controller.State == GameState.ReturnToMenuHold) {
                text.Transitioning = true;                
            }
        }
    }
}
