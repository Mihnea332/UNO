using System;
using System.Collections.Generic;
using System.Linq;
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
        public void ShowHand(Control parent)
        {
            if (parent == null) return;
            parent.Controls.Clear();
            if (Hand == null || Hand.Count == 0) return;
            int cardWidth = 80;
            int cardHeight = 120;
            int spacing = 10;
            int x = 150;
            int y = Math.Max(10, parent.ClientSize.Height - cardHeight - 10);
            int i = 0;
            foreach (Card c in Hand)
            {
                PictureBox pb = new PictureBox
                {
                    Size = new Size(cardWidth, cardWidth),
                    Location = new Point(x, y),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    Name = "card" + i
                };
                try
                {
                    String path = c.GetCardName();
                    if (File.Exists(path))
                    {
                        pb.Image = Image.FromFile(path);
                    }
                    else
                    {
                        pb.BackColor = Color.Gray;
                    }
                }

                catch
                {
                    pb.BackColor = Color.Gray;
                }
                parent.Controls.Add(pb);
                x += cardWidth + spacing;
                i++;

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
        public bool IsCardValid(Card TopCard,Card CardToPlay)
        {
            if (TopCard.color == CardToPlay.color || TopCard.value == CardToPlay.value || CardToPlay.value == Val.Wild || CardToPlay.value == Val.WildDrawFour)
                return true;
            return false;
        }
        public void PlayCard(Card TopCard,Card card)
        {
           
                RemoveCard(card);
        }
       
    }
}