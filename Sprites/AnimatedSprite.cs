using App05MonoGame.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace App05MonoGame.Sprites
{
    public class AnimatedSprite : Sprite
    {

        public Animation Animation { get; set; }

        private Rectangle sourceRectangle;

        public AnimatedSprite() : base()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if(Animation != null)
            {
                sourceRectangle = Animation.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Animation != null && sourceRectangle != Rectangle.Empty)
            {
                spriteBatch.Draw
                        (Animation.FrameSet,
                         Position,
                         sourceRectangle,
                         Color.White, Rotation, Origin,
                         Scale, SpriteEffects.None, 1);
            }
            else
                base.Draw(spriteBatch);
        }
    }
}
