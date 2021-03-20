using App05MonoGame.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace App05MonoGame.Sprites
{
    public class Sprite: ICloneable
    {
        // Single Image
        public Texture2D Image { get; set; }

        public Vector2 Position { get; set; }

        // A rectangle limiting where the sprite can move
        public Rectangle Boundary { get; set; }

        // Speed = 60 is 1 Pixel/second
        public float Speed { get; set; }

        public float Rotation { get; set; }

        public float RotationSpeed { get; set; }

        public Vector2 Direction { get; set; }

        public float Scale { get; set; }

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
        /// Create a sprite that is active, alive and
        /// visible with no speed, rotation or scale
        /// facing east (to the right)
        /// </summary>
        public Sprite()
        {
            Direction = new Vector2(1, 0);
            Speed = 0;

            IsVisible = true;
            IsAlive = true;
            IsActive = true;

            Scale = 1;
            Rotation = 0;
            RotationSpeed = 0;
        }

        /// <summary>
        /// Constructor sets the main image and starting position of
        /// the Sprite as a Vector2
        /// </summary>
        public Sprite(Texture2D image, int x, int y) : this()
        {
            Image = image;
            Position = new Vector2(x, y);
            

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


            //spriteBatch.Draw(Image, BoundingBox, Color.White);

            spriteBatch.Draw
                (Image,
                 Position,
                 null,
                 Color.White, 0, Vector2.Zero,
                 Scale, SpriteEffects.None, 1);
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
