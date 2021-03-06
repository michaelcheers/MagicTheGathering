﻿using MagicTheGathering;
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
            bool fakeTapped = false;

            public bool Active(int state) => currentState == state;

            public UICard(CardReference card, int showingGameState)
            {
                this.card = card;
                currentState = showingGameState;
            }

            public void SetGameState(int newState) => currentState = newState;

            public void SetDesiredPos(Rectangle newTargetRect, bool showTapped)
            {
                float newTargetRotation = showTapped? (float)(Math.PI / 2): 0;

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

        static Texture2D cardBack;
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

            cardBack = Texture2D.FromStream(device, File.OpenRead("Content/card-back.jpg"));
            symbolsTexture = Texture2D.FromStream(device, File.OpenRead("Content/mtgsymbols.png"));

            basicButtonStyle = new UIButtonStyleSet(
                new UIButtonStyle(font, Color.Black, normalButtonTexture, Color.White),
                new UIButtonStyle(font, Color.Yellow, hoverButtonTexture, Color.White),
                new UIButtonStyle(font, Color.Yellow, pressButtonTexture, Color.White, new Vector2(0,1))
            );

            buttons = new List<UIButton>() { new UIButton("Continue", new Rectangle(690, 420, 100, 50), basicButtonStyle, OnPressContinue) };
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

        Player GetOpponent()
        {
            foreach (Player opponent in viewingPlayer.game.players)
            {
                if (opponent != viewingPlayer)
                {
                    return opponent;
                }
            }
            return null;
        }

        public void Update(Input.InputState inputState, Rectangle screenSize)
        {
            // for now, let's refresh the state every frame
            NewGameState();

            LayOutArea(viewingPlayer.Hand, new Rectangle(100, screenSize.Height - 100, screenSize.Width - 200, 100), handCardSize, 0);
            int battlefieldHeight = (screenSize.Height - 130) / 2;
            LayOutBattlefield(new Rectangle(10, battlefieldHeight, screenSize.Width-20, battlefieldHeight), viewingPlayer.Battlefield, false);

            Player opponent = GetOpponent();
            if(opponent != null)
            {
                LayOutBattlefield(new Rectangle(10, 0, screenSize.Width-20, battlefieldHeight), opponent.Battlefield, true);
            }

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
                    bool activatedAnAbility = false;
                    foreach(AbilityInstance a in hoveredCard.card.Abilities)
                    {
                        if(a.ability is ActivatedAbility)
                        {
                            ((ActivatedAbility)a.ability).Activate((BattlefieldCardReference)hoveredCard.card);
                            activatedAnAbility = true;
                            break;
                        }
                    }

                    if(!activatedAnAbility && hoveredCard.card.IsCreature && viewingPlayer.game.currentPhase == Phase.Attack)
                    {
                        BattlefieldCardReference battlefieldCard = (BattlefieldCardReference)hoveredCard.card;
                        if (viewingPlayer.IsAttacking(battlefieldCard))
                        {
                            viewingPlayer.WithdrawAttacker(battlefieldCard);
                        }
                        else
                        {
                            viewingPlayer.DeclareAttacker(battlefieldCard, GetOpponent());
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

            string phaseText = "";
            switch(viewingPlayer.game.currentPhase)
            {
                case Phase.Main: phaseText = "Precombat Main"; break;
                case Phase.Attack: phaseText = "Combat"; break;
                case Phase.Main2: phaseText = "Postcombat Main"; break;
            }
            spriteBatch.DrawString(font, phaseText, new Vector2(100, 0), Color.White);

            viewingPlayer.manaPool.Draw(spriteBatch, font, new Vector2(200,20));
            viewingPlayer.DrawInfo(spriteBatch, font, new Vector2(20,20));

            Player opponent = GetOpponent();
            if(opponent != null)
            {
                opponent.DrawInfo(spriteBatch, font, new Vector2(640, 20));
            }
        }

        void LayOutCardFan(IEnumerable<CardReference> cards, Vector2 start, Vector2 cardSize, float cardOffsetX)
        {
            int cardIdx = 0;
            foreach (CardReference c in cards)
            {
                Rectangle rect = new Rectangle((int)(start.X + (cardOffsetX * cardIdx)), (int)start.Y, (int)cardSize.X, (int)cardSize.Y);
                bool showTapped = false;

                if (c is BattlefieldCardReference)
                {
                    BattlefieldCardReference permanent = (BattlefieldCardReference)c;
                    showTapped = permanent.isTapped;

                    if(!showTapped && viewingPlayer.game.currentPhase == Phase.Attack)
                    {
                        if(c.IsCreature && viewingPlayer.IsAttacking(permanent)) //FIXME: vigilance
                        {
                            showTapped = true;
                        }
                    }
                }

                gameStateRepresentation[c.cardID].SetDesiredPos(new Rectangle((int)(start.X + (cardOffsetX * cardIdx)), (int)start.Y, (int)cardSize.X, (int)cardSize.Y), showTapped);
                cardIdx++;
            }
        }

        void LayOutArea(IEnumerable<CardReference> cards, Rectangle bounds, Vector2 cardSize, float cardSpacing)
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
            LayOutCardFan(cards, layoutStartPos, cardSize, actualSpacing);
        }

        void LayOutDualArea(IEnumerable<CardReference> cardsLeft, IEnumerable<CardReference> cardsRight, Rectangle bounds, Vector2 cardSize, float cardSpacing)
        {
            float actualSpacing = cardSize.X + cardSpacing;
            int numCardsLeft = cardsLeft.Count();
            int numCardsRight = cardsRight.Count();
            int totalCards = numCardsLeft + numCardsRight;
            int areaSpacingX = 10;
            if (totalCards > 2)
            {
                float overlapSpacing = (bounds.Width - areaSpacingX - cardSize.X*2) / (totalCards - 2);
                if (actualSpacing > overlapSpacing)
                    actualSpacing = overlapSpacing;
            }
            float cardsWidthLeft = cardSize.X + actualSpacing * (numCardsLeft - 1);
            float cardsWidthRight = cardSize.X + actualSpacing * (numCardsRight - 1);

            float leftAreaWidth = bounds.Width - cardsWidthRight;

            Vector2 leftLayoutStartPos = new Vector2(bounds.Left + (leftAreaWidth - cardsWidthLeft) * 0.5f, bounds.Top);
            LayOutCardFan(cardsLeft, leftLayoutStartPos, cardSize, actualSpacing);

            Vector2 rightLayoutStartPos = new Vector2(bounds.Right - cardsWidthRight, bounds.Top);
            LayOutCardFan(cardsRight, rightLayoutStartPos, cardSize, actualSpacing);
        }

        void LayOutBattlefield(Rectangle bounds, IEnumerable<CardReference> permanents, bool isOpponent)
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

            int landRowY = isOpponent ? bounds.Top : bounds.Bottom - (int)battlefieldCardSize.Y;
            int mainRowY = isOpponent ? bounds.Top + (int)(battlefieldSpacing.Y + battlefieldCardSize.Y * 2) : bounds.Bottom - (int)(battlefieldSpacing.Y + battlefieldCardSize.Y * 2);

            if (miscPermanents.Count > 0)
            {
                if (lands.Count > creatures.Count)
                {
                    // misc permanents on the creatures row
                    LayOutDualArea(creatures, miscPermanents, new Rectangle(bounds.Left, mainRowY, bounds.Width, (int)battlefieldCardSize.Y), battlefieldCardSize, battlefieldSpacing.X);
                    LayOutArea(lands, new Rectangle(bounds.Left, landRowY, bounds.Width, (int)battlefieldCardSize.Y), battlefieldCardSize, battlefieldSpacing.X);
                }
                else
                {
                    // misc permanents on the lands row
                    LayOutArea(creatures, new Rectangle(bounds.Left, mainRowY, bounds.Width, (int)battlefieldCardSize.Y), battlefieldCardSize, battlefieldSpacing.X);
                    LayOutDualArea(lands, miscPermanents, new Rectangle(bounds.Left, landRowY, bounds.Width, (int)battlefieldCardSize.Y), battlefieldCardSize, battlefieldSpacing.X);
                }
            }
            else
            {
                LayOutArea(creatures, new Rectangle(bounds.Left, mainRowY, bounds.Width, (int)battlefieldCardSize.Y), battlefieldCardSize, battlefieldSpacing.X);
                LayOutArea(lands, new Rectangle(bounds.Left, landRowY, bounds.Width, (int)battlefieldCardSize.Y), battlefieldCardSize, battlefieldSpacing.X);
            }
        }

        public static void DrawCardBack(SpriteBatch spriteBatch, Rectangle rect)
        {
            spriteBatch.Draw(cardBack, rect, Color.White);
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