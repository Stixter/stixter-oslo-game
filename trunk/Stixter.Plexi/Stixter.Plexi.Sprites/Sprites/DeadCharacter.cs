using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class DeadCharacter : GameComponent
    {
        private readonly ContentManager _contentManager;
        public GameSprite Sprite;
        private Texture2D _texture;

        public DeadCharacter(Game game)
            : base(game)
        {
            _contentManager = game.Content;
            CreateObject();
        }

        public Rectangle GetFloorRec()
        {
            return new Rectangle(
                (int)Sprite.Position.X,
                (int)Sprite.Position.Y - 20,
                Sprite.Sprite.Width,
                Sprite.Sprite.Height - 40);
        }

        private void CreateObject()
        {
            _texture = _contentManager.Load<Texture2D>("Sprites\\player_angel");
            Sprite = new GameSprite(_texture) { Alive = true };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.Sprite, Sprite.Position, null, Color.White, Sprite.Rotation, Sprite.Center, 1.0f, SpriteEffects.None, 0);
        }
    }
}