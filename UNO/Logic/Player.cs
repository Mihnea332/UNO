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
        private string name;
        private List<Card> Hand;
        public List<Card> getHand()
        {
            return Hand;
        }
        public Player(string name)
        {
            Hand = new List<Card>();
            this.name = name;
            
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
            int count = Hand.Count;
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


  

        public bool IsCardValid(Card TopCard, Card CardToPlay) { 
        if (CardToPlay.value == Val.WildDrawFour || CardToPlay.value==Val.Wild)
            
               
                return true;
            if ((TopCard.value == Val.Wild || TopCard.value == Val.WildDrawFour) && TopCard.color == Colors.None)
                return true;

            
            if (TopCard.color == CardToPlay.color || TopCard.value == CardToPlay.value)
                return true;
            return false;
            }
    
       
    }
}