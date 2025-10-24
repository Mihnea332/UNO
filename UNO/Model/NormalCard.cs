using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model
{
    public class NormalCard:Card
    {
        public NormalCard(Color color, Val value) : base(color, value)
        { }
        public override string ToString()
        {
            return $"{color} {value}";
        }

    }
}
