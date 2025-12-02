using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
namespace UNO.Model
{

    abstract public class Card
    {

        public Colors color;
        public Val value;
      
    public Card(Colors color, Val value)
        {
            this.color = color;
            this.value = value;

        }
        public abstract override string ToString();
        public abstract string GetCardName();
    
       
        
    }
}
