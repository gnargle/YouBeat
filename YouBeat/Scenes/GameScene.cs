using Otter;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeat.Entities;
using YouBeatTypes;

namespace YouBeat.Scenes {
    class GameScene : BaseScene {

        private const int SOUND_LIMIT = 20;
        private const float FX_VOLUME = 0.2f;

        private ScoreEntity scoreEntity;
        private Sound MissSound;
        private Sound BadSound;
        private Sound OKSound;
        private Sound GoodSound;
        private Sound GreatSound;
        private Sound PerfectSound;
        private Sound[] soundCache;
        private GameState lastState;
        private int i = 0;

        public GameScene(GameController gameController) : base(gameController) {
            SetupGame();
            _controller.RemoveHold();
        }

        private void SetupGame() {
            _controller.OnHitReg = HitRegHandler;
            AddGraphic<Image>(new Image(@"..\..\Backgrounds\bg.png"));
            scoreEntity = new ScoreEntity(Game.Instance.Width - 250, 50);
            Add(scoreEntity);
            MissSound = new Sound(@"..\..\FX\Miss.wav") {
                Volume = FX_VOLUME
            };
            BadSound = new Sound(@"..\..\FX\Bad.wav") {
                Volume = FX_VOLUME
            };
            OKSound = new Sound(@"..\..\FX\OK.wav") {
                Volume = FX_VOLUME
            };
            GoodSound = new Sound(@"..\..\FX\Good.wav") {
                Volume = FX_VOLUME
            };
            GreatSound = new Sound(@"..\..\FX\Great.wav") {
                Volume = FX_VOLUME
            };
            PerfectSound = new Sound(@"..\..\FX\Perfect.wav") {
                Volume = FX_VOLUME
            };
            soundCache = new Sound[SOUND_LIMIT];
            i = 0;
        }
        public override void Update() {
            base.Update();
            scoreEntity.UpdateCombo(_controller.Combo, _controller.Score);
            if (_controller.State == GameState.GameEnding || _controller.State == GameState.GameOver) {
                if (lastState == GameState.Game) {
                    //first time in game ending state, set up the various entities for the end of the song.
                    if (_controller.Combo == _controller.TotalBeats)
                        Add(new FullComboEntity(Game.Instance.HalfWidth, Game.Instance.HalfHeight - 150));
                    Add(new TitleMessageEntity("Press any pad to continue", Game.Instance.HalfWidth, Game.Instance.HalfHeight + 150));
                }
            } else if (_controller.State == GameState.HighScoreEntryHold) {
                Game.SwitchScene(new HighScoreScene(_controller));
            } else if (_controller.State == GameState.ReturnToMenu || _controller.State == GameState.Menu) {
                Game.SwitchScene(new MenuScene(_controller));
            }
            lastState = _controller.State;
        }

        public void HitRegHandler(ScoreVelo score) {
            Sound copySound = null;
            var randX = Rand.Int(-50, 50);
            String sprite = String.Empty;
            int width = 0, height = 0;
            switch (score) {
                case ScoreVelo.Perfect:
                    sprite = @"..\..\Sprites\Perfect.png";
                    width = 458;
                    height = 89;
                    copySound = PerfectSound;
                    break;
                case ScoreVelo.Great:
                    sprite = @"..\..\Sprites\Great.png";
                    width = 345;
                    height = 89;
                    copySound = GreatSound;
                    break;
                case ScoreVelo.Good:
                    sprite = @"..\..\Sprites\Good.png";
                    width = 284;
                    height = 89;
                    copySound = GoodSound;
                    break;
                case ScoreVelo.OK:
                    sprite = @"..\..\Sprites\OK.png";
                    width = 154;
                    height = 89;
                    copySound = OKSound;
                    break;
                case ScoreVelo.Bad:
                    sprite = @"..\..\Sprites\Bad.png";
                    width = 214;
                    height = 89;
                    copySound = BadSound;
                    break;
                case ScoreVelo.Miss:
                    sprite = @"..\..\Sprites\Miss.png";
                    width = 247;
                    height = 89;
                    copySound = MissSound;
                    break;
            }
            Add(new Particle(Game.Instance.HalfWidth + randX, Game.Instance.HalfHeight, sprite, width, height) {
                Alpha = 1,
                FinalAlpha = 0,
                FinalY = 0,
                LifeSpan = 120
            });
            if (copySound != null) {
                //cache sounds so the garbage collector doesn't eat them
                soundCache[i] = new Sound(copySound);
                soundCache[i].Play();
                i++;
                if (i > SOUND_LIMIT - 1)
                    i = 0;
            }
        }
    }
}
