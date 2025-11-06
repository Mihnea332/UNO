using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace UNO.Model
{
    public class NormalCard:Card
    {
        public NormalCard(Colors color, Val value) : base(color, value)
        { }
        public override string ToString()
        {
            return $"{color} {value}";
        }
        public override string  GetCardName()
        {
            string ValuePart =value.ToString();
            string ColorPart =color.ToString();
            

            return Path.Combine(Application.StartupPath, @"..\..\Resources", ColorPart + "_" + ValuePart + ".png");
        }

    }
}
