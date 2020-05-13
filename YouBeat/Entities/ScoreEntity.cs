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
        private const int fontSz = 72;
        public ScoreEntity(float x, float y) : base(x, y) {
            _scoreLabel = new Text("Score:", @"..\..\Fonts\AstronBoyWonder.ttf", fontSz) {
                Color = Color.Black                
            };
            _scoreLabel.CenterTextOriginX();
            _scoreText = new Text("0", @"..\..\Fonts\AstronBoyWonder.ttf", fontSz) {
                Color = Color.Black,
                Y = fontSz
            };
            _scoreText.CenterTextOriginX();            
            _comboLabel = new Text("Combo:", @"..\..\Fonts\AstronBoyWonder.ttf", fontSz) {
                Color = Color.Black,
                Y = fontSz * 2
            };
            _comboLabel.CenterTextOriginX();
            _comboText = new Text("0", @"..\..\Fonts\AstronBoyWonder.ttf", fontSz) {
                Color = Color.Black,
                Y = fontSz * 3
            };
            _comboText.CenterTextOriginX();
            AddGraphic<Text>(_scoreText);
            AddGraphic<Text>(_comboText);
            AddGraphic<Text>(_scoreLabel);
            AddGraphic<Text>(_comboLabel);
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
