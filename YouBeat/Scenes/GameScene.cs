﻿using Otter;
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
        private bool changingScene = false;
        private ScoreEntity scoreEntity;
        private FullComboEntity fullComboEntity;
        private TitleMessageEntity titleMsgEntity;
        private CountsEntity countsEntity;
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
            countsEntity = new CountsEntity(Game.Instance.HalfWidth, 20);
            Add(countsEntity);
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
            countsEntity.UpdateCounts(_controller);
            if (changingScene) {
                if (scoreEntity.Transitioned)
                    if (_controller.State == GameState.HighScoreEntryHold) {
                        _controller.RemoveHold();
                        Game.SwitchScene(new HighScoreScene(_controller));
                    } else if (_controller.State == GameState.ReturnToMenuHold) {
                        _controller.RemoveHold();
                        _controller.MainLoop(); //we need to do this so that we're all set up to transition to the menu scene.
                        Game.SwitchScene(new MenuScene(_controller));
                    }
            }
            else if (_controller.State == GameState.GameEnding || _controller.State == GameState.GameOver) {
                if (lastState == GameState.Game) {
                    //first time in game ending state, set up the various entities for the end of the song.
                    if (_controller.Combo == _controller.TotalBeats) {
                        fullComboEntity = new FullComboEntity(Game.Instance.HalfWidth, Game.Instance.HalfHeight - 150);
                        Add(fullComboEntity);
                    }
                    titleMsgEntity = new TitleMessageEntity("Press any pad to continue", Game.Instance.HalfWidth, Game.Instance.HalfHeight + 150);
                    Add(titleMsgEntity);
                }
            } else if (_controller.State == GameState.HighScoreEntryHold || _controller.State == GameState.ReturnToMenuHold) {
                changingScene = true;
                scoreEntity.Transitioning = true;
                if (fullComboEntity != null) fullComboEntity.Transitioning = true;
                titleMsgEntity.Transitioning = true;
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
            //TODO: need to change this to use a cache created at song start to help the framerate out
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
