
using System.IO;

using System.Windows.Forms;
namespace UNO.Model
{
   public class WildCard:Card
    {
        public WildCard(Colors c, Val v) : base(Colors.None, v) { }

        public override string ToString()
        {
            return $"[Wild] {value}";
        }
        public override string GetCardName()
        {
            string ValuePart = value.ToString();
            string ColorPart = color.ToString();


            return Path.Combine(Application.StartupPath, @"..\..\Resources", ValuePart + ".png");
        }
    }
}
