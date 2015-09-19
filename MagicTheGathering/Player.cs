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

        protected void Play (HandCardReference card)
        {
            hand.Remove(card);
            battlefield.Add(new BattlefieldCardReference(card.Card));
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
        public void DrawHand(SpriteBatch spriteBatch, Rectangle rect)
        {
            Vector2 handItemSize = new Vector2(75, 100);
            float spacing = handItemSize.X;
            if (hand.Count > 1)
            {
                float overlapSpacing = (rect.Width - handItemSize.X) / (hand.Count - 1);
                if (spacing > overlapSpacing)
                    spacing = overlapSpacing;
            }
            float totalWidth = handItemSize.X + spacing * (hand.Count - 1);

            Vector2 currentPos = new Vector2(rect.Left + (rect.Width - totalWidth)*0.5f, rect.Top);
            foreach (HandCardReference c in hand)
            {
                c.Card.Draw(spriteBatch, new Rectangle((int)currentPos.X, (int)currentPos.Y, (int)handItemSize.X, (int)handItemSize.Y));
                currentPos.X += spacing;
            }
        }
    }
}