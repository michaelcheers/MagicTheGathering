using MagicTheGathering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using MTGColor = MagicTheGathering.MTGColor;

namespace MagicTheGatheringUI
{
    class MagicUI
    {
        class UICard
        {
            Rectangle animTargetRect;
            Rectangle animStartRect;
            Rectangle currentRect;
            float animStartRotation;
            float animTargetRotation;
            float currentRotation;
            bool hovered;
            internal CardReference card;
            int currentState;
            const int animDuration = 15;
            int animCurrentFrame;

            public bool Active(int state) => currentState == state;

            public UICard(CardReference card, int showingGameState)
            {
                this.card = card;
                currentState = showingGameState;
            }

            public void SetGameState(int newState) => currentState = newState;

            public void SetDesiredPos(Rectangle newTargetRect)
            {
                float newTargetRotation = 0;
                if (card is BattlefieldCardReference && ((BattlefieldCardReference)card).isTapped)
                {
                    newTargetRotation = (float)(Math.PI / 2);
                }

                if (animTargetRect != newTargetRect || animTargetRotation != newTargetRotation)
                {
                    animCurrentFrame = 0;
                    animTargetRect = newTargetRect;
                    animStartRect = currentRect;
                    animTargetRotation = newTargetRotation;
                    animStartRotation = currentRotation;
                }
            }

            public bool Contains(Vector2 pos) => currentRect.Contains(pos);

            public void Update(bool hovered)
            {
                this.hovered = hovered;

                if (animCurrentFrame < animDuration)
                {
                    animCurrentFrame++;
                    if (animCurrentFrame == animDuration)
                    {
                        currentRect = animTargetRect;
                        currentRotation = animTargetRotation;
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

                        currentRotation = fLerp(animStartRotation, animTargetRotation, fraction);
                    }
                }
            }

            public float fLerp(float a, float b, float fraction) => (float)(a + fraction * (b - a));
            public int Lerp(int a, int b, float fraction) => (int)(a + fraction * (b-a));

            public void Draw(SpriteBatch spriteBatch)
            {
                if (hovered)
                {
                    if (card.Location == CardReference.CardLocation.Hand)
                    {
                        int HOVER_BULGE = animTargetRect.Height / 10;
                        Rectangle bulgeRect = new Rectangle(currentRect.X - HOVER_BULGE, currentRect.Y - HOVER_BULGE, currentRect.Width + HOVER_BULGE * 2, currentRect.Height + HOVER_BULGE * 2);
                        card.card.Draw(spriteBatch, bulgeRect, currentRotation);
                    }
                    else
                    {
                        card.card.Draw(spriteBatch, currentRect, currentRotation);
                        // TODO: highlight glow
                    }
                }
                else
                {
                    card.card.Draw(spriteBatch, currentRect, currentRotation);
                }
            }
        }

        Player viewingPlayer;
        List<UIButton> buttons;
        UIButtonStyleSet basicButtonStyle;
        SpriteFont font;

        static Texture2D symbolsTexture;
        static Dictionary<MTGColor, Rectangle> colorSymbolRects = new Dictionary<MTGColor, Rectangle>()
        {
            { MTGColor.ColorlessMana, new Rectangle(103,208,102,102) },
            { MTGColor.White, new Rectangle(418,208,102,102) },
            { MTGColor.Blue, new Rectangle(523,208,102,102) },
            { MTGColor.Black, new Rectangle(628,208,102,102) },
            { MTGColor.Red, new Rectangle(733,208,102,102) },
            { MTGColor.Green, new Rectangle(838,208,102,102) },
        };
        static Rectangle[] genericSymbolRects =
        {
            new Rectangle(-2,-2,102,102), //0
            new Rectangle(103,-2,102,102),
            new Rectangle(208,-2,102,102),
            new Rectangle(313,-2,102,102),
            new Rectangle(418,-2,102,102),
            new Rectangle(523,-2,102,102),
            new Rectangle(628,-2,102,102),
            new Rectangle(733,-2,102,102),
            new Rectangle(838,-2,102,102),
            new Rectangle(943,-2,102,102),
            new Rectangle(-2,103,102,102), //10
            new Rectangle(103,103,102,102),
            new Rectangle(208,103,102,102),
            new Rectangle(313,103,102,102),
            new Rectangle(418,103,102,102),
            new Rectangle(523,103,102,102),
            new Rectangle(628,103,102,102),
            new Rectangle(733,103,102,102),
            new Rectangle(838,103,102,102),
            new Rectangle(943,103,102,102),
            new Rectangle(-2,208,102,102), //20
        };
        static Rectangle tapSymbolRect = new Rectangle(-2,523, 102,102);

        readonly Vector2 handCardSize = new Vector2(75, 100);
        readonly Vector2 battlefieldCardSize = new Vector2(60, 80);
        readonly Vector2 battlefieldSpacing = new Vector2(5, 10);
        int showingGameState = 0;

        Dictionary<CardReference.CardID, UICard> gameStateRepresentation = new Dictionary<CardReference.CardID, UICard>();
        UICard hoveredCard;

        public MagicUI(Player viewingPlayer, ContentManager content, GraphicsDevice device)
        {
            this.viewingPlayer = viewingPlayer;

            font = content.Load<SpriteFont>("Font");

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

        public void OnPressContinue() => viewingPlayer.ContinueToNextPhase();

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
                {
                    viewingPlayer.Play((HandCardReference)hoveredCard.card);
                }
                else if (hoveredCard.card is BattlefieldCardReference)
                {
                    foreach(AbilityInstance a in hoveredCard.card.Abilities)
                    {
                        if(a.ability is ActivatedAbility)
                        {
                            ((ActivatedAbility)a.ability).Activate((BattlefieldCardReference)hoveredCard.card);
                        }
                    }
//                    BattlefieldCardReference cardRef = ((BattlefieldCardReference)hoveredCard.card);
//                    cardRef.isTapped = !cardRef.isTapped;
                }
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

            viewingPlayer.manaPool.Draw(spriteBatch, font, new Vector2(200,20));
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

            foreach (CardReference c in permanents)
            {
                if(c.IsCreature)
                    creatures.Add(c);
                else if(c.IsLand)
                    lands.Add(c);
                else
                    miscPermanents.Add(c);
            }

            float battlefieldScale = 1.0f;
            float battlefieldNeededHeight = battlefieldCardSize.Y * 2 + battlefieldSpacing.Y;
            if (bounds.Height < battlefieldNeededHeight)
            {
                battlefieldScale = bounds.Height / battlefieldNeededHeight;
            }

            LayOutArea(new Rectangle(bounds.Left, (int)(bounds.Bottom - (battlefieldSpacing.Y+battlefieldCardSize.Y*2)), bounds.Width, (int)battlefieldCardSize.Y), creatures, battlefieldCardSize, battlefieldSpacing.X);
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

        public static void DrawColorSymbol(SpriteBatch spriteBatch, MTGColor color, Rectangle rect)
        {
            spriteBatch.Draw(symbolsTexture, rect, colorSymbolRects[color], Color.White);
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