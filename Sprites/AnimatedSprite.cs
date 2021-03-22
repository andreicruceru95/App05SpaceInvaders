using App05MonoGame.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

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
    public class AnimatedSprite : Sprite
    {

        public Dictionary<string, Animation> Animations { get; set; }

        public Animation Animation { get; set; }



        private Rectangle sourceRectangle;

        public AnimatedSprite() : base()
        {

        }

        public void PlayAnimation(string key)
        {
            if(Animations != null && Animations.ContainsKey(key))
            {
                Animation = Animations[key];
                Animation.Start();
            }
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
