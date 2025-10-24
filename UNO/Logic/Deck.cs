using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNO.Model;
namespace UNO.Logic
{
    public class Deck
    {
        List<Card> deck = new List<Card>();
        List<Card> deck_played = new List<Card>();
        public void Generate()
        {
            foreach (Color c in Enum.GetValues(typeof(Color)))
            {
                if (c == Color.None) continue;
                foreach (Val v in Enum.GetValues(typeof(Val)))
                {
                    if (v <= Val.Nine)
                        deck.Add(new NormalCard(c, v));
                    else if (v == Val.Reverse || v == Val.DrawTwo || v == Val.Skip)
                        deck.Add(new SpecialCard(c, v));
                }

            }
            deck.Add(new WildCard(Color.None, Val.Wild));
            deck.Add(new WildCard(Color.None, Val.WildDrawFour));
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
    }
}