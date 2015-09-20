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

        internal void ContinueToNextPhase ()
        {
            landsPlayed = 0;
        }

        internal void Play (HandCardReference card)
        {
            switch (card.Card.Type)
            {
                case MagicCardType.Creature:
                    break;
                case MagicCardType.Artifact:
                    break;
                case MagicCardType.Basic:
                    break;
                case MagicCardType.Enchantment:
                    break;
                case MagicCardType.Instant:
                    break;
                case MagicCardType.Land:
                    {
                        if (LandsPlayed >= 1)
                            return;
                        landsPlayed++;
                        break;
                    }
                case MagicCardType.Legendary:
                    break;
                case MagicCardType.Planeswalker:
                    break;
                case MagicCardType.Snow:
                    break;
                default:
                    break;
            }
            hand.Remove(card);
            battlefield.Add(new BattlefieldCardReference(card));
        }

        private void StartGame()
        {
            foreach (var item in deck.DrawTopCard(7))
            {
                Hand.Add(new HandCardReference(item));
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

        public List<BattlefieldCardReference> Battlefield
        {
            get
            {
                return battlefield;
            }

            private set
            {
                battlefield = value;
            }
        }

    }
}