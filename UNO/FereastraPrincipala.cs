using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using UNO.Model;
using UNO.Logic;
namespace UNO
{
    public partial class Form1 : Form
    {
        private Game game;
        public void ShowHand(Player p)
        {
            panelHand.Controls.Clear();
            int x = 0;
            foreach (Card c in p.Hand)
            {
                PictureBox pb = new PictureBox();
                pb.Size = new Size(80, 120); // dimensiune carte
                pb.Location = new Point(x, 0); // poziția în panel
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Image = Image.FromFile(c.GetCardName());
                panelHand.Controls.Add(pb);
                x += 90;
            }
        }
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Player p = new Player("Mihnea");
            Deck d = new Deck();
            d.Generate();
            d.Shuffle();
            d.Deal(p);
           
            ShowHand(p);
        }

        private void pictureBoxTest_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
