using System;
using System.Collections.Generic;
using Stixter.Plexi.ScreenManager.GameScreens;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.Handlers
{
    public static class ItemPositionHandler
    {
        public static void PlaceItem(int itemCount, List<Platform> platforms, PickUpItem pickUpItem)
        {
            var array = new byte[8];
            var random1 = new Random();
            random1.NextBytes(array);
            var random = array[itemCount] % platforms.Count;
            var valueForItemPlacement = 200;
            if (platforms[random].GetLength() < 5)
            {
                valueForItemPlacement = 100;
            }
            var onPlatform = array[itemCount] % valueForItemPlacement;
            pickUpItem.Sprite.Position.X = platforms[random].GetPostionX() + onPlatform;
            pickUpItem.Sprite.Position.Y = platforms[random].GetPostionY() - 40;
            pickUpItem.Sprite.Alive = true;
        }
    }
}