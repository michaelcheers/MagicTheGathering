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

        private void StartGame()
        {
            foreach (var item in deck.DrawTopCard(7))
            {
                hand.Add(new HandCardReference(item.Card));
            }
        }

        List<HandCardReference> hand = new List<HandCardReference>();
        MagicGame game;
        Deck deck;
        int landsPlayed = 0;

        public IEnumerable<HandCardReference> Hand { get { return hand; } }

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