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
    public class Deck
    {
        List<Card> deck = new List<Card>();
        public List<Card> deck_played = new List<Card>();
        public void Generate()
        {
            foreach (Colors c in Enum.GetValues(typeof(Colors)))
            {
                if (c == Colors.None) continue;
                foreach (Val v in Enum.GetValues(typeof(Val)))
                {
                    if (v <= Val.Nine)
                    {
                        Card carte = new NormalCard(c, v);
                        deck.Add(carte);
                    }
                    else if (v == Val.Reverse || v == Val.DrawTwo || v == Val.Skip)
                    {

                        Card carte = new SpecialCard(c, v);
                        deck.Add(carte);
                    }
                }

                }
                deck.Add(new WildCard(Colors.None, Val.Wild));
                deck.Add(new WildCard(Colors.None, Val.WildDrawFour));
            }
            public void Print()
            {
                foreach (Card c in deck)
                    Console.WriteLine(c.ToString());
            }
            public void Shuffle()
            {
                Random rnd = new Random();
                for (int i = 0; i < deck.Count; i++)
                {
                    int index = rnd.Next(0, deck.Count);
                    Card temp = deck[i];
                    deck[i] = deck[index];
                    deck[index] = temp;
                }

            }
            public void Deal(Player p)
            {
                for (int i = 0; i < 5; i++)
                {
                    p.AddCard(deck[deck.Count - 1]);
                    deck.RemoveAt(deck.Count - 1);
                }
            }
            public void DrawCard(Player p, Card TopCard)
            {
                if (deck.Count == 0)
                {
                    List<Card> deck_temp = new List<Card>();
                    foreach (Card c in deck_played)
                        if (c != TopCard) deck_temp.Add(c);

                    deck_played.Clear();
                    deck = deck_temp;
                    Random rnd = new Random();
                    for (int i = 0; i < deck.Count; i++)
                    {
                        int index = rnd.Next(0, deck.Count);
                        Card temp = deck[i];
                        deck[i] = deck[index];
                        deck[index] = temp;
                    }
                }
                p.AddCard(deck[deck.Count - 1]);
                deck.RemoveAt(deck.Count - 1);

            }
            public Card GetTopCard()
            {
                Card temp = deck[deck.Count - 1];
                deck.RemoveAt(deck.Count - 1);
                return temp;
            }
        public void ShowTopCard(Control parent, Card TopCard)
        {
            if (parent == null) return;
            List<Control> old = parent.Controls.OfType<Control>().Where(c => c.Tag != null && c.Tag.ToString() == "topCard").ToList();
            foreach (Control c in old)
            {
                if (c is PictureBox oldPb)
                {
                    if (oldPb.Image!=null)
                        try
                        {
                            oldPb.Image.Dispose();
                        }
                        catch
                        {
                            oldPb.Image = null;
                        }
                }
                parent.Controls.Remove(c);
                try
                {
                    c.Dispose();
                }
                catch { }
            }
            Card cardToShow;
            if (TopCard != null)
             cardToShow = TopCard;
            else
            {
                if (deck_played.Count > 0)
                    cardToShow = deck_played[deck_played.Count - 1];
                else
                    return;
            }
            int cardWidth = 80;
            int cardHeight = 120;
            int x = (parent.ClientSize.Width - cardWidth) / 2;
            int y = (parent.ClientSize.Height - cardHeight) / 2;
            PictureBox pb = new PictureBox
            {
                Size = new Size(cardWidth, cardHeight),
                Location = new Point(x, y),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = "topCard",
                Name = "topCardBox"

            };
            try
            {
                String path = TopCard.GetCardName();
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
            pb.BringToFront();
        }
        }
    }