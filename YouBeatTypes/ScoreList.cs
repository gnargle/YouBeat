using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouBeatTypes {
    public class ScoreList {
        public List<Tuple<String, long>> Scores { get; set; }
        public ScoreList() { Scores = new List<Tuple<string, long>>(); }
    }
}
