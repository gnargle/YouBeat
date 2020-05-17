using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouBeat.Entities;
using YouBeatTypes;

namespace YouBeat.Scenes {
    class MenuScene : BaseScene {

        private Dictionary<Song, SongArtworkEntity> songEntities = new Dictionary<Song, SongArtworkEntity>();
        private SongDetailsEntity detailsEntity;

        private enum OffScreenEnum { None = 0, Left = 1, Right = 2 }
        private OffScreenEnum offScreenState = OffScreenEnum.None;
        private Sound NextSound;

        public void OnSongChange(Song newSong, Song prevSong) {
            var songList = songEntities.Keys.ToList();
            var entList = songEntities.Values.ToList();
            if (songEntities.TryGetValue(newSong, out SongArtworkEntity ent)) {
                ent.SetSelectedEntity(true);
            }
            if (songEntities.TryGetValue(prevSong, out ent)) {
                ent.SetSelectedEntity(false);
            }
            if (newSong == songList.First() && prevSong == songList.Last()) {
                //special case handling for jumping back to the start
                _controller.SongSelectActive = false; //disable changing song until everything's back on the screen.
                offScreenState = OffScreenEnum.Left;
                //i think first set them all to jump off the left of the screen, then lay them out as original.
                for (int i = 0; i <= songEntities.Count - 1; i++) {
                    ent = entList[i];
                    ent.SetNewX(-SongArtworkEntity.MainSize - 50); //full size of the big one, plus some padding so it doesn't poke on screen.
                }
            } else if (newSong == songList.Last() && prevSong == songList.First()) {
                //special case handling for jumping back to the end
                _controller.SongSelectActive = false; //disable changing song until everything's back on the screen.
                offScreenState = OffScreenEnum.Right;
                //i think first set them all to jump off the left of the screen, then lay them out as original.
                for (int i = 0; i <= songEntities.Count - 1; i++) {
                    ent = entList[i];
                    ent.SetNewX(Game.Instance.Width + SongArtworkEntity.MainSize + 50);
                }
            } else {                
                bool right = (songList.IndexOf(newSong) > songList.IndexOf(prevSong));
                //right means the next song is TO THE RIGHt, so we need to shift everything LEFT

                if (songEntities.Count > 1) { //can't move if we don't have more than 1
                    for (int i = 0; i <= songEntities.Count - 1; i++) {
                        ent = entList[i];
                        var song = songList[i];
                        float x;
                        if (ent.NewXTweenComplete)
                            x = ent.X;
                        else
                            x = ent.CompleteX;
                        if (song == prevSong) {
                            //if this is the previous song, it needs to move out the centre
                            //the distance between the centre image and others differs, so get the new song's entity
                            //to figure it out.  
                            if (right)
                                ent.SetNewX(x - SongArtworkEntity.MainSize - SongArtworkEntity.SubGap);
                            else
                                ent.SetNewX(x + SongArtworkEntity.MainSize + SongArtworkEntity.SubGap);
                        } else if (song == newSong) {
                            //similar to above, but in reverse.
                            var tempent = songEntities[prevSong];
                            if (right)
                                ent.SetNewX(x - SongArtworkEntity.MainSize - SongArtworkEntity.SubGap);
                            else
                                ent.SetNewX(x + SongArtworkEntity.MainSize + SongArtworkEntity.SubGap);
                        } else {
                            if (right)
                                ent.SetNewX(x - SongArtworkEntity.SubSize);
                            else
                                ent.SetNewX(x + SongArtworkEntity.SubSize);
                        }
                    }
                }
                songEntities.OrderBy(p => p.Value.X);
            }
            detailsEntity.ChangeSong(newSong);
            NextSound.Play();
        }

        public void SetupMenu() {
            _controller.OnSongChange = OnSongChange;
            _controller.OnMenuStateChange = OnMenuStateChange;
            _controller.OnSetDifficulty = OnSetDifficulty;
            AddGraphic<Image>(new Image(@"..\..\Backgrounds\bg.png"));            
            var mainEntity = new SongArtworkEntity(_controller.GetImageFilename(_controller.CurrentSong), Game.Instance.HalfWidth, Game.Instance.HalfHeight, true);
            Add(mainEntity);
            songEntities.Add(_controller.CurrentSong, mainEntity);
            var currX = (Game.Instance.Width / 2) + SongArtworkEntity.MainSize;
            foreach (var song in _controller.Songs) {
                if (song == _controller.CurrentSong)
                    continue;
                var songEnt = new SongArtworkEntity(_controller.GetImageFilename(song), currX, Game.Instance.Height / 2);
                Add(songEnt);
                songEntities.Add(song, songEnt);
                currX = songEnt.X + SongArtworkEntity.SubSize + SongArtworkEntity.SubGap;                        
            }
            songEntities.OrderBy(p => p.Value.X);            
            detailsEntity = new SongDetailsEntity(_controller.CurrentSong, Game.Instance.HalfWidth, Game.Instance.Height - 15);
            Add(detailsEntity);
            NextSound = new Sound(@"..\..\FX\Next.wav");

            _controller.AcceptInput = true; //all tiles generated, now the user can swap them to their content without causing mixups
        }

        private void OnSetDifficulty(Difficulty difficulty) {
            detailsEntity.ChangeDifficulty(difficulty);
        }

        private void OnMenuStateChange(MenuState newMenuState, MenuState prevMenuState) {
            switch (prevMenuState) {
                case MenuState.SongSelect:
                    if (newMenuState == MenuState.DifficultySelect)
                        detailsEntity.ShowDifficulty(true);
                    break;
                case MenuState.DifficultySelect:
                    if (newMenuState == MenuState.SongSelect)
                        detailsEntity.ShowDifficulty(false);
                    break;
            }
        }

        public MenuScene(GameController gameController) : base(gameController) {
            SetupMenu();
        }

        public override void Update() {
            base.Update();
            if (_controller.State == GameState.ReturnToTitle || _controller.State == GameState.Title) {
                Game.SwitchScene(new TitleScene(_controller));
            } else if (_controller.State == GameState.Setup || _controller.State == GameState.PreGameHold) {
                _controller.OnSongChange = null; //dregister the callbacks to prevent problems
                _controller.OnMenuStateChange = null;
                _controller.OnSetDifficulty = null;
                Game.SwitchScene(new GameScene(_controller));
            }         
            if (offScreenState != OffScreenEnum.None) {
                var songList = songEntities.Keys.ToList();
                var entList = songEntities.Values.ToList();
                bool nextTween = true;
                foreach (var ent in songEntities.Values) {
                    if (!ent.NewXTweenComplete) {
                        nextTween = false;
                        break;
                    }
                }
                if (nextTween) {
                    float currX = 0;
                    if (offScreenState == OffScreenEnum.Left) {
                        for (int i = 0; i <= songEntities.Count - 1; i++) {
                            var ent = entList[i];
                            var song = songList[i];
                            ent.X = Game.Instance.Width + 450;
                            if (song == _controller.CurrentSong) {
                                currX = (Game.Instance.Width / 2);
                            } else {
                                if (songList[i - 1] == _controller.CurrentSong)
                                    currX = (Game.Instance.Width / 2) + SongArtworkEntity.MainSize;
                                else
                                    currX = entList[i - 1].CompleteX + SongArtworkEntity.SubSize + SongArtworkEntity.SubGap;
                            }
                            ent.SetNewX(currX);
                        }
                    } else if (offScreenState == OffScreenEnum.Right) {
                        //the same as left but do it the opposite way.
                        for (int i = songEntities.Count - 1; i >= 0; i--) {
                            var ent = entList[i];
                            var song = songList[i];
                            ent.X = Game.Instance.Width + 450;
                            if (song == _controller.CurrentSong) {
                                currX = (Game.Instance.Width / 2);
                            } else {
                                if (songList[i + 1] == _controller.CurrentSong)
                                    currX = (Game.Instance.Width / 2) - SongArtworkEntity.MainSize;
                                else
                                    currX = entList[i + 1].CompleteX - SongArtworkEntity.SubSize + SongArtworkEntity.SubGap;
                            }
                            ent.SetNewX(currX);
                        }
                    }
                    detailsEntity.BringBackToView();
                    _controller.SongSelectActive = true;
                    offScreenState = OffScreenEnum.None;
                }
            } else if (detailsEntity.UpdatingSong) {
                if (detailsEntity.OffScreen)
                    detailsEntity.BringBackToView();
            }
        }
    }
}
