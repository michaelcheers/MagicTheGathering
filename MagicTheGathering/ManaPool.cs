using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    public class ManaPool
    {
        internal ManaAmount amount;

        public void Empty ()
        {
            amount.Clear();
        }

        public ManaPool()
        {
        }

        internal void Add (ManaAmount newAmount)
        {
            amount.Add(newAmount);
        }

        public int Get(MTGColor c)
        {
            return amount.GetAmount(c);
        }

        static MTGColor[] ColorsList = { MTGColor.White, MTGColor.Blue, MTGColor.Black, MTGColor.Red, MTGColor.Green, MTGColor.Colorless };

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position)
        {
            Point currentPos = new Point((int)position.X, (int)position.Y);
            foreach(MTGColor c in ColorsList)
            {
                int amount = Get(c);
                if (amount > 0)
                {
                    MagicTheGatheringUI.MagicUI.DrawColorSymbol(spriteBatch, c, new Rectangle(currentPos.X, currentPos.Y, 20, 20));
                    string text = "x"+amount;
                    spriteBatch.DrawString(font, text, new Vector2(currentPos.X+22, currentPos.Y+2), Color.White);

                    currentPos.X += 27 + (int)font.MeasureString(text).X;
                }
            }
        }
    }
}