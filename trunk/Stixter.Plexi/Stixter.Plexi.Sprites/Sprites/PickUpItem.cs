using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class PickUpItem : GameComponent
    {
        private readonly ContentManager _contentManager;
        public GameSprite Sprite;
        private Texture2D _texture;

        public PickUpItem(Game game) : base(game)
        {
            _contentManager = game.Content;
            CreateObject();
        }

        public Rectangle GetFloorRec()
        {
            return new Rectangle(
                (int)Sprite.Position.X,
                (int)Sprite.Position.Y,
                Sprite.Sprite.Width,
                Sprite.Sprite.Height);
        }

        private void CreateObject()
        {
            _texture = _contentManager.Load<Texture2D>("Sprites\\pickupitem");
            Sprite = new GameSprite(_texture) { Alive = true };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Sprite.Alive)
                spriteBatch.Draw(Sprite.Sprite, Sprite.Position, null, Color.White, Sprite.Rotation, Sprite.Center, 1.0f, SpriteEffects.None, 0);
        }
    }
}