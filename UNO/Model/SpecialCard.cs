using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model
{
   public class SpecialCard:Card
    {
        public SpecialCard(Color c,Val value) : base(c, value)
        {

        }
        
        public override string ToString()
        {
            return $"[SpecialCard] {color} {value}";
        }
    }
}
