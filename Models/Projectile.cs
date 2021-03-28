using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace App05MonoGame.Models
{
    public class Projectile : Sprite
    {        
        public float LifeSpan = 0f;
        private float timer;

        public Projectile() : base()
        {            
        }

        /// <summary>
        /// Constructor sets the main image and starting position of
        /// the Sprite as a Vector2
        /// </summary>
        public Projectile(Texture2D image, int x, int y) : this()
        {
            Image = image;
            Position = new Vector2(x, y);
            Origin = new Vector2(Image.Width / 2, Image.Height / 2);            
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer > LifeSpan)
            {
                Speed = 0f;

                IsActive = false;
                IsAlive = false;
                IsVisible = false;
            }
            Rotation += MathHelper.ToRadians(RotationSpeed);
            Position += Direction * 10;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
             spriteBatch.Draw(Image, Position, new Rectangle(0,0, Image.Width, Image.Height), Color.White,
                Rotation, Origin, 1, SpriteEffects.None, 0);
        }
    }
}
