using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.ScreenManager.GameScreens;


namespace Stixter.Plexi.ScreenManager.Handlers
{
    public interface ICollectItemHandler
    {
        void Init(Game game);
        void CreatePickUpItems();
        void SetPickUpItemsOnPlatforms(List<Platform> platforms);
        void CheckIfPlayerPickUpItemAndCreateNewItems(Rectangle targetRectangel, List<Platform> platforms);
        void DrawAllItems(SpriteBatch batch);
    }
}
