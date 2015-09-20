using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class MagicUI
    {
        class UICard
        {
            Rectangle desiredRect;
            Rectangle currentRect;
            internal CardReference card;
            int currentState;

            public bool Active(int state) { return currentState == state; }

            public UICard(CardReference card, int showingGameState)
            {
                this.card = card;
                this.currentState = showingGameState;
            }

            public void SetGameState(int newState)
            {
                this.currentState = newState;
            }

            public void SetDesiredPos(Rectangle rect)
            {
                this.desiredRect = rect;
            }

            public bool Contains(Vector2 pos)
            {
                return currentRect.Contains(pos);
            }

            public void Update(bool hovered)
            {
                if (hovered)
                {
                    int HOVER_BULGE = desiredRect.Height/10;
                    currentRect = new Rectangle(desiredRect.X - HOVER_BULGE, desiredRect.Y - HOVER_BULGE, desiredRect.Width + HOVER_BULGE * 2, desiredRect.Height + HOVER_BULGE * 2);
                }
                else
                {
                    currentRect = desiredRect;
                }
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                card.card.Draw(spriteBatch, currentRect);
            }
        }

        Player viewingPlayer;

        readonly Vector2 handCardSize = new Vector2(75, 100);
        readonly Vector2 battlefieldCardSize = new Vector2(60, 80);
        readonly Vector2 battlefieldSpacing = new Vector2(5, 10);
        int showingGameState = 0;

        Dictionary<CardReference.CardID, UICard> gameStateRepresentation = new Dictionary<CardReference.CardID, UICard>();
        UICard hoveredCard;

        public MagicUI(Player viewingPlayer)
        {
            this.viewingPlayer = viewingPlayer;
        }

        public void NewGameState()
        {
            showingGameState++;

            RefreshCards(viewingPlayer.Hand);
            RefreshCards(viewingPlayer.Battlefield);

            // remove the ones that didn't get refreshed
            foreach (var kv in gameStateRepresentation.Where(kv => !kv.Value.Active(showingGameState)).ToList())
            {
                gameStateRepresentation.Remove(kv.Key);
            }
        }

        public void RefreshCards(IEnumerable<CardReference> cards)
        {
            foreach (CardReference c in cards)
            {
                if (!gameStateRepresentation.ContainsKey(c.cardID))
                {
                    gameStateRepresentation[c.cardID] = new UICard(c, showingGameState);
                }
                else
                {
                    UICard uiCard = gameStateRepresentation[c.cardID];
                    uiCard.card = c;
                    uiCard.SetGameState(showingGameState);
                }
            }
        }

        public void Update(Input.InputState inputState, Rectangle screenSize)
        {
            // for now, let's refresh the state every frame
            NewGameState();

            LayOutArea(new Rectangle(0, screenSize.Height - 100, screenSize.Width, 100), viewingPlayer.Hand, handCardSize, 0);
            int battlefieldHeight = (screenSize.Height - 130) / 2;
            LayOutBattlefield(new Rectangle(0, battlefieldHeight, screenSize.Width, battlefieldHeight), viewingPlayer.Battlefield);

            Vector2 mousePos = inputState.MousePos;
            if (hoveredCard != null)
            {
                if (!hoveredCard.Contains(mousePos))
                    hoveredCard = null;
            }
            bool hoveredCardMissing = (hoveredCard != null);

            foreach (UICard c in gameStateRepresentation.Values)
            {
                if (c == hoveredCard)
                    hoveredCardMissing = false;
                else if (hoveredCard == null && c.Contains(mousePos))
                    hoveredCard = c;

                c.Update(hoveredCard == c);
            }

            if (hoveredCardMissing)
                hoveredCard = null;

            if (hoveredCard != null && inputState.WasMouseLeftJustPressed())
            {
                viewingPlayer.Play(hoveredCard.card);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (UICard c in gameStateRepresentation.Values)
            {
                c.Draw(spriteBatch);
            }
        }

        void LayOutArea(Rectangle bounds, IEnumerable<CardReference> cards, Vector2 cardSize, float cardSpacing)
        {
            float actualSpacing = cardSize.X + cardSpacing;
            int numCards = cards.Count();
            if (numCards > 1)
            {
                float overlapSpacing = (bounds.Width - cardSize.X) / (numCards - 1);
                if (actualSpacing > overlapSpacing)
                    actualSpacing = overlapSpacing;
            }
            float totalWidth = cardSize.X + actualSpacing * (numCards - 1);

            Vector2 layoutStartPos = new Vector2(bounds.Left + (bounds.Width - totalWidth) * 0.5f, bounds.Top);
            int cardIdx = 0;
            foreach (CardReference c in cards)
            {
                Rectangle rect = new Rectangle((int)(layoutStartPos.X + (actualSpacing * cardIdx)), (int)layoutStartPos.Y, (int)cardSize.X, (int)cardSize.Y);
                gameStateRepresentation[c.cardID].SetDesiredPos(new Rectangle((int)(layoutStartPos.X + (actualSpacing * cardIdx)), (int)layoutStartPos.Y, (int)cardSize.X, (int)cardSize.Y));
                cardIdx++;
            }
        }

        void LayOutBattlefield(Rectangle bounds, IEnumerable<CardReference> permanents)
        {
            List<CardReference> lands = new List<CardReference>();
            List<CardReference> creatures = new List<CardReference>();
            List<CardReference> miscPermanents = new List<CardReference>();
            /*            foreach (CardReference c in permanents)
                        {
                            if(c.isCreature)
                                creatures.Add(c);
                            else if(c.isLand)
                                lands.Add(c);
                            else
                                miscPermanents.Add(c);
                        }*/
            lands.AddRange(permanents);

            float battlefieldScale = 1.0f;
            float battlefieldNeededHeight = battlefieldCardSize.Y * 2 + battlefieldSpacing.Y;
            if (bounds.Height < battlefieldNeededHeight)
            {
                battlefieldScale = bounds.Height / battlefieldNeededHeight;
            }

            LayOutArea(new Rectangle(bounds.Left, (int)(bounds.Bottom - battlefieldCardSize.Y), bounds.Width, (int)battlefieldCardSize.Y), lands, battlefieldCardSize, battlefieldSpacing.X);

/*                Vector2 battlefieldStartPos = new Vector2(bounds.Left + (bounds.Width - totalWidth) * 0.5f, bounds.Top);
            int cardIdx = 0;
            foreach (CardReference c in permanents)
            {
                Rectangle rect = new Rectangle((int)(battlefieldStartPos.X + (handSpacing * handIdx)), (int)handStartPos.Y, (int)handCardSize.X, (int)handCardSize.Y);
                gameStateRepresentation[c.cardID].SetDesiredPos(new Rectangle((int)(handStartPos.X + (handSpacing * handIdx)), (int)handStartPos.Y, (int)handCardSize.X, (int)handCardSize.Y));
                handIdx++;
            }*/
        }
    }
}