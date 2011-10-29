using Microsoft.Xna.Framework.Graphics;

namespace Stixter.Plexi.Sprites.Helpers
{
    public static class SpritePosition
    {
        public static void KeepSpriteOnScreen(AnimatedSprite gs, GraphicsDevice graphicsDevice)
        {
            //if (gs.Position.Y <= graphicsDevice.Viewport.Y)
            //{
            //    gs.Position.Y = graphicsDevice.Viewport.Y;
            //}

            if (gs.Position.X <= graphicsDevice.Viewport.X)
            {
                gs.Position.X = graphicsDevice.Viewport.X;
            }

            if (gs.Position.Y >= graphicsDevice.Viewport.Height-65)
            {
                gs.Position.Y = graphicsDevice.Viewport.Height -65;
            }

            if (gs.Position.X >= graphicsDevice.Viewport.Width - 35)
            {
                gs.Position.X = graphicsDevice.Viewport.Width - 35;
            }
        }
    }
}
