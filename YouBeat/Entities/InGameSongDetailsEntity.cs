using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeat.Entities {
    class InGameSongDetailsEntity : Entity {

        public InGameSongDetailsEntity(String artworkpath, float x, float y) : base(x, y) {
            //this will have the artwork, song title and artist name all as one sidebar.
        }
    }
}
