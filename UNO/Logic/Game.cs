using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UNO.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
namespace UNO.Logic
{
    public class Game
    {
         private Deck deck;
        private List<Player> Players;
          private Card TopCard;
        private int CurrentPlayerIndex;
        private Player currentPlayer;
        public Deck getdeck()
        {
            return deck;
        }
        public List<Player> getPlayers()
        {
            return Players;
        }
        public Card getTopCard()
        {
            return TopCard;
        }
        public void setTopCard(Card TopCard)
        {
            this.TopCard = TopCard;
        }
        public int getCurrentPlayerIndex()
        {
            return CurrentPlayerIndex;
        }
        public void setCurrentPlayerIndex(int CurrentPlayerIndex)
        {
            this.CurrentPlayerIndex = CurrentPlayerIndex;
        }
        public Player getcurrentPlayer()
        {
            return currentPlayer;
        }
        public void setcurrentPlayer(Player currentPlayer)
        {
            this.currentPlayer = currentPlayer;
        }
        public Game()
        {

            deck = new Deck();
            deck.Generate();
            deck.Shuffle();

            Players = new List<Player>();
            for (int i=0;i<2;i++)
            {
                Player p = new Player(i.ToString());
                deck.Deal(p);
                Players.Add(p);
            }
            TopCard = deck.GetTopCard();
            currentPlayer = Players[0];
        }
        

        public void UpdateColor(Colors ChosenColor)
        {
            TopCard.color = ChosenColor;
        }
         
        public void ShowTopCard(Control parent)
        {
            parent.Controls.Clear();
            if (TopCard == null) return;
                PictureBox pb = new PictureBox
            {
                Size = new Size(80, 120),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Tag = TopCard

            };


            string path = TopCard.GetCardName();
            if (File.Exists(path))
                pb.Image = Image.FromFile(path);
            else
                pb.BackColor = Color.Gray;

            parent.Controls.Add(pb);
        }
        public void ShowOpponentHand(Control parent, int count)
        {
            parent.Controls.Clear();
            int spacing = -100;
            int x = 450;
            int y = 10;
           
            for (int i = 0; i < count; i++)
            {
                PictureBox pb = new PictureBox
                {
                    Size = new Size(60, 90),
                    Location = new Point(x, y),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    BorderStyle = BorderStyle.FixedSingle 
                };

                
                if (File.Exists(@"..\..\Resources\Deck.png"))
                {
                    
                    pb.Image = Image.FromFile(@"..\..\Resources\Deck.png");
                }
                else
                {
                    pb.BackColor = Color.OrangeRed; // O culoare vizibilă dacă nu găsește poza
                }

               
                parent.Controls.Add(pb);
                // -----------------------------

                pb.BringToFront();

                x += 60 + spacing;
            }
        }
    }
}
