using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeat.Entities;
using YouBeatTypes;

namespace YouBeat.Scenes {
    class GameScene : BaseScene {

        private ScoreEntity comboEntity;

        public GameScene(GameController gameController) : base(gameController) {
            SetupGame();
            _controller.StartGame();
        }

        private void SetupGame() {
            _controller.OnHitReg = HitRegHandler;
            AddGraphic<Image>(new Image(@"..\..\Backgrounds\bg.png"));
            comboEntity = new ScoreEntity(Game.Instance.Width - 250, 50);
            Add(comboEntity);
        }
        public override void Update() {
            base.Update();
            comboEntity.UpdateCombo(_controller.Combo, _controller.Score);
        }

        public void HitRegHandler(ScoreVelo score) {
            var randX = Rand.Int(-50, 50);
            String sprite = String.Empty;
            int width = 0, height = 0;
            switch (score) {
                case ScoreVelo.Perfect:
                    sprite = @"..\..\Sprites\Perfect.png";
                    width = 458;
                    height = 89;
                    break;
                case ScoreVelo.Great:
                    sprite = @"..\..\Sprites\Great.png";
                    width = 345;
                    height = 89;
                    break;
                case ScoreVelo.Good:
                    sprite = @"..\..\Sprites\Good.png";
                    width = 284;
                    height = 89;
                    break;
                case ScoreVelo.OK:
                    sprite = @"..\..\Sprites\OK.png";
                    width = 154;
                    height = 89;
                    break;
                case ScoreVelo.Bad:
                    sprite = @"..\..\Sprites\Bad.png";
                    width = 214;
                    height = 89;
                    break;
                case ScoreVelo.Miss:
                    sprite = @"..\..\Sprites\Miss.png";
                    width = 247;
                    height = 89;
                    break;
            }
            Add(new Particle(Game.Instance.HalfWidth + randX, Game.Instance.HalfHeight, sprite, width, height) {
                Alpha = 1,
                FinalAlpha = 0,
                FinalY = 0,
                LifeSpan = 120
            });
        }
    }
}
