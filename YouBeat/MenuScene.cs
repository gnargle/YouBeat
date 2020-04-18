using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeatTypes;

namespace YouBeat {
    class MenuScene : BaseScene {

        public void SetupMenu() {
            AddGraphic<Image>(new Image("..\\..\\Backgrounds\\bg.png"));
        }

        public MenuScene(GameController gameController) : base(gameController) {
            SetupMenu();
        }       

        public override void Update() {
            base.Update();            
        }
    }
}
