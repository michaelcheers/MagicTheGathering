using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace MagicTheGathering
{
    internal class MagicCard
    {
        private readonly string name;
        private readonly Texture2D texture;
        private readonly MagicCardType type;
        public readonly Ability[] abilities;
        public readonly string[] subtypes;
        internal readonly int power;
        internal readonly int toughness;
        public readonly Cost cost;
        
        public MagicCard (string name, GraphicsDevice GraphicsDevice, MagicCardType type, string[] subtypes, Ability[] abilities, Cost cost, int power, int toughness)
        {
            this.cost = cost;
            this.type = type;
            this.name = name;
            texture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Cards/" + name + ".jpg"));
            this.subtypes = subtypes;
            this.abilities = abilities;
            this.power = power;
            this.toughness = toughness;
        }

        public bool IsSubtype (string subtype)
        {
            return new List<string>(subtypes).Contains(subtype);
        }

        internal MagicCardType Type
        {
            get
            {
                return type;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle rect, float rotation)
        {
            Point origin = new Point(texture.Width /2, texture.Height /2);
            spriteBatch.Draw(texture, new Rectangle(rect.X+rect.Width/2, rect.Y+rect.Height/2, rect.Width, rect.Height), null, Color.White, rotation, new Vector2(origin.X, origin.Y), SpriteEffects.None, 0);
        }
    }
}