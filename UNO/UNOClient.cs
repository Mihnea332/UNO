using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNO
{
    public partial class UNOClient : Form
    {
        public UNOClient()
        {
            InitializeComponent();
            this.FormClosed += (s, e) => Application.Exit();
        }
    }
}
