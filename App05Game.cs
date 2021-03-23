using App05MonoGame.Helpers;
using App05MonoGame.Managers;
using App05MonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace App05MonoGame
{
    /// <summary>
    /// This game creates a variety of sprites as an
    /// example.  There is no game to play yet.
    /// The spaceShip and the asteroid can be used
    /// for a space shooting game, the player, the
    /// coin and the enemy could be used for a pacman
    /// style game where the player moves around collecting
    /// random coins and the enemy tries to catch the
    /// player.
    /// </summary>
    /// <authors>
    /// Derek Peacock & Andrei Cruceru
    /// </authors>
    public class App05Game : Game
    {
        // Constants

        public const int HD_Height = 720;
        public const int HD_Width = 1280;

        // Variables

        private GraphicsDeviceManager graphicsManager;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        private Texture2D backgroundImage;

        private CoinsController coinsController;

        private PlayerSprite shipSprite;
        private Sprite asteroidSprite;

        private AnimatedPlayer playerSprite;
        private AnimatedSprite coinSprite;
        private AnimatedSprite enemySprite;

        public App05Game()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            coinsController = new CoinsController();
        }

        /// <summary>
        /// Setup the game window size to 720P 1280 x 720 pixels
        /// Simple fixed playing area with no camera or scrolling
        /// </summary>
        protected override void Initialize()
        {
            graphicsManager.PreferredBackBufferWidth = HD_Width;
            graphicsManager.PreferredBackBufferHeight = HD_Height;

            graphicsManager.ApplyChanges();

            graphicsDevice = graphicsManager.GraphicsDevice;

            base.Initialize();
        }

        /// <summary>
        /// use Content to load your game images and other content here
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundImage = Content.Load<Texture2D>(
                "backgrounds/green_background720p");

            // suitable for asteroids type game

            SetupSpaceShip();
            SetupAsteroid();

            // animated sprites suitable for pacman type game

            SetupAnimatedPlayer();
            SetupEnemy();

            Texture2D coinSheet = Content.Load<Texture2D>("Actors/coin_copper");
            coinsController.CreateCoin(graphicsDevice, coinSheet);
        }

        /// <summary>
        /// This is a single image sprite that rotates
        /// and move at a constant speed in a fixed direction
        /// </summary>
        private void SetupAsteroid()
        {
            Texture2D asteroid = Content.Load<Texture2D>(
               "Actors/Stones2Filled_01");

            asteroidSprite = new Sprite(asteroid, 1200, 500);
            asteroidSprite.Direction = new Vector2(-1, 0);
            asteroidSprite.Speed = 100;

            asteroidSprite.Rotation = MathHelper.ToRadians(3);
            asteroidSprite.RotationSpeed = 2f;

        }

        /// <summary>
        /// This is a Sprite that can be controlled by a
        /// player using Rotate Left = A, Rotate Right = D, 
        /// Forward = Space
        /// </summary>
        private void SetupSpaceShip()
        {
            Texture2D ship = Content.Load<Texture2D>(
               "Actors/GreenShip");

            shipSprite = new PlayerSprite(ship, 200, 500);
            shipSprite.Direction = new Vector2(1, 0);
            shipSprite.Speed = 200;
            shipSprite.DirectionControl = DirectionControl.Rotational;
        }


        /// <summary>
        /// This is a Sprite with four animations for the four
        /// directions, up, down, left and right
        /// </summary>
        private void SetupAnimatedPlayer()
        {
            Texture2D sheet4x3 = Content.Load<Texture2D>("Actors/rsc-sprite-sheet1");

            AnimationManager manager = new AnimationManager(graphicsDevice, sheet4x3, 4, 3);

            string[] keys = new string[] { "Down", "Left", "Right", "Up" };
            manager.CreateAnimationGroup(keys);

            playerSprite = new AnimatedPlayer();
            manager.AppendAnimationsTo(playerSprite);

            playerSprite.CanWalk = true;
            playerSprite.Scale = 2.0f;

            playerSprite.Position = new Vector2(200, 200);
            playerSprite.Speed = 200;
            playerSprite.Direction = new Vector2(1, 0);

            playerSprite.Rotation = MathHelper.ToRadians(0);
            playerSprite.RotationSpeed = 0f;
        }

        /// <summary>
        /// This is a Sprite with four animations for the four
        /// directions, up, down, left and right
        /// </summary>
        private void SetupEnemy()
        {
            Texture2D sheet4x3 = Content.Load<Texture2D>("Actors/rsc-sprite-sheet3");

            AnimationManager manager = new AnimationManager(graphicsDevice, sheet4x3, 4, 3);

            string[] keys = new string[] { "Down", "Left", "Right", "Up" };

            manager.CreateAnimationGroup(keys);

            enemySprite = new AnimatedSprite();
            manager.AppendAnimationsTo(enemySprite);

            enemySprite.Scale = 2.0f;
            enemySprite.PlayAnimation("Left");

            enemySprite.Position = new Vector2(1000, 200);
            enemySprite.Direction = new Vector2(-1, 0);
            enemySprite.Speed = 50;

            enemySprite.Rotation = MathHelper.ToRadians(0);
        }


        /// <summary>
        /// Called 60 frames/per second and updates the positions
        /// of all the drawable objects
        /// </summary>
        /// <param name="gameTime">
        /// Can work out the elapsed time since last call
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            // Update Asteroids

            shipSprite.Update(gameTime);
            asteroidSprite.Update(gameTime);

            if (shipSprite.HasCollided(asteroidSprite))
            {
                shipSprite.IsActive = false;
                shipSprite.IsAlive = false;
                shipSprite.IsVisible = false;
            }

            // Update Chase Game

            playerSprite.Update(gameTime);
            coinsController.Update(gameTime);
            enemySprite.Update(gameTime);

            coinsController.HasCollided(playerSprite);

            if (playerSprite.HasCollided(enemySprite))
            {
                playerSprite.IsActive = false;
                playerSprite.IsAlive = false;
                playerSprite.IsVisible = false;

                //enemySprite.IsActive = false;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Called 60 frames/per second and Draw all the 
        /// sprites and other drawable images here
        /// </summary>
        /// <param name="gameTime">
        /// Can work out the elapsed time since last call
        /// </param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LawnGreen);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundImage, Vector2.Zero, Color.White);

            // Draw asteroids game

            shipSprite.Draw(spriteBatch);
            asteroidSprite.Draw(spriteBatch);

            // Draw Chase game

            playerSprite.Draw(spriteBatch);
            coinsController.Draw(spriteBatch);
            enemySprite.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
