using App05MonoGame.Helpers;
using App05MonoGame.Managers;
using App05MonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App05MonoGame
{
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

        private Sprite enemySprite;

        private AnimatedSprite playerSprite;
        private AnimatedSprite coinSprite;
        public App05Game()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Setup the game window size
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
        /// use this.Content to load your game content here
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundImage = Content.Load<Texture2D>("backgrounds/green_background720p");
            Texture2D asteroid = Content.Load<Texture2D>("Actors/Stones2Filled_01");
            Texture2D coin = Content.Load<Texture2D>("Actors/coin_copper");

            Animation animation = new Animation("coin", coin, 8);

            coinSprite = new AnimatedSprite();
            coinSprite.Animation = animation;

            coinSprite.Scale = 2.0f;
            //sprite.Rotation = MathHelper.ToRadians(90);
            coinSprite.Speed = 0;

            SetupPlayer();
            //SetupEnemy();
        }

        private void SetupPlayer()
        {
            Texture2D sheet4x3 = Content.Load<Texture2D>("Actors/rsc-sprite-sheet1");
            playerSprite = CreateAnimatedSprite(sheet4x3);

            playerSprite.Position = new Vector2(200, 200);
            playerSprite.Speed = 50;
            playerSprite.Direction = new Vector2(1, 0);
            playerSprite.Scale = 4.0f;

            playerSprite.Rotation = MathHelper.ToRadians(0);
            playerSprite.RotationSpeed = 0f;
        }

        private AnimatedSprite CreateAnimatedSprite(Texture2D sheet)
        {
            AnimationManager manager = new AnimationManager(graphicsDevice, sheet, 4, 3);
            
            manager.CreateAnimation("Down", 1);
            manager.CreateAnimation("Left", 2);
            manager.CreateAnimation("Right", 3);
            manager.CreateAnimation("Up", 4);

            AnimatedSprite animatedSprite = new AnimatedSprite();
            
            animatedSprite.Animations = manager.Animations;
            animatedSprite.PlayAnimation("Right");
            animatedSprite.Image = manager.FirstFrame;

            return animatedSprite;
        }

        private void SetupEnemy()
        {
            //Texture2D sheet = Content.Load<Texture2D>("Actors/rsc-sprite-sheet3");
            //enemySprite = LoadSpriteSheet(sheet);
            //enemySprite.Position = new Vector2(500, 100);
            //enemySprite.Direction = new Vector2(-1, 0);
            ////enemySprite.Rotation = MathHelper.ToRadians(-45);
            //enemySprite.Speed = 100;
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

            // TODO: Add your update logic here

            playerSprite.Update(gameTime);
            //coinSprite.Update(gameTime);

            //enemySprite.Update(gameTime);

            //if (playerSprite.HasCollided(coinSprite))
            //{
            //    coinSprite.IsActive = false;
            //    coinSprite.IsAlive = false;
            //    coinSprite.IsVisible = false;
            //}

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
            
            playerSprite.Draw(spriteBatch);
            //coinSprite.Draw(spriteBatch);

            //enemySprite.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
