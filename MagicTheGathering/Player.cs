using MagicTheGatheringUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public bool IsAttacking(BattlefieldCardReference reference)
        {
            foreach(KeyValuePair<BattlefieldCardReference, Player> kv in attacking)
            {
                if (kv.Key == reference)
                    return true;
            }
            return false;
        }

        protected internal void DeclareAttacker (BattlefieldCardReference reference, Player toAttack)
        {
            if (reference.controller != this)
                return;
            attacking.Add(new KeyValuePair<BattlefieldCardReference, Player>(reference, toAttack));
        }

        protected internal void WithdrawAttacker (BattlefieldCardReference reference)
        {
            for (int n = 0; n < attacking.Count; n++)
            {
                var item = attacking[n];
                if (item.Key == reference)
                    attacking.RemoveAt(n);
            }
        }

        protected virtual void GameStart ()
        {

        }

        internal List<KeyValuePair<BattlefieldCardReference, Player>> attacking = new List<KeyValuePair<BattlefieldCardReference, Player>>();

        internal void ContinueToNextTurn()
        {
            game.currentPhase = Phase.Main; // should be Untap

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

        internal void ContinueToNextPhase ()
        {
            manaPool.Empty();

            switch (game.currentPhase)
            {
                case Phase.Main:
                    break;
                case Phase.Attack:
                    {
                        foreach (var item in attacking)
                        {
                            item.Value.lifeTotal -= item.Key.Power;
                            item.Key.isTapped = true;
                        }
                        attacking.Clear();
                        break;
                    }
                case Phase.Main2:
                    break;
                default:
                    break;
            }

            if (game.currentPhase == Phase.Main2) // should be Cleanup
            {
                ContinueToNextTurn();
            }
            else
            {
                game.currentPhase++;
            }
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

            if (card.card.Type.HasFlag(MagicCardType.Land))
            {
                if (LandsPlayed >= 1)
                    return;
                landsPlayed++;
            }
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
        internal int lifeTotal = 20;

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

        //int SortBattleField (BattlefieldCardReference a, BattlefieldCardReference b)
        //{
        //    return a.Card.Type.CompareTo(b.Card.Type);
        //}

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

        public void DrawInfo(SpriteBatch spriteBatch, SpriteFont font, Vector2 position)
        {
            spriteBatch.DrawString(font, "Life: " + lifeTotal, position, Color.White);
            MagicUI.DrawCardBack(spriteBatch, new Rectangle((int)position.X, (int)position.Y + 20, 20,30));
            spriteBatch.DrawString(font, "" + deck.Length, new Vector2(position.X+25, position.Y+25), Color.White);
        }
    }
}