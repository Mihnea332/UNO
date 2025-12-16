using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Logic
{
    public class Message
    {
        public string Type { get; set; }
        public string Color { get; set; }
        public string Value { get; set; }
        public int PlayerId { get; set; }
        public string Payload { get; set; }
    }
}
