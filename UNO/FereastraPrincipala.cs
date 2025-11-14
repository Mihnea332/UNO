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
        private Player currentPlayer;
        private Deck deck;
        private Panel panelHand;
        private Panel panelTopCard;

        public Form1()
        {
            InitializeComponent();
            
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedCard = sender as PictureBox;
            if (clickedCard == null) return;

            Card selectedCard = clickedCard.Tag as Card;
            if (selectedCard == null) return;

            Card topCard = deck.deck_played.LastOrDefault();
            if (topCard == null) return;

            if (currentPlayer.IsCardValid(topCard, selectedCard))
            {
                currentPlayer.RemoveCard(selectedCard);
                deck.deck_played.Add(selectedCard);

                deck.ShowTopCard(panelTopCard, selectedCard);
                currentPlayer.ShowHand(panelHand, PictureBox_Click);

            }
            else
            {
                MessageBox.Show("Cartea nu este validă!");
            }
           
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            panelHand = panelHandControl;
            panelTopCard = panelTopCardControl;

            currentPlayer = new Player("Mihnea");
            deck = new Deck();

            deck.Generate();
            deck.Shuffle();
            deck.Deal(currentPlayer);

            Card topCard = deck.GetTopCard();
            deck.deck_played.Add(topCard);

            currentPlayer.ShowHand(panelHand, PictureBox_Click);
            deck.ShowTopCard(panelTopCard, topCard);

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

        private void button1_Click(object sender, EventArgs e)
        {
            Card topCard = deck.deck_played.LastOrDefault();
            deck.DrawCard(currentPlayer, topCard);
            currentPlayer.ShowHand(panelHand, PictureBox_Click);
        }
    }
}
