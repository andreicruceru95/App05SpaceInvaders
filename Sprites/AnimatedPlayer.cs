using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using App05MonoGame.Models;

namespace App05MonoGame.Sprites
{
    /// <summary>
    /// This class is an AnimatedSprite whose direction
    /// can be controlled by the keyboard in four
    /// directions, up, down, left and right
    /// </summary>
    /// <authors>
    /// Derek Peacock & Andrei Cruceru
    /// </authors>
    public class AnimatedPlayer : AnimatedSprite
    {
        public InputKeys InputKeys { get; set; }

        public bool CanWalk { get; set; }
 
        public AnimatedPlayer() : base()
        {
            CanWalk = false;

            InputKeys = new InputKeys()
            {
                // For directions

                Up = Keys.Up,
                Down = Keys.Down,
                Left = Keys.Left,
                Right = Keys.Right,
            };
        }

        /// <summary>
        /// TODO: Get rid of this duplication
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            IsActive = false;

            if (keyState.IsKeyDown(InputKeys.Right))
            {
                Direction = new Vector2(1, 0);
                IsActive = true;
            }

            if (keyState.IsKeyDown(InputKeys.Left))
            {
                Direction = new Vector2(-1, 0);
                IsActive = true;
            }

            if (keyState.IsKeyDown(InputKeys.Up))
            {
                Direction = new Vector2(0, -1);
                IsActive = true;
            }

            if (keyState.IsKeyDown(InputKeys.Down))
            {
                Direction = new Vector2(0, 1);
                IsActive = true;
            }

            if (CanWalk) Walk();

            base.Update(gameTime);
        }

        private void Walk()
        {
            if (Animations.Count >= 4)
            {
                if (Direction.X > 0 && Direction.Y < Direction.X)
                    Animation = Animations["Right"];

                else if (Direction.Y > 0 && Direction.X < Direction.Y)
                    Animation = Animations["Down"];

                else if (Direction.X < 0 && Direction.X < Direction.Y)
                    Animation = Animations["Left"];

                else if (Direction.Y < 0 && Direction.Y < Direction.X)
                    Animation = Animations["Up"];
            }
        }

    }
}
