using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeatTypes {
    public class Song {
        private long _LeadInTime;
        [Browsable(false)]
        public long LastLeadInTime = -1;
        //serialisable song class. This is a manifest of the beatmap data, files for the UI and to load the song.
        public string Title { get; set; }
        public string Artist { get; set; }
        [ReadOnly(true)]
        public string ImageFileName { get; set; }
        [ReadOnly(true)]
        public string FileName { get; set; }
        [Browsable(false)]
        public string SongName { get; set; }
        public int BPM { get; set; }
        [DisplayName("Lead In Time"), Description("The length silence to lead into the track.")]
        public long LeadInTime { get { return _LeadInTime; } set {
                if (_LeadInTime != 0 && LeadInTimeGenerated) {
                    LastLeadInTime = _LeadInTime;
                    LeadInTimeGenerated = false;
                }
                _LeadInTime = value;
            }
        }
        [Browsable(false)]
        public bool LeadInTimeGenerated { get; set; }
        [DisplayName("Preview Start"), Description("The position in the track to start the audio preview from in the menu.")]
        public long PreviewStart { get; set; }
        [DisplayName("Preview End"), Description("The position in the track to end the audio preview in the menu.")]
        public long PreviewEnd { get; set; }
        [DisplayName("Easy Beats")]
        public List<Beat> EasyBeats { get; set; }
        [DisplayName("Advanced Beats")]
        public List<Beat> AdvancedBeats { get; set; }
        [DisplayName("Expert Beats")]
        public List<Beat> ExpertBeats { get; set; }
        public Song() {

        }
    }
}
