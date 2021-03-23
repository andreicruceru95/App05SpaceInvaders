using App05MonoGame.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace App05MonoGame.Sprites
{
    /// <summary>
    /// This class contains at least one animation,
    /// although more can be added to the Dictionary
    /// It updates and draws the current animation.
    /// </summary>
    /// <authors>
    /// Derek Peacock & Andrei Cruceru
    /// </authors>
    public class PlayerSprite : Sprite
    {
        public InputKeys InputKeys { get; set; }

        private KeyboardState lastKeyState;
        public PlayerSprite(): base()
        {
            InputKeys = new InputKeys()
            {
                // For directions

                Up = Keys.Up,
                Down = Keys.Down,
                Left = Keys.Left,
                Right = Keys.Right,
                
                // Rotate and Move

                TurnLeft = Keys.A,
                TurnRight = Keys.D,
                Forward = Keys.Space
            };
        }

        /// <summary>
        /// Constructor sets the main image and starting position of
        /// the Sprite as a Vector2
        /// </summary>
        public PlayerSprite(Texture2D image, int x, int y) : this()
        {
            Image = image;
            Position = new Vector2(x, y);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            IsActive = false;
            RotationSpeed = 0;

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

            if(keyState.IsKeyDown(InputKeys.TurnRight))
            {
                if (RotationSpeed == 0) RotationSpeed = 1.0f;
                Rotation += MathHelper.ToRadians(RotationSpeed);
                Direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            }
            else if (keyState.IsKeyDown(InputKeys.TurnLeft))
            {
                if (RotationSpeed == 0) RotationSpeed = 1.0f;
                Rotation -= MathHelper.ToRadians(RotationSpeed);
                Direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            }

            if (keyState.IsKeyDown(InputKeys.Forward))
            {
                IsActive = true;
            }

            base.Update(gameTime);

            lastKeyState = keyState;
        }
    }
}
