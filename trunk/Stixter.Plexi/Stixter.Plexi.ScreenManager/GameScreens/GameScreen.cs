using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Stixter.Plexi.ScreenManager.GameScreens
{
    public abstract class GameScreen : DrawableGameComponent
    {
        readonly List<GameComponent> _components = new List<GameComponent>();
        protected Game CurrentGame;
        protected SpriteBatch SpriteBatch;
        protected ContentManager ContentManager;

        public List<GameComponent> Components
        {
            get { return _components; }
        }

        protected GameScreen(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            CurrentGame = game;
            SpriteBatch = spriteBatch;
            ContentManager = game.Content;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (var component in _components)
                if (component.Enabled == true)
                    component.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (var component in _components)
                if (component is DrawableGameComponent && ((DrawableGameComponent)component).Visible)
                    ((DrawableGameComponent)component).Draw(gameTime);
        }

        public virtual void Show()
        {
            Visible = true;
            Enabled = true;
            foreach (var component in _components)
            {
                component.Enabled = true;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = true;
            }
        }

        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;
            foreach (var component in _components)
            {
                component.Enabled = false;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = false;
            }
        }
    }
}
