using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {
    class SongArtworkEntity : Entity {

        //tween values
        private Tween NewXTween;
        private float _y;
        private float _sx;
        private float _sy;
        private float _x;        
        //consts
        public const float MainSize = 400;
        public const float SubSize = 250;
        public const float SubGap = 10;
        public bool Selected { get; set; }
        public float CompleteX { get; private set; }
        public float CompleteScaledWidth { get; private set; }
        public bool NewXTweenComplete { get {                
                return NewXTween == null || NewXTween?.Completion == 1;
            } }
        private bool _transitioning = false;
        public bool Transitioned { get; set; } = false;
        public bool Transitioning {
            get { return _transitioning; }
            set {
                if (value == true) {
                    Tween(this, new { _y = -Graphic.HalfHeight }, 120, 0).Ease(Ease.ElasticInOut);
                }
                _transitioning = value;
            }
        }

        public SongArtworkEntity(String artworkpath, float x, float y, bool selected = false) : base(x, y) {
            Image imageGraphic;
            if (String.IsNullOrEmpty(artworkpath))
                imageGraphic = new Image(@"..\..\Sprites\DefaultArt.png");
            else
                imageGraphic = new Image(artworkpath);
            if (selected) {
                //ok, let's define a size for this then. If it's the main image, lets say we want it as 400x400 px.
                //to do that, we need to divide 400 by the current size to get a float.
                CompleteScaledWidth = MainSize;
                imageGraphic.ScaleX = MainSize / (float)imageGraphic.Width;
                imageGraphic.ScaleY = MainSize / (float)imageGraphic.Height;                
            } else {
                //Let's make the sub images 200x200.
                CompleteScaledWidth = SubSize;
                imageGraphic.ScaleX = SubSize / (float)imageGraphic.Width;
                imageGraphic.ScaleY = SubSize / (float)imageGraphic.Height;
            }
            
            _sx = imageGraphic.ScaleX;
            _sy = imageGraphic.ScaleY;
            CompleteX = X;
            _x = X;
            imageGraphic.CenterOrigin();
            Selected = selected;
            _y = -imageGraphic.HalfHeight;
            AddGraphic<Image>(imageGraphic);
            Tween(this, new { _y = y }, 120, 0).Ease(Ease.ElasticInOut);            
        }

        public void SetSelectedEntity(bool selected) {
            Selected = selected;
            if (selected) {
                CompleteScaledWidth = MainSize;
                Tween(this, new {
                    _sx = MainSize / (float)Graphic.Width,
                    _sy = MainSize / (float)Graphic.Height
                }, 60, 0).Ease(Ease.SineInOut);
            } else {
                CompleteScaledWidth = SubSize;
                Tween(this, new {
                    _sx = SubSize / (float)Graphic.Width,
                    _sy = SubSize / (float)Graphic.Height
                }, 60, 0).Ease(Ease.SineInOut);
            }
        }

        public void SetNewX(float x) {
            NewXTween = Tween(this, new { _x = x }, 60, 0).Ease(Ease.QuintInOut);
            CompleteX = x;            
        }

        public override void Update() {
            base.Update();
            Y = _y;
            X = _x;
            Graphic.ScaleX = _sx;
            Graphic.ScaleY = _sy;
            if (Transitioning && Y == -Graphic.HalfHeight)
                Transitioned = true;
        }

    }
}
