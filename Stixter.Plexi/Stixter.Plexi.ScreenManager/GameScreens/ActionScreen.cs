using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.GameScreens
{
    public class ActionScreen : GameScreen
    {
        private KeyboardState _keyboardState;
        private readonly Texture2D _image;
        private readonly Rectangle _imageRectangle;
        private Player _player;
        private KeyboardState _oldKeyboardState;

        public ActionScreen(ContentManager contentManager, Game game, SpriteBatch spriteBatch, Texture2D image)
            : base(contentManager, game, spriteBatch)
        {
            _image = image;
            _imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            _player = new Player(contentManager);
            _player.Sprite.Position.Y = 100f;
            _player.Sprite.Position.X = 200f;
        }

        public override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();

            if (_keyboardState.IsKeyDown(Keys.Right))
            {
                _player.Sprite.Position.X = _player.Sprite.Position.X + 4f;
            }
            if (_keyboardState.IsKeyDown(Keys.Left))
            {
                _player.Sprite.Position.X = _player.Sprite.Position.X - 4f;
            }
            if (_keyboardState.IsKeyDown(Keys.Up))
            {
                _player.Sprite.Position.Y = _player.Sprite.Position.Y - 4f;
            }
            if (_keyboardState.IsKeyDown(Keys.Down))
            {
                _player.Sprite.Position.Y = _player.Sprite.Position.Y + 4f;
            }
            _oldKeyboardState = _keyboardState;
            base.Update(gameTime);
            
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(_image, _imageRectangle, Color.White);
            _player.Draw(SpriteBatch);
            base.Draw(gameTime);
        }

    }
}
