using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using UNO.Model;

namespace UNO.Logic
{
    public class Player
    {
        string Name;
        public List<Card> Hand = new List<Card>();
        public event Action<Card> CardClicked;
        public Player(string Name)
        {
            this.Name = Name;
            
        }
        public void AddCard(Card card)
        {
            Hand.Add(card);
        }
        public void RemoveCard(Card card)
        {
            Hand.Remove(card);
           
        }
        public void ShowHand(Control parent, EventHandler clickHandler)
        {
            if (parent == null || Hand == null || Hand.Count == 0) return;

            parent.Controls.Clear();
            int count = Hand.Count();
            int cardWidth = 80;
            int cardHeight = 120;
            int spacing = -150;
            int x = 500;
            int y = 10;
            if (count > 6)
            {
                y = 10 +count;
                x = 500 + count * 3;
                spacing += count*5;
                cardWidth = 80 - count * 2;
                cardHeight = 120 - count * 3;
            }
            for (int i = 0; i < Hand.Count; i++)
            {
                Card c = Hand[i];

                PictureBox pb = new PictureBox
                {
                    Size = new Size(cardWidth, cardHeight),
                    Location = new Point(x, y),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    Tag = c  
                };

                string path = c.GetCardName();
                if (File.Exists(path))
                {
                    pb.Image = Image.FromFile(path);
                }
                else
                {
                    pb.BackColor = Color.Gray;
                }

                pb.Click += clickHandler;
                parent.Controls.Add(pb);

                x += cardWidth + spacing;
            }
        }


        public void ShowHand()
        {
            foreach (Card c in Hand)
                Console.Write(c + " ");
        }
        public bool HasPlayableCard(Card TopCard)
        {
            foreach (var x in Hand)
                if (x.color == TopCard.color || x.value == TopCard.value) return true;
            return false;
        }
        public bool IsCardValid(Card TopCard, Card CardToPlay)
        {
            if (TopCard.color == CardToPlay.color || TopCard.value == CardToPlay.value || CardToPlay.value == Val.Wild || CardToPlay.value == Val.WildDrawFour||TopCard.value==Val.Wild)
                return true;
            return false;
        }
        public void PlayCard(Card TopCard,Card card)
        {
           
                RemoveCard(card);
        }
       
    }
}