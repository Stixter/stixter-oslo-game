using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stixter.Plexi.Core;
using Stixter.Plexi.Sprites.Sprites;

namespace Stixter.Plexi.ScreenManager.GameScreens
{
    public class GameOverScreen : GameScreen
    {
        private string _result;
        private readonly ScreenText _resultText;
        private readonly BlinkText _gameOverText;
        private readonly BlinkText _pressSpaceToContinue;
        private readonly Texture2D _image;
        private readonly Rectangle _imageRectangle;
        private readonly ScreenText _highScoreTableText;
        public bool IshighScore;

        public GameOverScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D image)
            : base(game, spriteBatch)
        {
            IshighScore = false;
            _gameOverText = new BlinkText(spriteFont, 20);
            _gameOverText.SetPosition(555, 140);
            _gameOverText.SetSpacing(3);

            _pressSpaceToContinue = new BlinkText(spriteFont, 320);
            _pressSpaceToContinue.SetPosition(430, 670);

            _highScoreTableText = new ScreenText(game);
            _highScoreTableText.SetPosition(360, 250);

            _resultText = new ScreenText(game);
            _resultText.SetPosition(20, 130);

            _image = image;
            _imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
        }

        public void SetScoreText()
        {
            var rank = "Loser";
            if (ScoreHandler.TotalScore > 5)
                rank = "Chicken";
            if (ScoreHandler.TotalScore > 10)
                rank = "Amature";
            if (ScoreHandler.TotalScore > 15)
                rank = "Pro";
            if (ScoreHandler.TotalScore > 20)
                rank = "Ninja";
            if (ScoreHandler.TotalScore > 25)
                rank = "Zlatan";
            if (ScoreHandler.TotalScore > 30)
                rank = "Chuck Norris";

            _result = "Your score: " + ScoreHandler.TotalScore + "\nYour rank: " + rank;
        }

        public override void Update(GameTime gameTime)
        {
            _gameOverText.Update(gameTime.ElapsedGameTime.Milliseconds);
            _pressSpaceToContinue.Update(gameTime.ElapsedGameTime.Milliseconds);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(_image, _imageRectangle, Color.White);

            //_highScoreTableText.Draw(SpriteBatch, "1. Jonas Persson - Score: 12\n2. Jonas - Score: 12");
            _pressSpaceToContinue.Draw(SpriteBatch, "GO TO MAIN MENU, PRESS \"F1\"");
           
            if (!IshighScore)
            {
                _gameOverText.Draw(SpriteBatch, "GAME OVER");

                if (!string.IsNullOrEmpty(_result))
                {
                    _resultText.Draw(SpriteBatch, _result);
                }
            }

            base.Draw(gameTime);
        }
    }
}