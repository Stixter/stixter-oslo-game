using Microsoft.Xna.Framework;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class Player : Character
    {
        public Player(Game game, string texture) : base(game, texture)
        {
            MaxCharacterVelocity = 7.0f;
            LastPlayerY = 312f;
            Sprite.Position.X = 400f;
            Sprite.Alive = true;
        }

        public void MoveCharacter(AnimatedSprite.PlayerDirection direction)
        {
            if (Sprite.Alive)
            {
                Sprite.Position.Y = LastPlayerY;

                SetCurrentDirection(direction);
                SetCorretSpeedOnPlayer();
                MovePlayerLeftOrRight();

                if (PlayerState.Equals(State.Jumping) && AllowJump)
                    Jump();

                LastplayerDirection = direction;

                if (!JumpInProgress)
                    Sprite.Position.Y = LastPlayerY + 5.0f;
                else
                    Sprite.Position.Y = LastPlayerY - 5.0f;

                LastPlayerY = Sprite.Position.Y;

                PlayerState = State.Walking;
            }
        }
    }
}