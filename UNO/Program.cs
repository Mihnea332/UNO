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
          
         
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());
        }
    }
}
