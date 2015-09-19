using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class DefaultPlayer : Player
    {
        public DefaultPlayer (SpriteBatch batch, GraphicsDevice device, MagicGame game, Deck deck) : base (game, deck)
        {
            GraphicsDevice = device;
            this.batch = batch;
        }
        SpriteBatch batch;
        GraphicsDevice GraphicsDevice;
        public override void Draw()
        {
            DrawHand(batch, new Rectangle(0, GraphicsDevice.Viewport.Height - 100, GraphicsDevice.Viewport.Width, 100));
        }
    }
}
