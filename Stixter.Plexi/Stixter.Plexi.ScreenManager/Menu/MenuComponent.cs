using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stixter.Plexi.Sprites;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.Menu
{
    public class MenuComponent : DrawableGameComponent
    {
        private readonly List<MenuItems.MenuItem> _menuItems;
        private int _selectedIndex;
        private int _lastSelectedIndex;
        private Color _notSelectedColor = Color.Yellow;
        private Color _slectedColor = Color.Red;
        private KeyboardState _keyboardState;
        private KeyboardState _oldKeyboardState;
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _spriteFont;
        private Vector2 _position;
        private float _width = 0f;
        private float _height = 0f;
        private ContentManager _contentManager;
        private MenuBackgroundItem _menuBackgroundItem;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                if (_selectedIndex < 0)
                    _selectedIndex = 0;
                if (_selectedIndex >= _menuItems.Count)
                    _selectedIndex = _menuItems.Count - 1;
            }
        }

        public MenuComponent(ContentManager contentManager, Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, List<MenuItems.MenuItem> menuItems)
            : base(game)
        {
            _spriteBatch = spriteBatch;
            _spriteFont = spriteFont;
            _menuItems = menuItems;
            _contentManager = contentManager;
            MeasureMenu();
            _menuBackgroundItem = new MenuBackgroundItem(contentManager);
            _menuBackgroundItem.Sprite.Alive = true;
        }

        private void MeasureMenu()
        {
            _height = 0;
            _width = 0;
            foreach (var item in _menuItems)
            {
                var size = _spriteFont.MeasureString(item.ToString());
                if (size.X > _width)
                    _width = size.X;
                _height += _spriteFont.LineSpacing + 50;
            }

            _position = new Vector2(
                (Game.Window.ClientBounds.Width - _width) / 2, 
                (Game.Window.ClientBounds.Height - _height) / 2);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private bool CheckKey(Keys theKey)
        {
            return _keyboardState.IsKeyUp(theKey) && _oldKeyboardState.IsKeyDown(theKey);
        }

        public override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();

            if (CheckKey(Keys.Down))
            {
                _selectedIndex++;
                if (_selectedIndex == _menuItems.Count)
                    _selectedIndex = 0;
            }

            if (CheckKey(Keys.Up))
            {
                _selectedIndex--;
                if (_selectedIndex < 0)
                    _selectedIndex = _menuItems.Count - 1;
            }

            if(_keyboardState.GetPressedKeys().Contains(Keys.Enter))
            {
                _lastSelectedIndex = SelectedIndex;
                _slectedColor = Color.Red;
            }

            base.Update(gameTime);
            _oldKeyboardState = _keyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            

            base.Draw(gameTime);

            var location = _position;
            var boxLocation = _position;
            boxLocation.X = location.X + 80f;
            boxLocation.Y = location.Y + 15f;
            
            for (var i = 0; i < _menuItems.Count; i++)
            {
                var color = i == _selectedIndex 
                                 ? _slectedColor 
                                 : _notSelectedColor;

                _menuBackgroundItem.Sprite.Position = boxLocation;

                if (i == _selectedIndex)
                    _menuBackgroundItem.SetSelected();
                else
                    _menuBackgroundItem.SetNotSelected();

                _menuBackgroundItem.Draw(_spriteBatch);
                _spriteBatch.DrawString(_spriteFont, MenuItems.GetLabelFromItem(_menuItems[i]), location, color);
                location.Y += _spriteFont.LineSpacing + 30;
                boxLocation.Y += _spriteFont.LineSpacing + 30;
                
            }
        }

       
    }
}
