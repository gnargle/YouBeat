using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeatTypes {
    public class Beat {
        public long HitTime { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Beat() {

        }
        public Beat(long hitTime, int _x, int _y) {
            HitTime = hitTime;
            x = _x;
            y = _y;
        }
    }
}
