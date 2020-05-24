using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {
    class HighScoreTextEntity : Entity {
        private Text nameText;
        private String lastName = String.Empty;
        private Image EndImage;
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
        public HighScoreTextEntity(float x, float y) : base(x, y) {
            var titleLabel = new Text("New High Score!", Globals.CoolFont, Globals.FontSz) {
                Color = Color.Black
            };
            titleLabel.CenterTextOriginX();
            var enterNameLabel = new Text("Please enter your name.", Globals.CoolFont, 56) {
                Color = Color.Black,
                Y = Globals.FontSz
            };
            enterNameLabel.CenterTextOriginX();
            nameText = new Text(String.Empty, Globals.CoolFont, Globals.FontSz) {
                Color = Color.Black,
                Y = Globals.FontSz + 56
            };
            nameText.CenterTextOriginX();
            EndImage = new Image(@"..\..\Sprites\End.png") {
                Visible = false,
                Y = Globals.FontSz + 66
            };
            AddGraphic<Text>(nameText);
            AddGraphic<Text>(titleLabel);
            AddGraphic<Text>(enterNameLabel);
            AddGraphic<Image>(EndImage);
            alpha = 1;
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

            public void UpdateName(String name) {
            if (!name.Equals(lastName)) {
                var showEnd = false;
                if (name.EndsWith("[")) {
                    name = name.Substring(0, name.Length - 1);
                    showEnd = true;
                }
                nameText.String = name;
                if (showEnd) {
                    EndImage.X = nameText.Right + 5;
                    EndImage.Visible = true;
                } else {
                    EndImage.Visible = false;
                    nameText.CenterTextOriginX();
                }
            }
        }

    }
}
