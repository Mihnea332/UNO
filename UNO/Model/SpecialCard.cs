using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace UNO.Model
{
   public class SpecialCard:Card
    {
        public SpecialCard(Colors c,Val value) : base(c, value)
        {

        }
        
        public override string ToString()
        {
            return $"[SpecialCard] {color} {value}";
        }
        public override string GetCardName()
        {
            string ValuePart = value.ToString();
            string ColorPart = color.ToString();


            return Path.Combine(Application.StartupPath, @"..\..\Resources", ColorPart + "_" + ValuePart + ".png");
        }
    }
}
