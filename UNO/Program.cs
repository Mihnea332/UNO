using System;
using System.Collections.Generic;
using System.Linq;
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
            /* Player p1 = new Player("Mihnea");
             Deck d = new Deck();
             d.Generate();
             d.Shuffle();
             d.Print();
             Console.WriteLine();
             d.Deal(p1);
             p1.ShowHand();
             Console.WriteLine();
             Console.WriteLine();

             d.Print();
            
            GameTimer timer = new GameTimer(5000);

            // Pornim timer-ul
            timer.Start();

            Console.WriteLine("Timer pornit. Așteptăm expirarea...");

            // Bucla care așteaptă expirarea
            while (!timer.TimeOut)
            {
                if (timer.TimeOut)
                {
                    Console.WriteLine("Timpul s-a scurs! Programul se închide.");
                    Environment.Exit(0); // 0 = ieșire normală
                }
            }
            */
            Console.WriteLine("Timpul s-a scurs! Testul s-a terminat.");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
