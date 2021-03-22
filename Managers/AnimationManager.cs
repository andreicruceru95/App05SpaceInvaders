using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using App05MonoGame.Helpers;
using System.Collections.Generic;

namespace App05MonoGame.Managers
{
    /// <summary>
    /// This class takes a sprite sheet which may have many
    /// rows and columns and breaks it up into separate
    /// animations.
    /// </summary>
    /// <authors>
    /// Derek Peacock & Andrei Cruceru
    /// </authors>
    public class AnimationManager
    {
        // Original SpriteSheet rows * cols or frames
        public Texture2D SpriteSheet { get; set; }

        private int sheetWidth;

        private int frameHeight;

        private int frameWidth;

        private int frameCount;

        private int animationCount;

        private GraphicsDevice graphicsDevice;

        // A key image for the base sprite
        public Texture2D FirstFrame { get; set; }

        // Each element is a row of image frames
        public Texture2D[] SpriteSheetRow { get; }

        public Dictionary<string, Animation> Animations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AnimationManager(GraphicsDevice graphics,
            Texture2D sheet, int rows, int columns)
        {
            graphicsDevice = graphics;
            SpriteSheet = sheet;

            frameHeight = sheet.Height / rows;
            sheetWidth = SpriteSheet.Width;
            frameWidth = sheetWidth / columns;
            
            frameCount = columns;
            animationCount = rows;

            SpriteSheetRow = new Texture2D[animationCount];
            Animations = new Dictionary<string, Animation>();

            CreateAnimationSheets();
        }

        private void CreateAnimationSheets()
        {
            for (int row = 0; row < animationCount; row++)
            {
                Texture2D Image = SpriteSheet.CreateTexture(
                    graphicsDevice, new Rectangle(0, row * frameHeight,
                                            sheetWidth, frameHeight));
                SpriteSheetRow[row] = Image;
            }

            FirstFrame = SpriteSheetRow[0].CreateTexture(
                graphicsDevice, new Rectangle(0, 0, frameWidth, frameHeight));

        }

        /// <summary>
        /// Create an animation based on the row (starts at 1)
        /// and add it to the dictionary based on its name as
        /// a key.
        /// </summary>
        public void CreateAnimation(string keyName, int row)
        {
            if (row > 0 && row <= animationCount)
            {
                Animation animation = new Animation(
                    keyName, SpriteSheetRow[row - 1], frameCount);
                
                Animations.Add(keyName, animation);
            }
        }

        /// <summary>
        /// From the array of keyNames create an animation
        /// for each of the keynames from the SpriteSheetRows
        /// </summary>
        public void CreateAnimationGroup(string[] keyNames)
        {
            int row = 0;
            foreach(string key in keyNames)
            {
                row++;
                CreateAnimation(key, row);
            }
        }

    }
}
