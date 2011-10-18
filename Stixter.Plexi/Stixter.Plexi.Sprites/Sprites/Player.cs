using Microsoft.Xna.Framework;

namespace Stixter.Plexi.Sprites.Sprites
{
    public class Player : Character
    {
        public Player(Game game, string texture) : base(game, texture)
        {
            MaxCharacterVelocity = 6.8f;
            Reset();
            EnableJumpSound = true;
        }

        public void Reset()
        {
            LastPlayerY = 600f;
            Sprite.Position.X = 100f;
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
                {
                    Sprite.Position.Y = LastPlayerY + FallingVelocity;
                    if(FallingVelocity<7.0)
                        FallingVelocity = FallingVelocity + 0.2f;
                }
                else
                {
                    Sprite.Position.Y = LastPlayerY - JumpingVelocity;
                    if (JumpingVelocity < 1.0)
                        JumpingVelocity = JumpingVelocity - 0.8f;
                }


                LastPlayerY = Sprite.Position.Y;

                PlayerState = State.Walking;
            }
        }
    }
}