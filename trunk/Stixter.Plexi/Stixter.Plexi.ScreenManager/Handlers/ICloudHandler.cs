using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stixter.Plexi.ScreenManager.Handlers
{
    public interface ICloudHandler
    {
        void Reset();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}