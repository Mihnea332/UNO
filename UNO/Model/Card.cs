using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
namespace UNO.Model
{

    abstract public class Card
    {

        public Colors color {  get; set; }
        public Val value { get; set; }
      
    public Card(Colors color, Val value)
        {
            this.color = color;
            this.value = value;

        }
        public abstract override string ToString();
        public abstract string GetCardName();
        /*{
            string ValuePart =value.ToString();
            string ColorPart =color.ToString();
            if (ValuePart==Val.Wild.ToString() || ValuePart==Val.WildDrawFour.ToString() ) return Path.Combine(Application.StartupPath, @"..\..\Resources", ValuePart+".png");

            return Path.Combine(Application.StartupPath, @"..\..\Resources", ColorPart + "_" + ValuePart + ".png");
        }*/
       
        
    }
}
