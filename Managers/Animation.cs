using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace App05MonoGame.Managers
{
    public class Animation
    {
        public string Name { get; private set; }
        // One animation: one row of multiple images
        public Texture2D FrameSet { get; set; }
        public int CurrentFrame { get; set; }
        public bool IsPlaying { get; set; }
        public int FramesPerSecond { get; set; }

        private int NumberOfFrames;
        private int frameWidth;
        private int frameHeight;
        private float timer;
        private float maxTime;

        public Animation(string name, Texture2D frameSet, int frames)
        {
            Name = name;
            FrameSet = frameSet;
            NumberOfFrames = frames;
            CurrentFrame = 0;
            IsPlaying = true;

            FramesPerSecond = 10;
            frameHeight = FrameSet.Height;
            frameWidth = FrameSet.Width / NumberOfFrames;
            timer = 0;
            maxTime = 1 / FramesPerSecond;
            
        }

        public Rectangle Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(IsPlaying && timer >= maxTime)
            {
                if (CurrentFrame < NumberOfFrames)
                    CurrentFrame++;

                else
                    CurrentFrame = 0;

                timer = 0;

                return new Rectangle((CurrentFrame - 1) * frameWidth, 0,
                    frameWidth, frameHeight);
            }

            return Rectangle.Empty;
        }        
    }
}
