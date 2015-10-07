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
        public readonly Cost cost;
        
        public MagicCard (string name, GraphicsDevice GraphicsDevice, MagicCardType type, string[] subtypes, Ability[] abilities, Cost cost)
        {
            this.cost = cost;
            this.type = type;
            this.name = name;
            texture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Cards/" + name + ".jpg"));
            this.subtypes = subtypes;
            this.abilities = abilities;
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

        public void Draw(SpriteBatch spriteBatch, Rectangle rect)
        {
            spriteBatch.Draw(texture, rect, Color.White);
        }
    }
}