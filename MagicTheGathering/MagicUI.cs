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
            CardReference card;
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
                if(hovered)
                {
                    int HOVER_BULGE = 10;
                    currentRect = new Rectangle(desiredRect.X- HOVER_BULGE, desiredRect.Y-HOVER_BULGE, desiredRect.Width+HOVER_BULGE*2, desiredRect.Height+HOVER_BULGE*2);
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
        int handHoverIdx;
        int showingGameState = 0;

        Dictionary<CardReference, UICard> gameStateRepresentation = new Dictionary<CardReference, UICard>();
        UICard hoveredCard;

        public MagicUI(Player viewingPlayer)
        {
            this.viewingPlayer = viewingPlayer;
        }

        public void NewGameState()
        {
            showingGameState++;

            IEnumerable<HandCardReference> hand = viewingPlayer.Hand;
            foreach (HandCardReference c in hand)
            {
                if (!gameStateRepresentation.ContainsKey(c))
                {
                    gameStateRepresentation[c] = new UICard(c, showingGameState);
                }
                else
                {
                    gameStateRepresentation[c].SetGameState(showingGameState);
                }
            }

            foreach (var kv in gameStateRepresentation.Where(kv => !kv.Value.Active(showingGameState)).ToList())
            {
                gameStateRepresentation.Remove(kv.Key);
            }
        }

        public void Update(Input.InputState inputState, Rectangle screenSize)
        {
            // for now, let's refresh it every frame
            NewGameState();

            LayOutHand(new Rectangle(0, screenSize.Height - 100, screenSize.Width, 100), viewingPlayer.Hand);

            Vector2 mousePos = inputState.MousePos;
            if (hoveredCard != null)
            {
                if(!hoveredCard.Contains(mousePos))
                    hoveredCard = null;
            }

            foreach (UICard c in gameStateRepresentation.Values)
            {
                if (hoveredCard == null && c.Contains(mousePos))
                {
                    hoveredCard = c;
                }
                c.Update(hoveredCard == c);
            }
            
            if(hoveredCard != null && inputState.WasMouseLeftJustPressed())
            {
//                Play(hoveredCard);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (UICard c in gameStateRepresentation.Values)
            {
                c.Draw(spriteBatch);
            }
        }

        void LayOutHand(Rectangle bounds, IEnumerable<CardReference> hand)
        {
            float handSpacing = handCardSize.X;
            int numCards = hand.Count();
            if (numCards > 1)
            {
                float overlapSpacing = (bounds.Width - handCardSize.X) / (numCards - 1);
                if (handSpacing > overlapSpacing)
                    handSpacing = overlapSpacing;
            }
            float totalWidth = handCardSize.X + handSpacing * (numCards - 1);

            Vector2 handStartPos = new Vector2(bounds.Left + (bounds.Width - totalWidth) * 0.5f, bounds.Top);
            int handIdx = 0;
            foreach (CardReference c in hand)
            {
                Rectangle rect = new Rectangle((int)(handStartPos.X + (handSpacing * handIdx)), (int)handStartPos.Y, (int)handCardSize.X, (int)handCardSize.Y);
                gameStateRepresentation[c].SetDesiredPos(new Rectangle((int)(handStartPos.X+(handSpacing*handIdx)), (int)handStartPos.Y, (int)handCardSize.X, (int)handCardSize.Y));
                handIdx++;
            }
        }
    }
}
