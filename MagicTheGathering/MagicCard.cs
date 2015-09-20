using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MagicTheGathering
{
    internal class MagicCard
    {
        private readonly string name;
        private readonly Texture2D texture;
        private readonly MagicCardType type;

        public MagicCard (string name, GraphicsDevice GraphicsDevice, MagicCardType type)
        {
            this.type = type;
            this.name = name;
            texture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Cards/" + name + ".jpg"));
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