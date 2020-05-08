using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            new Thread(delegate () {
                _controller.ContinuousLoop(); //despatch this into a separate thread. Don't tie responsiveness to the UI refresh.
            }).Start();
        }

        public override void Update() {
            base.Update();
            //_controller.MainLoop();
        }

        public override void End() {
            base.End();
            _controller.LoopActive = false; // stop the loop
        }
    }
}
