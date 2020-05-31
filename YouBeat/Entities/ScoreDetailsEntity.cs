using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeatTypes;

namespace YouBeat.Entities {    
    class ScoreDetailsEntity : Entity {
        private Text ComboText;
        private List<Text> ScoreListTexts = new List<Text>();
        private Song nextSong;
        private int _x;
        private float _alpha = 1;
        private bool _transitioning = false;
        public bool Transitioned { get; set; } = false;
        public bool UpdatingSong { get; private set; } = false;        
        public bool OffScreen { get { return X == -Graphic.Width - 10; } }
        public bool Transitioning {
            get { return _transitioning; }
            set {
                if (value == true) {
                    Tween(this, new { _x = -Graphic.Height }, 120, 0).Ease(Ease.ElasticOut);
                }
                _transitioning = value;
            }
        }
        public ScoreDetailsEntity(Song initialSong, float x, float y) : base(x, y) {
            var imageGraphic = new Image(@"..\..\Sprites\ScoreDetailsFrame.png");
            imageGraphic.CenterOrigin();
            AddGraphic<Image>(imageGraphic);
            _x = -imageGraphic.Width;
            var shiftY = -imageGraphic.HalfHeight + 15;
            var fontSz = 40;
            Tween(this, new { _x = x }, 120, 0).Ease(Ease.ElasticInOut);
            ComboText = new Text($"Max Combo: {initialSong.ScoreList.MaxCombo}", Globals.GeneralFont, fontSz);
            ComboText.Color = Color.Black;
            ComboText.Y = shiftY;
            ComboText.CenterTextOriginX();
            shiftY += fontSz+50;
            AddGraphic<Text>(ComboText);
            for (int i = 0; i < 10; i++) {
                var currText = new Text("", Globals.GeneralFont, fontSz);
                if (initialSong.ScoreList.Scores.Count > i) {
                    var currScore = initialSong.ScoreList.Scores[i];
                    currText.String = $"{currScore.Item1}: {currScore.Item2}";
                }
                currText.Color = Color.Black;
                currText.Y += shiftY;
                currText.CenterTextOriginX();
                shiftY += fontSz+10;
                AddGraphic<Text>(currText);
                ScoreListTexts.Add(currText);
            }
        }

        public void SetTextDetails(Song newSong) {
            ComboText.String = $"Max Combo: {newSong.ScoreList.MaxCombo}";
            if (ComboText.Width > 710)
                ComboText.ScaledWidth = 710;
            else
                ComboText.ScaledWidth = ComboText.Width;
            ComboText.CenterTextOriginX();
            for (int i = 0; i < 10; i++) {
                var currText = ScoreListTexts[i];
                if (newSong.ScoreList.Scores.Count > i) {
                    var currScore = newSong.ScoreList.Scores[i];
                    currText.String = $"{currScore.Item1}: {currScore.Item2}";
                    currText.CenterTextOriginX();
                }
            }
            UpdatingSong = false;
        }

        public void ChangeSong(Song newSong) {
            nextSong = newSong;
            Tween(this, new { _alpha = 0 }, 30, 0).Ease(Ease.QuadOut);
            UpdatingSong = true;
        }


        public override void Update() {
            base.Update();
            X = _x;
            if (UpdatingSong) {                
                if (_alpha == 0) {
                    SetTextDetails(nextSong);
                    Tween(this, new { _alpha = 1 }, 30, 0).Ease(Ease.QuadIn);
                }             
            }
            ComboText.Alpha = _alpha;
            foreach (var currText in ScoreListTexts) {
                currText.Alpha = _alpha;
            }
        }
    }
}
