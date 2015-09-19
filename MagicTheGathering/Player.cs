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

        protected void Play (HandCardReference card)
        {

        }

        private void StartGame()
        {
            foreach (var item in deck.DrawTopCard(7))
            {
                Hand.Add(new HandCardReference(item.Card));
            }
        }

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

        protected List<HandCardReference> Hand
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