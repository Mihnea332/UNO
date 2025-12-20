using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Logic
{
    public class GameStateMessage
    {
        public string Type { get; set; } = "STATE";
        public string TopColor { get; set; }
        public string TopValue { get; set; }
        public List<string> HandColors { get; set; }
        public List<string> HandValues { get; set; }
        public int OpponentCardCount { get; set; }
        public int CurrentPlayerIndex { get; set; }

    }
}
