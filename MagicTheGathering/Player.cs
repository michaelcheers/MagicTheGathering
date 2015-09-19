using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MagicTheGathering
{
    internal abstract class Player
    {
        public virtual void Draw() { }
        public virtual void Update() { }
        public Player (MagicGame game, Deck deck)
        {
            game.Add(this);
            game.StartGame += StartGame;
            this.game = game;
            this.deck = deck;
        }

        internal void Play (CardReference card)
        {
            if (card is HandCardReference)
            {
                HandCardReference hcard = (HandCardReference)card;
                hand.Remove(hcard);
                battlefield.Add(new BattlefieldCardReference(card.Card));
            }
        }

        private void StartGame()
        {
            foreach (var item in deck.DrawTopCard(7))
            {
                Hand.Add(new HandCardReference(item.Card));
            }
        }

        List<BattlefieldCardReference> battlefield = new List<BattlefieldCardReference>();
        List<HandCardReference> hand = new List<HandCardReference>();
        MagicGame game;
        Deck deck;
        int landsPlayed = 0;

        protected int LandsPlayed
        {
            get
            {
                return landsPlayed;
            }

            private set
            {
                landsPlayed = value;
            }
        }
        
        public List<HandCardReference> Hand
        {
            get
            {
                return hand;
            }

            private set
            {
                hand = value;
            }
        }
    }
}