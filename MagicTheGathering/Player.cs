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

        private void StartGame()
        {
            hand = new List<MagicCard>(deck.DrawTopCard(7));
        }

        List<MagicCard> hand = new List<MagicCard>();
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
    }
}