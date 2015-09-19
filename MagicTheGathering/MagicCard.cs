using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MagicTheGathering
{
    internal class MagicCard
    {
        private readonly string name;
        private readonly Texture2D texture;

        public MagicCard (string name, GraphicsDevice GraphicsDevice)
        {
            this.name = name;
            texture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Cards/" + name + ".jpg"));
        }
    }
}