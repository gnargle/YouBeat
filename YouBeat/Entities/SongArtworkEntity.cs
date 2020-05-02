using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {
    class SongArtworkEntity : Entity {

        //tween values
        private float _y;
        private float _sx;
        private float _sy;
        private float _x;
        //consts
        private const float mainSize = 400;
        private const float subSize = 250;        
        public bool Selected { get; set; }

        public SongArtworkEntity(String artworkpath, float x, float y, bool selected = false) : base(x, y) {
            Image imageGraphic;
            if (String.IsNullOrEmpty(artworkpath))
                imageGraphic = new Image(@"..\..\Sprites\DefaultArt.png");
            else
                imageGraphic = new Image(artworkpath);
            if (selected) {
                //ok, let's define a size for this then. If it's the main image, lets say we want it as 250x250 px.
                //to do that, we need to divide 250 by the current size to get a float.
                imageGraphic.ScaleX = mainSize / (float)imageGraphic.Width;
                imageGraphic.ScaleY = mainSize / (float)imageGraphic.Height;                
            } else {
                //Let's make the sub images 200x200.
                imageGraphic.ScaleX = subSize / (float)imageGraphic.Width;
                imageGraphic.ScaleY = subSize / (float)imageGraphic.Height;
            }
            _sx = imageGraphic.ScaleX;
            _sy = imageGraphic.ScaleY;
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
                Tween(this, new {
                    _sx = mainSize / (float)Graphic.Width,
                    _sy = mainSize / (float)Graphic.Height
                }, 60, 0).Ease(Ease.SineInOut);
            } else {
                Tween(this, new {
                    _sx = subSize / (float)Graphic.Width,
                    _sy = subSize / (float)Graphic.Height
                }, 60, 0).Ease(Ease.SineInOut);
            }
        }

        public void SetNewX(float x) {
            Tween(this, new { _x = x }, 60, 0).Ease(Ease.QuintInOut);
        }

        public override void Update() {
            base.Update();
            Y = _y;
            X = _x;
            Graphic.ScaleX = _sx;
            Graphic.ScaleY = _sy;
        }

    }
}
