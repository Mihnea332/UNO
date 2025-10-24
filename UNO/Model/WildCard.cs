using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model
{
   public class WildCard:Card
    {
        public WildCard(Color c, Val v) : base(Color.None, v) { }

        public override string ToString()
        {
            return $"[Wild] {value}";
        }
    }
}
