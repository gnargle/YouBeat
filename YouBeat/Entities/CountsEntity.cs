using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeatTypes;

namespace YouBeat.Entities {
    class CountsEntity : Entity {
        private Text _countsText;
        private const string countsStr = "Perfects: {0}\tGreats: {1}\tGoods: {2}\t OKs: {3}\t Bads: {4}\t Misses: {5}";
        private float _alpha = 0;
        private int _misses = 0;
        private int _bads = 0;
        private int _oks = 0;
        private int _goods = 0;
        private int _greats = 0;
        private int _perfects = 0;
        public CountsEntity(float x, float y) : base(x,y) {
            _countsText = new Text(String.Format(countsStr, 0, 0, 0, 0, 0, 0), Globals.GeneralFont, 36) {
                Color = Color.Black
            };
            _countsText.CenterTextOriginX();
            AddGraphic<Text>(_countsText);
            Tween(this, new { _alpha = 1 }, 120).Ease(Ease.QuadIn);
        }
        public override void Update() {
            base.Update();
            Graphic.Alpha = _alpha;
        }
        public void UpdateCounts(GameController gameController) {
            if (CountsHaveChanged(gameController)) {
                _countsText.String = String.Format(countsStr, gameController.Perfects, gameController.Greats, gameController.Goods, gameController.OKs, gameController.Bads, gameController.Misses);
                UpdateInternalCounts(gameController);
            }
        }

        private void UpdateInternalCounts(GameController gameController) {
            _misses = gameController.Misses;
            _bads = gameController.Bads;
            _oks = gameController.OKs;
            _goods = gameController.Goods;
            _greats = gameController.Greats;
            _perfects = gameController.Perfects;
        }

        private bool CountsHaveChanged(GameController gameController) {
            if (gameController.Misses != _misses) return true;
            if (gameController.Bads != _bads) return true;
            if (gameController.OKs != _oks) return true;
            if (gameController.Goods != _goods) return true;
            if (gameController.Greats != _greats) return true;
            if (gameController.Perfects != _perfects) return true;
            return false;
        }
    }
}
