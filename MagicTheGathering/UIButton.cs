using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class UIButtonStyleSet
    {
        public readonly UIButtonStyle normal;
        public readonly UIButtonStyle hover;
        public readonly UIButtonStyle pressed;

        public UIButtonStyleSet(UIButtonStyle normal, UIButtonStyle hover, UIButtonStyle pressed)
        {
            this.normal = normal;
            this.hover = hover;
            this.pressed = pressed;
        }
    }

    class UIButtonStyle
    {
        public readonly SpriteFont font;
        public readonly Color textColor;
        public readonly Texture2D texture;
        public readonly Color fillColor;

        public UIButtonStyle(SpriteFont font, Color textColor, Texture2D texture, Color fillColor)
        {
            this.font = font;
            this.textColor = textColor;
            this.texture = texture;
            this.fillColor = fillColor;
        }

        public void Draw(SpriteBatch spriteBatch, string label, Rectangle frame)
        {
            spriteBatch.Draw(texture, frame, fillColor);
            if (font != null)
            {
                Vector2 labelSize = font.MeasureString(label);
                spriteBatch.DrawString(font, label, new Vector2(frame.Center.X - labelSize.X / 2, frame.Center.Y - labelSize.Y / 2), textColor);
            }
        }
    }

    class UIButton
    {
        public readonly string label;
        public readonly Rectangle frame;
        public readonly UIButtonStyleSet styles;
        public readonly OnPressDelegate onPress;
        public delegate void OnPressDelegate();
        bool mouseInside;
        bool pressedInside;

        public UIButton(string label, Rectangle frame, UIButtonStyleSet styles, OnPressDelegate onPress)
        {
            this.label = label;
            this.frame = frame;
            this.styles = styles;
            this.onPress = onPress;
        }

        public void Update(InputState inputState)
        {
            mouseInside = frame.Contains(inputState.MousePos);
            if(mouseInside && inputState.WasMouseLeftJustPressed())
            {
                pressedInside = true;
            }

            if (!inputState.mouseLeft.pressed)
            {
                if(mouseInside && pressedInside)
                {
                    onPress();
                }
                pressedInside = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            UIButtonStyle currentStyle;
            if(mouseInside)
            {
                if (pressedInside)
                    currentStyle = styles.pressed;
                else
                    currentStyle = styles.hover;
            }
            else
            {
                currentStyle = styles.normal;
            }

            currentStyle.Draw(spriteBatch, label, frame);
        }
    }
}
