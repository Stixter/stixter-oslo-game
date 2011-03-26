using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.ScreenManager.Menu;

namespace Stixter.Plexi.ScreenManager.GameScreens
{
    public class StartScreen : GameScreen
    {
        private readonly MenuComponent _menuComponent;
        private readonly Texture2D _image;
        private readonly Rectangle _imageRectangle;

        public int SelectedIndex
        {
            get { return _menuComponent.SelectedIndex; }
            set { _menuComponent.SelectedIndex = value; }
        }

        public StartScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D image)
            : base(game, spriteBatch)
        {
            var menuItems = new List<MenuItems.MenuItem>
                                {
                                    MenuItems.MenuItem.StartGame, 
                                    MenuItems.MenuItem.Help, 
                                    MenuItems.MenuItem.HighScore, 
                                    MenuItems.MenuItem.EndGame
                                }; 

            _menuComponent = new MenuComponent(game, spriteBatch, spriteFont, menuItems);

            Components.Add(_menuComponent);

            _image = image;
            _imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
          
            SpriteBatch.Draw(_image, _imageRectangle, Color.White);
        }
    }
}
