using App05MonoGame.Helpers;
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

        private Sprite playerSprite;
        private Sprite enemySprite;
        private Sprite sprite;
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
            Texture2D image = Content.Load<Texture2D>("Actors/BigShip1");
            
            sprite = new Sprite(image, 400, 400);
            sprite.Scale = 0.2f;
            sprite.Rotation = MathHelper.ToRadians(90);
            sprite.Speed = 200;

            SetupPlayer();
            //SetupEnemy();
        }

        private void SetupPlayer()
        {
            Texture2D sheet = Content.Load<Texture2D>("Actors/sprite-sheet1");
            playerSprite = LoadSprite(sheet);

            playerSprite.Position = new Vector2(100, 100);
            playerSprite.Speed = 200;

            //playerSprite.Rotation = MathHelper.ToRadians(10);
            playerSprite.RotationSpeed = 0f;
        }

        private void SetupEnemy()
        {
            Texture2D sheet = Content.Load<Texture2D>("Actors/rsc-sprite-sheet3");
            enemySprite = LoadSprite(sheet);
            enemySprite.Position = new Vector2(500, 100);
            enemySprite.Direction = new Vector2(-1, 0);
            //enemySprite.Rotation = MathHelper.ToRadians(-45);
            enemySprite.Speed = 100;
        }

        private Sprite LoadSprite(Texture2D sheet4x3)
        {
            SpriteSheetHelper helper = new SpriteSheetHelper(
                graphicsDevice, sheet4x3, 4, 3);

            PlayerSprite sprite = new PlayerSprite(helper.FirstFrame, 100, 500);
            sprite.Scale = 2.0f;

            return sprite;
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
            sprite.Update(gameTime);

            //enemySprite.Update(gameTime);

            //if(playerSprite.HasCollided(enemySprite))
            //{
            //    playerSprite.IsAlive = false;
            //    enemySprite.IsActive = false;
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
            sprite.Draw(spriteBatch);

            //enemySprite.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
