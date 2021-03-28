using App05MonoGame.Controllers;
using App05MonoGame.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App05MonoGame
{
    public enum GameStates
    {
        Playing, Paused
    }
    /// <summary>
    /// This game creates a variety of sprites as an example.  
    /// There is no game to play yet. The spaceShip and the 
    /// asteroid can be used for a space shooting game, the player, 
    /// the coin and the enemy could be used for a pacman
    /// style game where the player moves around collecting
    /// random coins and the enemy tries to catch the player.
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

        private readonly GraphicsDeviceManager graphicsManager;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        private SpriteFont arialFont;
        private SpriteFont calibriFont;

        private Texture2D backgroundImage;
        private Texture2D cooperCoinSheet;
        private Texture2D silverCoinSheet;
        private Texture2D goldCoinSheet;
        private Texture2D healthTexture;

        private Rectangle healthRectangle;
        private Rectangle projectileRectangle;

        private SoundEffect flameEffect;
        private Random rand;

        private readonly CoinsController coinsController;

        private PlayerSprite shipSprite;
        private List<Sprite> projectiles;
        private List<Sprite> colidableSprites;        
        private List<Texture2D> asteroids;

        private Dictionary<Texture2D, int> coins;
        private Dictionary<Texture2D, int> ammunition;
        private Dictionary<Texture2D, Rectangle> GUI;
        private Dictionary<Texture2D, int> upgradeRequirements;

        private Projectile projectile;
        private MouseState pastMouse;        
        private MouseState mouse;
        private GameStates gameState;
        private Rectangle mouseRect;
        private SoundEffect upgradeEffect;
        private SoundEffect reloadEffect;
        private SoundEffect gameOverEffect;
        private Vector2 stringPossition;

        private int score;
        private int health;
        private int maxHealth;
        private float timer = 0;
        private float stringTimer = 0;
        private bool gameOver;
        private int ammoToIncrease = 20;
        private bool IsPlaying = true;
        private bool hasUpgraded = false;

        public App05Game()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            rand = new Random();
            mouse = Mouse.GetState();

            coinsController = new CoinsController();
            projectiles = new List<Sprite>();
            colidableSprites = new List<Sprite>();
            asteroids = new List<Texture2D>();

            ammunition = new Dictionary<Texture2D, int>();            
            coins = new Dictionary<Texture2D, int>();
            GUI = new Dictionary<Texture2D, Rectangle>();
            upgradeRequirements = new Dictionary<Texture2D, int>();            

            projectileRectangle = new Rectangle(30, 10, 50, 50);
            mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);            
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

            score = 0;
            health = maxHealth = 1000;
            gameState = GameStates.Playing;
            base.Initialize();
        }

        /// <summary>
        /// use Content to load your game images, fonts,
        /// music and sound effects
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundImage = Content.Load<Texture2D>("backgrounds/bg58");
            healthTexture = Content.Load<Texture2D>("Actors/healthTexture");

            LoadCoins();
            LoadAsteroids();
            LoadAmmunition();
            LoadShips();

            // Load Music and SoundEffects
            SoundController.LoadContent(Content);
            SoundController.PlaySong("Adventure");
            flameEffect = SoundController.GetSoundEffect("Flame");
            upgradeEffect = SoundController.GetSoundEffect("upgrade");
            reloadEffect = SoundController.GetSoundEffect("reload");
            gameOverEffect = SoundController.GetSoundEffect("gameover");

            // Load Fonts

            arialFont = Content.Load<SpriteFont>("fonts/arial");
            calibriFont = Content.Load<SpriteFont>("fonts/calibri");

            // suitable for asteroids type game

            SetupSpaceShip();
            SetupAsteroid();
            LoadGUI();
        }

        private void LoadShips()
        {
            int requirement = 0;
            int increase = 5000;

            for (int i = 1; i < 13; i++)
            {       
                upgradeRequirements.Add(Content.Load<Texture2D>("Actors/ship" + i), requirement);
                requirement += increase;
            }
        }

        /// <summary>
        /// Load GUI objects in the dictionary.
        /// </summary>
        private void LoadGUI()
        {
            int squareSize = 50;
            int YPos = 10;

            Texture2D pause = Content.Load<Texture2D>("Actors/pause");
            Rectangle pauseRect = new Rectangle(HD_Width - squareSize, YPos, squareSize, squareSize);
            Texture2D play = Content.Load<Texture2D>("Actors/play");
            Rectangle playRect = new Rectangle(HD_Width - squareSize, YPos, squareSize, squareSize);
            Texture2D sound = Content.Load<Texture2D>("Actors/sound");
            Rectangle soundRect = new Rectangle(HD_Width - 2 * squareSize, YPos, squareSize, squareSize);
            Texture2D soundMute = Content.Load<Texture2D>("Actors/soundMute");
            Rectangle soundMuteRect = new Rectangle(HD_Width - 2 * squareSize, YPos, squareSize, squareSize);
            Texture2D star = Content.Load<Texture2D>("Actors/star");
            Rectangle starRect = new Rectangle(4 * squareSize, YPos, squareSize, squareSize);
            Texture2D healthIcon = Content.Load<Texture2D>("Actors/health");
            Rectangle healthRect = new Rectangle(HD_Width/2 - health/2,HD_Height - 2*squareSize, squareSize, squareSize);

            GUI.Add(pause, pauseRect);
            GUI.Add(play, playRect);
            GUI.Add(sound, soundRect);
            GUI.Add(soundMute, soundMuteRect);
            GUI.Add(star, starRect);
            GUI.Add(healthIcon, healthRect);
            GUI.Add(healthTexture, new Rectangle(0, 0, 0, 0));
        }

        /// <summary>
        /// Load al the ammunition images in the ammunition dictionary
        /// along with the damage they can inflict.
        /// </summary>
        private void LoadAmmunition()
        {
            int damageMultiplier = 10;

            for (int i = 1; i < 8; i++)
            {
                ammunition.Add(Content.Load<Texture2D>("Actors/ammo" + i), damageMultiplier * i);
            }
        }
        /// <summary>
        /// Load all the asteroids images in the list.
        /// </summary>
        private void LoadAsteroids()
        {
            for (int i = 1; i < 28; i++)
            {
                asteroids.Add(Content.Load<Texture2D>("Actors/Stones2Filled_" + i));
            }
        }
        /// <summary>
        /// Load all the coin images along with their value.
        /// </summary>
        private void LoadCoins()
        {
            cooperCoinSheet = Content.Load<Texture2D>("Actors/coin_copper");
            silverCoinSheet = Content.Load<Texture2D>("Actors/coin_silver");
            goldCoinSheet = Content.Load<Texture2D>("Actors/coin_gold");

            coins.Add(cooperCoinSheet, (int)CoinColours.Copper) ;
            coins.Add(silverCoinSheet, (int)CoinColours.Silver);
            coins.Add(goldCoinSheet, (int)CoinColours.Gold);
        }

        /// <summary>
        /// This is a single image sprite that rotates
        /// and move at a constant speed in a fixed direction
        /// </summary>
        private void SetupAsteroid()
        {               
            Texture2D asteroid = asteroids[rand.Next(0, 27)];

            colidableSprites.Add(new Sprite(asteroid, HD_Width, rand.Next(asteroid.Height/2, HD_Height - asteroid.Height/2))
            {
                Direction = new Vector2(-1, 0),
                Speed = 100,

                Rotation = MathHelper.ToRadians(3),
                RotationSpeed = 2f,

                Health = 100
            });
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
            projectile = new Projectile(Content.Load<Texture2D>("Actors/lunar_0001"),
                        200, 300);

            shipSprite = new PlayerSprite(upgradeRequirements.ElementAt(0).Key, 200, 500)
            {
                Direction = new Vector2(1, 0),
                Speed = 200,
                DirectionControl = DirectionControl.Rotational,
                Projectile = projectile
            };            
        }

        /// <summary>
        /// Called 60 frames/per second and updates the positions
        /// of all the drawable objects
        /// </summary>
        /// <param name="gameTime">
        /// Can work out the elapsed time since last call if
        /// you want to compensate for different frame rates
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();
            mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            ValidateGame();
            ValidateSound();
            
            if (gameState == GameStates.Playing)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                stringTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                    Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

                // Update Asteroids
                if (!gameOver)
                    shipSprite.Update(gameTime, projectiles);

                if (timer > 2)
                {
                    SetupAsteroid();

                    if (health >= maxHealth)
                        health = maxHealth;
                    else
                        health++;

                    timer = 0;
                }

                if (UpgradeShip())
                {
                    hasUpgraded = true;
                    stringTimer = 0;
                    stringPossition = new Vector2(HD_Width / 2 - arialFont.MeasureString("Upgrade Ship!").X / 2, HD_Height / 2);
                }

                coinsController.Update(gameTime, projectiles);
                score += coinsController.HasCollided(shipSprite);
                UpdateSprites(gameTime);
                CheckCollisionWithSprites();
                CheckScreenCollision();
                UpdateDynamicIcons();
            }
            pastMouse = mouse;

            base.Update(gameTime);
        }
        /// <summary>
        /// Change the ship image.
        /// </summary>
        private bool UpgradeShip()
        {
            Texture2D image = shipSprite.Image;

            for (int i = 0; i < upgradeRequirements.Count; i++)
            {
                if (score >= upgradeRequirements.ElementAt(i).Value)
                {
                    shipSprite.Image = upgradeRequirements.ElementAt(i).Key;                    
                }
            }
            shipSprite.Origin = new Vector2(shipSprite.Image.Width/2, shipSprite.Image.Height/2);

            if (image != shipSprite.Image)
            {
                upgradeEffect.Play();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Pause or Resume the game
        /// </summary>
        private void ValidateGame()
        {
            if (mouseRect.Intersects(GUI.ElementAt(0).Value) && (mouse.LeftButton == ButtonState.Pressed &&
                pastMouse.LeftButton == ButtonState.Released))
            {
                if (gameState == GameStates.Playing)
                {
                    gameState = GameStates.Paused;
                    IsPlaying = false;
                }
                else
                {
                    gameState = GameStates.Playing;
                    IsPlaying = true;
                }
            }
        }
        /// <summary>
        /// Mute or unmute the sound.
        /// </summary>
        private void ValidateSound()
        {
            if (mouseRect.Intersects(GUI.ElementAt(2).Value) && (mouse.LeftButton == ButtonState.Pressed &&
                pastMouse.LeftButton == ButtonState.Released))
            {
                if (MediaPlayer.IsMuted)
                    MediaPlayer.IsMuted = false;
                else
                {
                    MediaPlayer.IsMuted = true;
                }
            }
        }
        /// <summary>
        /// Update dynamic icons such as the health bar.
        /// </summary>
        private void UpdateDynamicIcons()
        {
            int healthIconIndex = 5;

            healthRectangle = new Rectangle((int)GUI.ElementAt(healthIconIndex).Value.X + 50, 
                (int)GUI.ElementAt(healthIconIndex).Value.Y, health, GUI.ElementAt(healthIconIndex).Value.Height);
            GUI[healthTexture] =  healthRectangle;
        }

        /// <summary>
        /// Validate the player's collision with the screen edges.
        /// </summary>
        private void CheckScreenCollision()
        {
            float distanceYBottom = shipSprite.Position.Y + shipSprite.Image.Height / 2;
            float distanceYTop = shipSprite.Position.Y - shipSprite.Image.Height / 2;
            float distanceXRight = shipSprite.Position.X + shipSprite.Image.Width / 2;
            float distanceXLeft = shipSprite.Position.X - shipSprite.Image.Width / 2;

            if (distanceXLeft <= 0)
                shipSprite.Position = new Vector2(shipSprite.Image.Width / 2, shipSprite.Position.Y);
            if (distanceXRight >= HD_Width)
                shipSprite.Position = new Vector2(HD_Width - shipSprite.Image.Width / 2, shipSprite.Position.Y);
            if (distanceYTop <= 0)
                shipSprite.Position = new Vector2(shipSprite.Position.X, shipSprite.Image.Height / 2);
            if (distanceYBottom >= HD_Height)
                shipSprite.Position = new Vector2(shipSprite.Position.X, HD_Height - shipSprite.Image.Height / 2);
        }
        /// <summary>
        /// Validate collision for asteroids.
        /// </summary>
        private void CheckCollisionWithSprites()
        {
            for (int j = 0; j < colidableSprites.Count; j++)
            {
                CheckAsteroidCollision(j);
            }
        }
        /// <summary>
        /// Validate collision between player ship and a colidable sprite.
        /// If a colidable sprite is type ammunition, increase ammunition. 
        /// </summary>
        /// <param name="j"></param>
        private void CheckAsteroidCollision(int j)
        {
            if (!ammunition.ContainsKey(colidableSprites[j].Image))
            {
                if (shipSprite.HasCollided(colidableSprites[j]) && shipSprite.IsAlive &&
                    colidableSprites[j].IsAlive)
                {
                    flameEffect.Play();
                    colidableSprites[j].IsAlive = false;
                    colidableSprites[j].IsActive = false;
                    colidableSprites[j].IsVisible = false;

                    health -= colidableSprites[j].Health;
                    ValidatePlayerHealth();
                }
            }            
            else
                AddAmmo(j);
        }
        /// <summary>
        /// Validate the player ship health.
        /// </summary>
        private void ValidatePlayerHealth()
        {
            if (health <= 0)
            {
                health = 0;

                shipSprite.IsActive = false;
                shipSprite.IsAlive = false;
                shipSprite.IsVisible = false;

                gameOver = true;
                gameOverEffect.Play();
            }
        }
        /// <summary>
        /// Add ammo to player.
        /// </summary>
        /// <param name="j"></param>
        private void AddAmmo(int j)
        {
            if (ammunition.ContainsKey(colidableSprites[j].Image)
                            && colidableSprites[j].BoundingBox.Intersects(shipSprite.BoundingBox))
            {
                ChangeProjectileImage(j);
                SetRotatebleProjectile(j);
                if(colidableSprites[j].IsAlive)
                    reloadEffect.Play();

                colidableSprites[j].IsAlive = false;
                colidableSprites[j].IsActive = false;
                colidableSprites[j].IsVisible = false; 
            }
        }
        /// <summary>
        /// Change the projectile image on collision with the projectile 
        /// and increase the projectile amount.
        /// </summary>
        /// <param name="j"></param>
        private void ChangeProjectileImage(int j)
        {
            if (colidableSprites[j].IsAlive)
                shipSprite.Ammo += ammoToIncrease;

            projectile.Image = colidableSprites[j].Image;
            projectile.Damage = ammunition[colidableSprites[j].Image];
        }
        /// <summary>
        /// Some of the projectiles require spinning.
        /// Set the rotation speed for this projectiles.
        /// </summary>
        /// <param name="j">projectile</param>
        private void SetRotatebleProjectile(int j)
        {
            int startIndex = 4;

            for (int i = startIndex; i < ammunition.Count; i++)
            {
                if (ammunition.ElementAt(i).Key == colidableSprites[j].Image)
                {
                    projectile.RotationSpeed = 20;

                    i = ammunition.Count + 1;
                }
                else
                    projectile.RotationSpeed = 0;
            }
        }
        /// <summary>
        /// Update sprite list.
        /// </summary>
        private void UpdateSprites(GameTime gameTime)
        {
            foreach (var sprite in projectiles.ToArray())
                sprite.Update(gameTime, projectiles);

            foreach (var sprite in colidableSprites.ToArray())
                sprite.Update(gameTime, projectiles);
            
            PostUpdateSprites();
        }
        /// <summary>
        /// Check for collision between projectiles and other sprites.
        /// </summary>
        private void PostUpdateSprites()
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int j = 0; j < colidableSprites.Count; j++)
                {
                    try
                    {
                        CheckSpritesCollision(i, j);
                        RemoveSprites(i, j);
                        RemoveOffScreenAsteroids(j);
                    }
                    catch (Exception) { }
                }
            }
        }
        /// <summary>
        /// Remove asteroids that leave the screen.
        /// </summary>
        /// <param name="j"></param>
        private void RemoveOffScreenAsteroids(int j)
        {
            if (colidableSprites[j].Position.X < 0 - colidableSprites[j].Image.Width)
            {
                colidableSprites[j].IsAlive = false;
            }
        }
        /// <summary>
        /// Remove projectiles that expired.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void RemoveSprites(int i, int j)
        {
            if (!projectiles[i].IsAlive)
                projectiles.RemoveAt(i);

            SpawnCoins(j);
        }
        /// <summary>
        /// Spawn coins when asteroids are destroyed.
        /// </summary>
        /// <param name="j"></param>
        private void SpawnCoins(int j)
        {
            int chance = 100;
            int chanceForCoins = 20;

            if (!colidableSprites[j].IsAlive)
            {
                int x = rand.Next(0, chance);
                if (x <= chanceForCoins)
                {
                    int index = rand.Next(0, coins.Count + 1);
                    coinsController.CreateCoin(graphicsDevice, coins.ElementAt(index).Key,
                        colidableSprites[j].Position, coins.ElementAt(index).Value);
                }
                colidableSprites.RemoveAt(j);
            }
        }
        /// <summary>
        /// Check collision between a projectile and an asteroid.
        /// </summary>
        /// <param name="i">index of projectile.</param>
        /// <param name="j">index of asteroid</param>
        private void CheckSpritesCollision(int i, int j)
        {
            if (projectiles[i].BoundingBox.Intersects(colidableSprites[j].BoundingBox)
                && colidableSprites[j].IsAlive)
            {
                if (ammunition.ContainsKey(colidableSprites[j].Image))
                    return;

                colidableSprites[j].Health -= projectiles[i].Damage;
                projectiles[i].IsAlive = false;
                ValidateAsteroidHealth(j);
            }
        }
        /// <summary>
        /// Verify and update the health of an asteroid.
        /// </summary>
        /// <param name="j">index of asteroid</param>
        private void ValidateAsteroidHealth(int j)
        {
            int max = 100;
            int chanceForAmmo = 70;

            if (colidableSprites[j].Health <= 0 && rand.Next(0, max) <= chanceForAmmo)
            {
                colidableSprites[j].IsAlive = false;

                score += 500;
            }
            else 
                SpawnAmmo(j);
        }
        /// <summary>
        /// Spawn random ammo where the asteroid died.
        /// </summary>
        /// <param name="j"></param>
        private void SpawnAmmo(int j)
        {
            int x = rand.Next(0, 100);

            if (colidableSprites[j].Health <= 0)
            {
                int index = rand.Next(0, ammunition.Count + 1);

                colidableSprites[j].Image = ammunition.ElementAt(index).Key;
                colidableSprites[j].Rotation = 0f;
                colidableSprites[j].Origin = new Vector2(colidableSprites[j].Image.Width / 2,
                    colidableSprites[j].Image.Height / 2);
            }
        }

        /// <summary>
        /// Called 60 frames/per second and Draw all the 
        /// sprites and other drawable images here
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LawnGreen);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, HD_Width, HD_Height), Color.White);

            // Draw asteroids game
            foreach (var sprite in projectiles)
                sprite.Draw(spriteBatch);
            DrawAsteroidHealth();
            shipSprite.Draw(spriteBatch);
            coinsController.Draw(spriteBatch);

            DrawGameStatus(spriteBatch);
            DrawGameFooter(spriteBatch);
            DrawGameOverMessage(spriteBatch);
            DrawGameStatusMessage(spriteBatch);
            DrawUpgradeMessage(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawGameOverMessage(SpriteBatch spriteBatch)
        {
            if (gameOver)
            {
                spriteBatch.DrawString(arialFont, "Game Over!",
                    new Vector2(HD_Width / 2 - arialFont.MeasureString("Game Over!").X / 2, HD_Height / 2),
                    Color.White);

                MediaPlayer.IsMuted = true;
            }
        }

        private void DrawGameStatusMessage(SpriteBatch spriteBatch)
        {
            if (!IsPlaying)
            {
                spriteBatch.DrawString(arialFont, "Game Paused!",
                    new Vector2(HD_Width / 2 - arialFont.MeasureString("Game Paused!").X / 2, HD_Height / 2),
                    Color.White);
            }
        }

        private void DrawUpgradeMessage(SpriteBatch spriteBatch)
        {
            if (hasUpgraded)
            {
                if (stringTimer <= 2)
                {
                    spriteBatch.DrawString(arialFont, "Upgrade Ship!",
                        stringPossition, Color.White);

                    stringPossition += new Vector2(0, -1.5f);
                }
                else
                    hasUpgraded = false;
            }
        }

        private void DrawAsteroidHealth()
        {
            foreach (var sprite in colidableSprites)
            {
                sprite.Draw(spriteBatch);

                if (!ammunition.ContainsKey(sprite.Image) && sprite.IsAlive)
                {
                    Rectangle asteroidRectangle = new Rectangle((int)sprite.Position.X - healthTexture.Width / 2,
                        (int)sprite.Position.Y, sprite.Health, 10);

                    spriteBatch.Draw(healthTexture, asteroidRectangle, Color.White);
                }
            }
        }

        /// <summary>
        /// Display the name fo the game and the current score
        /// and health of the player at the top of the screen
        /// </summary>
        public void DrawGameStatus(SpriteBatch spriteBatch)
        {
            string game = "Space Invaders";
            Vector2 gameSize = arialFont.MeasureString(game);
            Vector2 topCentre = new Vector2((HD_Width / 2 - gameSize.X / 2), 4);
            spriteBatch.DrawString(arialFont, game, topCentre, Color.White);

            DrawGUI(spriteBatch);
        }

        private void DrawGUI(SpriteBatch spriteBatch)
        {
            int star = 4;
            int sound = 2;

            for (int i = 0; i < GUI.Count; i++)
            {               
                var colour = Color.Gold;

                if (i == 0 && gameState == GameStates.Paused)
                    continue;
                if (i == 1 && gameState == GameStates.Playing)
                    continue;
                if (i == sound && MediaPlayer.IsMuted)
                    continue;
                if (i == sound + 1 && !MediaPlayer.IsMuted)
                    continue;

                if (i != star)
                    colour = Color.Red;

                spriteBatch.Draw(GUI.ElementAt(i).Key, GUI.ElementAt(i).Value, colour);
            }
            spriteBatch.Draw(projectile.Image, projectileRectangle, Color.White);
            spriteBatch.DrawString(arialFont, $"{score}",
                new Vector2(GUI.ElementAt(star).Value.X + GUI.ElementAt(star).Value.Width + 10, GUI.ElementAt(star).Key.Height / 2),
                Color.White);
            spriteBatch.DrawString(arialFont, $"{shipSprite.Ammo}", new Vector2(
                projectileRectangle.X + projectileRectangle.Width + 10, projectileRectangle.Height / 2),
                Color.White);
        }

        /// <summary>
        /// Display the Module, the authors and the application name
        /// at the bottom of the screen
        /// </summary>
        public void DrawGameFooter(SpriteBatch spriteBatch)
        {
            int margin = 20;

            string names = "Derek & Andrei";
            string app = "App05: MonogGame";
            string module = "BNU CO453-2020";

            Vector2 namesSize = calibriFont.MeasureString(names);
            Vector2 appSize = calibriFont.MeasureString(app);

            Vector2 bottomCentre = new Vector2((HD_Width - namesSize.X)/ 2, HD_Height - margin);
            Vector2 bottomLeft = new Vector2(margin, HD_Height - margin);
            Vector2 bottomRight = new Vector2(HD_Width - appSize.X - margin, HD_Height - margin);

            spriteBatch.DrawString(calibriFont, names, bottomCentre, Color.Yellow);
            spriteBatch.DrawString(calibriFont, module, bottomLeft, Color.Yellow);
            spriteBatch.DrawString(calibriFont, app, bottomRight, Color.Yellow);
        }        
    }
}
