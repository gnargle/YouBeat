using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeatTypes;

namespace YouBeat.Entities {    
    class SongDetailsEntity : Entity {
        private float _y;
        private Text TitleText;
        private Text ArtistText;
        private Text DifficultyText;
        private Image ArrowLeft;
        private Image ArrowRight;
        private Song nextSong;
        public bool UpdatingSong { get; private set; } = false;
        public bool OffScreen { get { return Y == Game.Instance.Height + Graphic.Height + 10; } }
        public SongDetailsEntity(Song initialSong, float x, float y) : base(x, y) {
            var imageGraphic = new Image(@"..\..\Sprites\SongDetailsFrame.png");
            imageGraphic.CenterOrigin();
            AddGraphic<Image>(imageGraphic);
            _y = Game.Instance.Height + imageGraphic.Height + 10;
            Tween(this, new { _y = y }, 120, 0).Ease(Ease.ElasticInOut);
            TitleText = new Text(String.Empty, Globals.GeneralFont, 80);
            TitleText.Color = Color.Black;
            TitleText.Y -= 115;
            AddGraphic<Text>(TitleText);
            ArtistText = new Text(String.Empty, Globals.GeneralFont, 80);
            ArtistText.Color = Color.Black;
            ArtistText.Y -= 115;
            AddGraphic<Text>(ArtistText);
            DifficultyText = new Text(String.Empty, Globals.GeneralFont, 80);
            DifficultyText.Color = Color.Black;
            DifficultyText.Y += 40;
            AddGraphic<Text>(DifficultyText);
            ArrowLeft = new Image(@"..\..\Sprites\arrow.png");
            ArrowLeft.Angle = 90f;
            ArrowLeft.X = (-Game.Instance.HalfWidth + 60);
            ArrowLeft.Y += 80;
            ArrowLeft.CenterOrigin();
            AddGraphic<Image>(ArrowLeft);
            ArrowRight = new Image(@"..\..\Sprites\arrow.png");
            ArrowRight.Angle = 270f;
            ArrowRight.X = Game.Instance.HalfWidth - 60;
            ArrowRight.Y += 80;
            ArrowRight.CenterOrigin();
            AddGraphic<Image>(ArrowRight);
            SetTextDetails(initialSong);
        }

        public void ChangeDifficulty(Difficulty difficulty) {
            //▶ for next, ◀ for prev
            DifficultyText.String = difficulty.ToString();
            DifficultyText.CenterTextOriginX();          
        }

        public void ChangeSong(Song newSong) {
            nextSong = newSong;
            Tween(this, new { _y = Game.Instance.Height + Graphic.Height + 10 }, 30, 0).Ease(Ease.ElasticOut);
            UpdatingSong = true;
        }

        public void BringBackToView() {
            SetTextDetails(nextSong);
            Tween(this, new { _y = Game.Instance.Height - 15 }, 30, 0).Ease(Ease.ElasticOut);
            UpdatingSong = false;
        }

        public void ShowDifficulty(bool show) {
            if (show) 
                Tween(this, new { _y = Game.Instance.Height - Graphic.HalfHeight }, 60, 0).Ease(Ease.ElasticOut);
            else
                Tween(this, new { _y = Game.Instance.Height - 15 }, 60, 0).Ease(Ease.ElasticOut);
        }

        public void SetTextDetails(Song newSong) {
            TitleText.String = newSong.Title;
            if (TitleText.Width > 900)
                TitleText.ScaledWidth = 900;
            else
                TitleText.ScaledWidth = TitleText.Width;
            TitleText.CenterTextOriginX();
            TitleText.X = -Graphic.Width / 4;
            ArtistText.String = newSong.Artist;
            if (ArtistText.Width > 900)
                ArtistText.ScaledWidth = 900;
            else
                ArtistText.ScaledWidth = ArtistText.Width;
            ArtistText.CenterTextOriginX();
            ArtistText.X = Graphic.Width / 4;
        }

        public override void Update() {
            base.Update();
            Y = _y;
        }
    }
}
