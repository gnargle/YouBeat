using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YouBeatTypes;

namespace YouBeat.Scenes {
    class BaseScene : Scene {
        protected GameController _controller;

        public BaseScene(GameController gameController) : base() {
            _controller = gameController;
        }

        public BaseScene() : base() {
            _controller = new GameController(ControllerCreator.Youbeat);
        }

        public override void Update() {
            base.Update();
            _controller.MainLoop();
        }

        public override void End() {
            base.End();
        }
    }
}
