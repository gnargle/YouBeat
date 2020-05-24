﻿using Otter;
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
            if (_controller.State == GameState.ReturnToMenu || _controller.State == GameState.Menu) {
                Game.SwitchScene(new MenuScene(_controller));
            }
        }
    }
}
