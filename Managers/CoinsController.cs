using App05MonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace App05MonoGame.Managers
{
    public enum CoinColours
    {
        copper = 100,
        Silver = 200,
        Gold = 500
    }

    /// <summary>
    /// This class creates a list of coins which
    /// can be updated and drawn and checked for
    /// collisions with the player sprite
    /// </summary>
    /// <authors>
    /// Derek Peacock & Andrei Cruceru
    /// </authors>
    public class CoinsController
    {
        public List<AnimatedSprite> Coins = new List<AnimatedSprite>();

        /// <summary>
        /// Create an animated sprite of a copper coin
        /// which could be collected by the player for a score
        /// </summary>
        public void CreateCoin(GraphicsDevice graphics, Texture2D coinSheet)
        {
            Animation animation = new Animation("coin", coinSheet, 8);

            AnimatedSprite coin = new AnimatedSprite();
            coin.Animation = animation;
            coin.Image = animation.SetMainFrame(graphics);

            coin.Scale = 2.0f;
            coin.Position = new Vector2(600, 100);
            coin.Speed = 0;

            Coins.Add(coin);
        }

        public void HasCollided(AnimatedPlayer player)
        {
            foreach (AnimatedSprite coin in Coins)
            {
                if (coin.HasCollided(player))
                {
                    coin.IsActive = false;
                    coin.IsAlive = false;
                    coin.IsVisible = false;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach(AnimatedSprite coin in Coins)
            {
                coin.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AnimatedSprite coin in Coins)
            {
                coin.Draw(spriteBatch);
            }
        }
    }
}
