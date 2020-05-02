using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeat.Entities;
using YouBeatTypes;

namespace YouBeat {
    class MenuScene : BaseScene {

        private Dictionary<Song, SongArtworkEntity> songEntities = new Dictionary<Song, SongArtworkEntity>();

        public void OnSongChange(Song newSong, Song prevSong) {
            SongArtworkEntity ent;
            if (songEntities.TryGetValue(newSong, out ent)) {
                ent.SetSelectedEntity(true);
            }
            if (songEntities.TryGetValue(prevSong, out ent)) {
                ent.SetSelectedEntity(false);
            }
            var songList = songEntities.Keys.ToList();
            bool right = (songList.IndexOf(newSong) > songList.IndexOf(prevSong));
            //right means the next song is TO THE RIGHt, so we need to shift everything LEFT
            var entList = songEntities.Values.ToList();
            if (songEntities.Count > 1) { //can't move if we don't have more than 1
                for (int i = 0; i <= songEntities.Count - 1; i++) {
                    ent = entList[i];
                    var song = songList[i];
                    if (song == prevSong) {
                        //if this is the previous song, it needs to move out the centre
                        //the distance between the centre image and others differs, so get the new song's entity
                        //to figure it out.
                        var tempent = songEntities[newSong];
                        if (right)
                            ent.SetNewX(ent.X - ent.Graphic.ScaledWidth - 10);
                        else
                            ent.SetNewX(ent.X + ent.Graphic.ScaledWidth + 10);
                    } else if (song == newSong) {
                        //similar to above, but in reverse.
                        var tempent = songEntities[prevSong];
                        if (right)
                            ent.SetNewX(ent.X - tempent.Graphic.ScaledWidth - 10);
                        else
                            ent.SetNewX(ent.X + tempent.Graphic.ScaledWidth + 10);
                    } else {
                        if (right)
                            ent.SetNewX(ent.X - ent.Graphic.ScaledWidth);
                        else
                            ent.SetNewX(ent.X + ent.Graphic.ScaledWidth);
                    }
                }
            }
            songEntities.OrderBy(p => p.Value.X);
        }

        public void SetupMenu() {
            _controller.OnSongChange = OnSongChange;
            AddGraphic<Image>(new Image(@"..\..\Backgrounds\bg.png"));            
            var mainEntity = new SongArtworkEntity(_controller.GetImageFilename(_controller.CurrentSong), Game.Instance.Width / 2, Game.Instance.Height / 2, true);
            Add(mainEntity);
            songEntities.Add(_controller.CurrentSong, mainEntity);
            var currX = (Game.Instance.Width / 2) + mainEntity.Graphic.ScaledWidth;
            var left = false;
            foreach (var song in _controller.Songs) {
                if (song == _controller.CurrentSong)
                    continue;
                var songEnt = new SongArtworkEntity(_controller.GetImageFilename(song), currX, Game.Instance.Height / 2);
                Add(songEnt);
                songEntities.Add(song, songEnt);
                if (left) {
                    currX = songEnt.X - (songEnt.Graphic.ScaledWidth) - 10; //small gap
                    /*if (currX < 0) {
                        break;                        
                    }*/
                } else {
                    currX = songEnt.X + (songEnt.Graphic.ScaledWidth) + 10;
                    /*if (currX > Game.Instance.Width) {
                        currX = (Game.Instance.Width / 2) - mainEntity.Graphic.ScaledWidth;
                        left = true;
                    }*/
                }                
            }
            songEntities.OrderBy(p => p.Value.X);
        }

        public MenuScene(GameController gameController) : base(gameController) {
            SetupMenu();
        }       

        public override void Update() {
            base.Update();       
            if (_controller.State == GameState.ReturnToTitle || _controller.State == GameState.Title) {
                Game.SwitchScene(new TitleScene(_controller));
            }
        }
    }
}
