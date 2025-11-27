using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UNO.Model;
namespace UNO.Logic
{
    public class Game
    {
        public Deck deck=new Deck();
        public List<Player> Players;
        public Player Winner;
        public Directions Direction;
        public Card TopCard;
        public int CurrentPlayerIndex;
        public int NrOfPlayers;
        public bool GameOver = false;
        public bool SkipPlayed = false;
        public Player currentPlayer;
        public Game()
        {
            this.NrOfPlayers = 2;
            this.Direction = Directions.Clockwise;

            deck.Generate();
            deck.Shuffle();

            Players = new List<Player>();
            for (int i=0;i<NrOfPlayers;i++)
            {
                Player p = new Player("Player " + (i + 1));
                deck.Deal(p);
                Players.Add(p);
            }
            TopCard = deck.GetTopCard();
            currentPlayer = Players[0];
        }
        
        public void NextPlayer()
        {
            int i = (SkipPlayed==true) ? 2 : 1;
            if (Direction == Directions.Clockwise) CurrentPlayerIndex+=i;
            else CurrentPlayerIndex-=i;
            CurrentPlayerIndex = (CurrentPlayerIndex + Players.Count) % Players.Count;
            SkipPlayed = false;
        }
        public void UpdateColor(Colors ChosenColor)
        {
            TopCard.color = ChosenColor;
        }
         public void ApplyEffect(Card PlayedCard,Colors ChosenColor)
        {
            Player AffectedPlayer = Players[(CurrentPlayerIndex + 1) % Players.Count];
            switch (PlayedCard.value)
            {

                case Val.Skip:
                    SkipPlayed = true;
                    break;
                case Val.Reverse:
                    Direction = (Direction == Directions.Clockwise) ? Directions.CounterClockwise : Directions.Clockwise; break;
                case Val.DrawTwo:
                    deck.DrawCard(AffectedPlayer, TopCard);
                    deck.DrawCard(AffectedPlayer, TopCard); 
                    break;
                case Val.Wild:
                    UpdateColor(ChosenColor); 
                    break;
                case Val.WildDrawFour:
                    UpdateColor(ChosenColor); 
                    for (int i=0;i<4;i++)
                        deck.DrawCard(AffectedPlayer, TopCard);
                    break;
            }
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
    }
}
