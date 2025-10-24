using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Model
{

    abstract public class Card
    {
        
      public Color color { get; set; }
      public Val value { get; set; }
        protected Card(Color color, Val value)
        {
            this.color = color;
            this.value = value;

        }
        public abstract override string ToString();
        
       
        
    }
}
