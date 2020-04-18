using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeatTypes;

namespace YouBeat {
    class BaseScene : Scene {
        protected GameController _controller;

        public BaseScene(GameController gameController) : base() {
            _controller = gameController;
        }

        public BaseScene() : base() {
            _controller = new GameController();
        }

        public override void Update() {
            base.Update();
            _controller.MainLoop();
        }
    }
}
