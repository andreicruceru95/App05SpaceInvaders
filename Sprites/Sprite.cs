using App05MonoGame.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace App05MonoGame.Sprites
{
    public class Sprite: ICloneable
    {
        public Texture2D Image { get; set; }

        public Vector2 Position { get; set; }

        // A rectangle limiting where the sprite can move
        public Rectangle Boundary { get; set; }

        public Vector2 StartPosition { get; set; }

        // Point around which the sprite rotates
        public Vector2 Origin
        {
            get
            {
                if (Image == null)
                    return Vector2.Zero;
                else
                    return new Vector2(Position.X - Width / 2,
                                        Position.Y - Height / 2);
            }
        }

        // Properties
        public int Speed { get; set; }

        public Vector2 Direction { get; set; }

        public Color Color = Color.White;


        public float Rotation = 0f;

        public float RotationSpeed = 0f;

        public float Scale = 1f;

        public SpriteEffects SpriteEffect;

        public SpriteFont TextFont { get; set; }

        public bool IsVisible { get; set; }

        public bool IsAlive { get; set; }

        public bool IsActive { get; set; }

        public int Width
        {
            get { return (int)(Image.Width * Scale); }
        }

        public int Height
        {
            get { return (int)(Image.Height * Scale); }
        }

        // The rectangle occupied by the unscaled image
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle
                (
                    (int)Position.X,
                    (int)Position.Y,
                    Width, Height
                );
            }
        }

        // Variables

        protected float deltaTime;

        protected bool debug = false;

        /// <summary>
        /// Constructor sets the starting position of
        /// the Sprite and current speed of a visible
        /// and alive sprite.
        /// </summary>
        public Sprite(Texture2D image, int x, int y)
        {
            Image = image;
            Position = new Vector2(x, y);
            StartPosition = Position;

            Direction = new Vector2(1, 0);
            Speed = 200;

            IsVisible = true;
            IsAlive = true;
            IsActive = true;

            Scale = 2;
        }


        public void ResetPosition()
        {
            Position = StartPosition;
            Speed = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (IsActive)
            {
                Vector2 newPosition = Position + ((Direction * Speed) * deltaTime);

                if (Boundary.Width == 0 || Boundary.Height == 0)
                {
                    Position = newPosition;
                }
                else if (newPosition.X >= Boundary.X &&
                    newPosition.Y >= Boundary.Y &&
                    newPosition.X + Width < Boundary.X + Boundary.Width &&
                    newPosition.Y + Height < Boundary.Y + Boundary.Height)
                {
                    Position = newPosition;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (debug)
            {
                TextHelper.DrawString(
                    $"({Position.X:0},{Position.Y:0})", Position);
            }

            Rectangle destination = new Rectangle
                ((int)Position.X, (int)Position.Y, Width, Height);


            spriteBatch.Draw(Image, BoundingBox, Color.White);

            //spriteBatch.Draw
            //    (Image,
            //     Position,
            //     null,
            //     Color.White, Rotation, Origin,
            //     Scale, SpriteEffect, 10f);
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
