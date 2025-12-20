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
                Player p = new Player("Player " + (i + 1));
                deck.Deal(p);
                Players.Add(p);
            }
            TopCard = deck.GetTopCard();
            currentPlayer = Players[0];
        }
        public void AdvanceTurn()
        {
            CurrentPlayerIndex++;
            if (CurrentPlayerIndex >= Players.Count)
                CurrentPlayerIndex = 0;
            currentPlayer = Players[CurrentPlayerIndex];
        }
        public void AfterPlayerPlays(Card playedCard,Colors chosenColor)
        {
            setTopCard(playedCard);
            Player affected = Players[(CurrentPlayerIndex + 1) % Players.Count];
            ApplyEffect(playedCard, chosenColor, affected);
            AdvanceTurn();
        }
        public void NextPlayer()
        {
            if (CurrentPlayerIndex == 0)
                CurrentPlayerIndex = 1;
            else CurrentPlayerIndex = 0;
            currentPlayer = Players[CurrentPlayerIndex];
        }
        public void UpdateColor(Colors ChosenColor)
        {
            TopCard.color = ChosenColor;
        }
         public void ApplyEffect(Card PlayedCard,Colors ChosenColor,Player AffectedPlayer)
        {

            switch (PlayedCard.value)
            {

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
