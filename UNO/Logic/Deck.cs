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
    public class Deck
    {
        public List<Card> deck;
        public List<Card> deck_played;
       public Deck()
        {
            deck = new List<Card>();
        }
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

                if (deck_temp.Count == 0)
                {
                    MessageBox.Show("Nu mai sunt cărți disponibile pentru tras!");
                    return;
                }

                deck_played.Clear();
                deck_played.Add(TopCard); 
                deck = deck_temp;

               
                Random rnd = new Random();
                for (int i = 0; i < deck.Count; i++)
                {
                    int index = rnd.Next(deck.Count);
                    Card temp = deck[i];
                    deck[i] = deck[index];
                    deck[index] = temp;
                }
            }

            if (deck.Count == 0)
            {
                MessageBox.Show("Pachetul este gol. Nu se poate trage.");
                return;
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
        
    }
}