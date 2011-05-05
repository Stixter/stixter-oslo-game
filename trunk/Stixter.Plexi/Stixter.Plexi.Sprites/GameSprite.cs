using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stixter.Plexi.Sprites
{
    public class GameSprite
    {
        public Texture2D Sprite;
        public Vector2 Position;
        public float Rotation;
        public Vector2 Center;
        public Vector2 Velocity;
        public bool Alive;
        public bool Hidden;

        public GameSprite(Texture2D loadTexture)
        {
            Hidden = false;
            Rotation = 0.0f;
            Position = Vector2.Zero;
            Sprite = loadTexture;
            Center = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            Velocity = Vector2.Zero;
            Alive = false;
        }
    }
}
