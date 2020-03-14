using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeatTypes {
    public class Song {
        //serialisable song class. This is a manifest of the beatmap data, files for the UI and to load the song.
        public string Title { get; set; }
        public string Artist { get; set; }
        public string ImageFileName { get; set; }
        public string FileName { get; set; }
        public List<Beat> Beats { get; set; }
        public Song() {

        }
    }
}
