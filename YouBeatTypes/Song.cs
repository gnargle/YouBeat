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
        public string SongName { get; set; }
        public int BPM { get; set; }
        public long LeadInTime { get; set; }
        public bool LeadInTimeGenerated;
        public List<Beat> EasyBeats { get; set; }
        public List<Beat> AdvancedBeats { get; set; }
        public List<Beat> ExpertBeats { get; set; }
        public Song() {

        }
    }
}
