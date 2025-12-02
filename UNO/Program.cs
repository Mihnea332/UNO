using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using UNO.Logic;
using UNO.Model;

namespace UNO
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Player p1 = new Player("Mihnea");
            Deck d = new Deck();
             d.Generate();
             d.Shuffle();
             d.Print();
             Console.WriteLine();
             d.Deal(p1);
             Console.WriteLine();
             Console.WriteLine();
             d.Print();
         
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
