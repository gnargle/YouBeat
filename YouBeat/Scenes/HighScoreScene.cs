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
        public HighScoreScene(GameController gameController) : base(gameController) {
            SetupScoreEntry();
            _controller.RemoveHold();
        }

        public void SetupScoreEntry() {
            Add(new HighScoreTitleEntity(Game.Instance.HalfWidth, Game.Instance.HalfHeight - 200));
        }
    }
}
