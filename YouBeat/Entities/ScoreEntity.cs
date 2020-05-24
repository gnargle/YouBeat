using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {
    class ScoreEntity : Entity {
        private Text _scoreText;
        private Text _comboText;
        private Text _scoreLabel;
        private Text _comboLabel;
        private long _lastCombo = 0;
        private long _lastScore = 0;
        private float alpha = 0;
        private bool _transitioning = false;
        public bool Transitioned { get; set; } = false;
        public bool Transitioning {
            get { return _transitioning; }
            set {
                if (value == true) {
                    Tween(this, new { alpha = 0 }, 120).Ease(Ease.QuadOut);
                }
                _transitioning = value;
            }
        }
        public ScoreEntity(float x, float y) : base(x, y) {
            _scoreLabel = new Text("Score:", Globals.CoolFont, Globals.FontSz) {
                Color = Color.Black                
            };
            _scoreLabel.CenterTextOriginX();
            _scoreText = new Text("0", Globals.CoolFont, Globals.FontSz) {
                Color = Color.Black,
                Y = Globals.FontSz
            };
            _scoreText.CenterTextOriginX();            
            _comboLabel = new Text("Combo:", Globals.CoolFont, Globals.FontSz) {
                Color = Color.Black,
                Y = Globals.FontSz * 2
            };
            _comboLabel.CenterTextOriginX();
            _comboText = new Text("0", Globals.CoolFont, Globals.FontSz) {
                Color = Color.Black,
                Y = Globals.FontSz * 3
            };
            _comboText.CenterTextOriginX();
            AddGraphic<Text>(_scoreText);
            AddGraphic<Text>(_comboText);
            AddGraphic<Text>(_scoreLabel);
            AddGraphic<Text>(_comboLabel);
            Tween(this, new { alpha = 1 }, 120).Ease(Ease.QuadOut);
        }
        public override void Update() {
            base.Update();
            foreach (var graph in Graphics) {
                graph.Alpha = alpha;
            }
            if (Transitioning && Graphic.Alpha == 0) {
                Transitioned = true;
            }
        }
        public void UpdateCombo(long combo, long score) {
            if (_lastCombo != combo) {
                _scoreText.String = $"{score}";
                _scoreText.CenterTextOriginX();
                _lastCombo = combo;                
            }
            if (_lastScore != score) {
                _comboText.String = $"{combo}";
                _comboText.CenterTextOriginX();
                _lastScore = score;
            }
        }

    }
}
