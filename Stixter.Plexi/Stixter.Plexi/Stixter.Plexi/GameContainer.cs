using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Stixter.Plexi
{
    public interface IGameContainer
    {
        ContentManager GetContentManager();
    }

    public class GameContainer : IGameContainer
    {
        private readonly ContentManager _contentManager;

        public GameContainer(Game game)
        {
            _contentManager = game.Content;
        }

        public ContentManager GetContentManager()
        {
            return _contentManager;
        }
    }
}