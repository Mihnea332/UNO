using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;

namespace UNO.Logic
{
    public class GameStateDto
    {
        public string TopColor { get; set; }
        public string TopValue { get; set; }
        public int CurrentPlayerId { get; set; }
        public List<string> ClientHand { get; set; }
        public List<int> OpponentHands { get; set; }
        public int DeckCount { get; set; }
    }
}
