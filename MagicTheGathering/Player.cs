using System;
using System.Collections.Generic;
using System.Threading;

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
            manaPool = new ManaPool();
        }

        protected virtual void GameStart ()
        {

        }

        internal void ContinueToNextPhase ()
        {
            manaPool.Empty();
            landsPlayed = 0;
            foreach (var item in battlefield)
            {
                item.isUntapped = true;
            }
            var card = deck.DrawTopCard();
            if (card == null)
                Environment.Exit(0);
            hand.Add(new HandCardReference(card));
        }

        internal void Play (HandCardReference card)
        {
            foreach (CostComponent component in card.card.cost.Payment)
            {
                // FIXME: costs should have a simple "try to pay this" function!
                if (component is ManaPaymentComponent)
                {
                    if (!((ManaPaymentComponent)component).TryPayWith(manaPool))
                    {
                        return;
                    }
                }
            }

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
            battlefield.Add(new BattlefieldCardReference(card, this));
        }

        private void StartGame()
        {
            foreach (var item in deck.DrawTopCard(7))
            {
                if (item == null)
                    Environment.Exit(0);
                Hand.Add(new HandCardReference(item));
            }
            Thread thread = new Thread(GameStart);
            thread.Start();
        }

        List<BattlefieldCardReference> battlefield = new List<BattlefieldCardReference>();
        List<HandCardReference> hand = new List<HandCardReference>();
        protected internal MagicGame game;
        internal ManaPool manaPool;
        protected internal Deck deck;
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
        
        protected internal List<HandCardReference> Hand
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