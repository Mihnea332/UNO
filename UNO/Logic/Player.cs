using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNO.Model;

namespace UNO.Logic
{
    public class Player
    {
        string Name;
        public List<Card> Hand = new List<Card>();
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