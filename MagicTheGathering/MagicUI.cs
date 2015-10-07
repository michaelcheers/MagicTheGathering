using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class MagicUI
    {
        class UICard
        {
            Rectangle animTargetRect;
            Rectangle animStartRect;
            Rectangle currentRect;
            bool hovered;
            internal CardReference card;
            int currentState;
            const int animDuration = 15;
            int animCurrentFrame;

            public bool Active(int state) { return currentState == state; }

            public UICard(CardReference card, int showingGameState)
            {
                this.card = card;
                currentState = showingGameState;
            }

            public void SetGameState(int newState)
            {
                currentState = newState;
            }

            public void SetDesiredPos(Rectangle rect)
            {
                if(animTargetRect != rect)
                {
                    animTargetRect = rect;
                    animStartRect = currentRect;
                    animCurrentFrame = 0;
                }
            }

            public bool Contains(Vector2 pos)
            {
                return currentRect.Contains(pos);
            }

            public void Update(bool hovered)
            {
                this.hovered = hovered;

                if (animCurrentFrame < animDuration)
                {
                    animCurrentFrame++;
                    if (animCurrentFrame == animDuration)
                    {
                        currentRect = animTargetRect;
                    }
                    else
                    {
                        float fraction = (float)animCurrentFrame / animDuration;

                        currentRect = new Rectangle(
                            Lerp(animStartRect.X, animTargetRect.X, fraction),
                            Lerp(animStartRect.Y, animTargetRect.Y, fraction),
                            Lerp(animStartRect.Width, animTargetRect.Width, fraction),
                            Lerp(animStartRect.Height, animTargetRect.Height, fraction)
                        );
                    }
                }
            }

            public int Lerp(int a, int b, float fraction)
            {
                return (int)(a + fraction * (b-a));
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                if (hovered)
                {
                    if (card.Location == CardReference.CardLocation.Hand)
                    {
                        int HOVER_BULGE = animTargetRect.Height / 10;
                        Rectangle bulgeRect = new Rectangle(currentRect.X - HOVER_BULGE, currentRect.Y - HOVER_BULGE, currentRect.Width + HOVER_BULGE * 2, currentRect.Height + HOVER_BULGE * 2);
                        card.card.Draw(spriteBatch, bulgeRect);
                    }
                    else
                    {
                        card.card.Draw(spriteBatch, currentRect);
                        // TODO: highlight glow
                    }
                }
                else
                {
                    card.card.Draw(spriteBatch, currentRect);
                }
            }
        }

        Player viewingPlayer;
        List<UIButton> buttons;
        UIButtonStyleSet basicButtonStyle;
        Texture2D symbolsTexture;

        readonly Vector2 handCardSize = new Vector2(75, 100);
        readonly Vector2 battlefieldCardSize = new Vector2(60, 80);
        readonly Vector2 battlefieldSpacing = new Vector2(5, 10);
        int showingGameState = 0;

        Dictionary<CardReference.CardID, UICard> gameStateRepresentation = new Dictionary<CardReference.CardID, UICard>();
        UICard hoveredCard;

        public MagicUI(Player viewingPlayer, ContentManager content, GraphicsDevice device)
        {
            this.viewingPlayer = viewingPlayer;

            SpriteFont font = content.Load<SpriteFont>("Font");

            Texture2D normalButtonTexture = Texture2D.FromStream(device, File.OpenRead("Content/button3d.png"));
            Texture2D hoverButtonTexture = Texture2D.FromStream(device, File.OpenRead("Content/button3d_hover.png"));
            Texture2D pressButtonTexture = Texture2D.FromStream(device, File.OpenRead("Content/button3d_pressed.png"));

            symbolsTexture = Texture2D.FromStream(device, File.OpenRead("Content/mtgsymbols.png"));

            basicButtonStyle = new UIButtonStyleSet(
                new UIButtonStyle(font, Color.Black, normalButtonTexture, Color.White),
                new UIButtonStyle(font, Color.Yellow, hoverButtonTexture, Color.White),
                new UIButtonStyle(font, Color.Yellow, pressButtonTexture, Color.White, new Vector2(0,1))
            );

            buttons = new List<UIButton>() { new UIButton("Continue", new Rectangle(10, 10, 100, 50), basicButtonStyle, OnPressContinue) };
        }

        public void OnPressContinue()
        {
            viewingPlayer.ContinueToNextPhase();
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
                if (hoveredCard.card is HandCardReference)
                    viewingPlayer.Play((HandCardReference)hoveredCard.card);
            }

            foreach(UIButton button in buttons)
            {
                button.Update(inputState);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (UICard c in gameStateRepresentation.Values)
            {
                c.Draw(spriteBatch);
            }

            foreach (UIButton button in buttons)
            {
                button.Draw(spriteBatch);
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

        public static void Draw9Grid(SpriteBatch spriteBatch, Texture2D texture, Rectangle rect, Color color)
        {
            Point cornerSize = new Point(Math.Min(rect.Width,texture.Width) / 2, Math.Min(rect.Height, texture.Height) / 2);
            //top left
            spriteBatch.Draw(texture, new Rectangle(rect.X, rect.Y, cornerSize.X, cornerSize.Y), new Rectangle(0,0,cornerSize.X,cornerSize.Y), color);
            //top right
            spriteBatch.Draw(texture, new Rectangle(rect.Right-cornerSize.X, rect.Y, cornerSize.X, cornerSize.Y), new Rectangle(texture.Width-cornerSize.X, 0, cornerSize.X, cornerSize.Y), color);
            //bottom left
            spriteBatch.Draw(texture, new Rectangle(rect.X, rect.Bottom - cornerSize.Y, cornerSize.X, cornerSize.Y), new Rectangle(0, texture.Height - cornerSize.Y, cornerSize.X, cornerSize.Y), color);
            //bottom right
            spriteBatch.Draw(texture, new Rectangle(rect.Right - cornerSize.X, rect.Bottom-cornerSize.Y, cornerSize.X, cornerSize.Y), new Rectangle(texture.Width - cornerSize.X, texture.Height-cornerSize.Y, cornerSize.X, cornerSize.Y), color);
            // top
            spriteBatch.Draw(texture, new Rectangle(rect.X + cornerSize.X, rect.Y, rect.Width - cornerSize.X * 2, cornerSize.Y), new Rectangle(cornerSize.X, 0, 0, cornerSize.Y), color);
            // bottom
            spriteBatch.Draw(texture, new Rectangle(rect.X + cornerSize.X, rect.Bottom-cornerSize.Y, rect.Width - cornerSize.X * 2, cornerSize.Y), new Rectangle(cornerSize.X, texture.Height-cornerSize.Y, 0, cornerSize.Y), color);
            // left
            spriteBatch.Draw(texture, new Rectangle(rect.X, rect.Y+cornerSize.Y, cornerSize.X,rect.Height-cornerSize.Y*2), new Rectangle(0, cornerSize.Y, cornerSize.X, 0), color);
            // right
            spriteBatch.Draw(texture, new Rectangle(rect.Right-cornerSize.X, rect.Y + cornerSize.Y, cornerSize.X, rect.Height - cornerSize.Y * 2), new Rectangle(texture.Width-cornerSize.X, cornerSize.Y, cornerSize.X, 0), color);
            // middle
            spriteBatch.Draw(texture, new Rectangle(rect.X + cornerSize.X, rect.Y + cornerSize.Y, rect.Width - cornerSize.X * 2, rect.Height - cornerSize.Y * 2), new Rectangle(cornerSize.X, cornerSize.Y, 0, 0), color);
        }
    }
}