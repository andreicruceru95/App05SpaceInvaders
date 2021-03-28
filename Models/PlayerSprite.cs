using App05MonoGame.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace App05MonoGame.Models
{
    public enum DirectionControl
    {
        Rotational,
        FourDirections
    }

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
        private readonly MovementController movement;
        SoundEffect laser;
        KeyboardState previousKey;
        KeyboardState currentKey;
        public int Ammo { get; set; } = 1000;
        public Projectile Projectile { get; set; }

        public DirectionControl DirectionControl { get; set; }

        public PlayerSprite(): base()
        {
            DirectionControl = DirectionControl.Rotational;
            movement = new MovementController();

            laser = SoundController.GetSoundEffect("Laser");
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

        public void SetControl(DirectionControl control)
        {
            this.DirectionControl = control;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            previousKey = currentKey;
            currentKey = Keyboard.GetState();

            IsActive = false;
            RotationSpeed = 0;

            if(DirectionControl == DirectionControl.FourDirections)
            {
                Vector2 newDirection = movement.ChangeDirection(currentKey);

                if (newDirection != Vector2.Zero)
                {
                    Direction = newDirection;
                    IsActive = true;
                }
            }
            else if(DirectionControl == DirectionControl.Rotational)
            {
                Rotate(currentKey);
            }
            Shoot(sprites, currentKey);

            base.Update(gameTime, sprites);
        }

        private void Rotate(KeyboardState keyState)
        {

            if (keyState.IsKeyDown(movement.InputKeys.TurnRight))
            {
                if (RotationSpeed == 0) RotationSpeed = 1.0f;
                Rotation += MathHelper.ToRadians(RotationSpeed);                
            }
            else if (keyState.IsKeyDown(movement.InputKeys.TurnLeft))
            {
                if (RotationSpeed == 0) RotationSpeed = 1.0f;
                Rotation -= MathHelper.ToRadians(RotationSpeed);                
            }

            if (keyState.IsKeyDown(movement.InputKeys.Forward))
            {
                IsActive = true;
            }
            Direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
        }

        /// <summary>
        /// Shoot projectiles when user presses space key.
        /// </summary>
        /// <param name="sprites"></param>
        private void Shoot(List<Sprite> sprites, KeyboardState keyState)
        {
            // this makes sure that each press of space key will shoot only once.
            if (previousKey.IsKeyUp(movement.InputKeys.Shoot) && 
                keyState.IsKeyDown(movement.InputKeys.Shoot))
            {
                Ammo--;

                if (ValidateAmmo())
                {
                    AddProjectile(sprites);
                    laser.Play(0.5f, 0.5f, 0);
                }                
            }
        }

        private bool ValidateAmmo()
        {
            if (Ammo <= 0)
            {
                Ammo = 0;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Add projectile to list.
        /// Each projectile will have the same direction and origin as the shooting parent.
        /// Each projectile will have twice the speed of it's parent.
        /// </summary>
        /// <param name="sprites">List of sprites</param>
        private void AddProjectile(List<Sprite> sprites)
        {
            var projectile = Projectile.Clone() as Projectile;
            projectile.Direction = this.Direction;
            projectile.Position = this.Position;
            projectile.Rotation += this.Rotation;
            projectile.Speed = this.Speed;
            projectile.LifeSpan = 1.5f;

            sprites.Add(projectile);
        }
    }
}
