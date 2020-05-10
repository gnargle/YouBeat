using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {
    class ComboEntity : Entity {
        private Text _comboText;
        private long _lastCombo = 0;
        public ComboEntity(float x, float y) : base(x, y) {
            _comboText = new Text("Combo: 0", @"..\..\Fonts\AstronBoyWonder.ttf", 72) {
                Color = Color.Black
            };
            _comboText.CenterTextOriginX();
            AddGraphic<Text>(_comboText);
        }

        public void UpdateCombo(long combo) {
            if (_lastCombo != combo) {
                _comboText.String = $"Combo: {combo}";
                _comboText.CenterTextOriginX();
                _lastCombo = combo;
            }
        }

    }
}
