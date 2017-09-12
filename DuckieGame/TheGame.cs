using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using DuckieGame.Character;
using DuckieGame.Level;

namespace DuckieGame
{
    class TheGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D[] wallTextures = new Texture2D[16];

        PlayerCharacter player;
        DuckieGame.Level.Level level;

        float waitTime = 1;

        List<Keys> prevPressedKeys = new List<Keys>();
        List<Keys> pressedKeys = new List<Keys>();

        AnimatedTexture SpriteTexture;
        private const float Rotation = 0;
        private const float Scale = 2.0f;
        private const float Depth = 0.5f;
        private const int Frames = 4;
        private const int FramesPerSec = 2;
        private Vector2 shipPos;

        public TheGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferHeight = 1040;
            graphics.PreferredBackBufferWidth = 1040;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;

            //maze = new MazeGenerator();
            //maze.GenerateMaze();

            //caGenerator.InitMap();

            level = new DuckieGame.Level.Level();
            level.CreateMap();

            player = new PlayerCharacter();
            player.Position = level.GetLastCell();

            SpriteTexture = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            level.LoadTexture("wall", Texture2D.FromStream(GraphicsDevice, new System.IO.FileStream("Content\\Wall.png", System.IO.FileMode.Open)));
            level.LoadTexture("floor", Texture2D.FromStream(GraphicsDevice, new System.IO.FileStream("Content\\Floor.png", System.IO.FileMode.Open)));

            player.Texture = Content.Load<Texture2D>("Character");

            System.Drawing.Bitmap bitmap = MapGenerator.PerlinNoise.CreatePerlinNoise();
            
            int bufferSize = bitmap.Height * bitmap.Width;

            System.Drawing.Imaging.BitmapData bData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, 512, 512), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bufferSize = bData.Height * bData.Stride;

            byte[] bytes = new byte[bufferSize];
            int[] imgData = new int[bitmap.Height * bitmap.Width];

            System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, imgData, 0, 512 * 512);


            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(bufferSize);
            //System.IO.FileStream fs = new System.IO.FileStream("PerlinNoise.png", System.IO.FileMode.Create);
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            //memoryStream.Close();
            Texture2D texture = new Texture2D(GraphicsDevice, 512, 512);

            texture.SetData(imgData);
            

            //player.Texture = texture;
            player.Position = new Point(0, 0);

            SpriteTexture.Load(Content, "shipanimated", FramesPerSec, FramesPerSec);
            //shipPos = player.Position;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            prevPressedKeys = pressedKeys;
            pressedKeys = Keyboard.GetState().GetPressedKeys().ToList();

            if (!pressedKeys.Contains(Keys.F5) && prevPressedKeys.Contains(Keys.F5))
            {
                level.TrimMap();
                //caGenerator.InitMap();
                //caGenerator.Generation();
            }

            if (!pressedKeys.Contains(Keys.F4) && prevPressedKeys.Contains(Keys.F4))
            {
                level.CreateMap();
                //caGenerator.InitMap();
                //caGenerator.Generation();
            }

            if (!pressedKeys.Contains(Keys.Left) && prevPressedKeys.Contains(Keys.Left))
            {
                level.UpdateMap(3);
                player.Position = level.GetLastCell();
                /*
                Point newPosition = new Point(player.Position.X - 1, player.Position.Y);
                if (!level.CheckWall(newPosition))
                {
                    player.Position = newPosition;
                }
                */
            }

            if (!pressedKeys.Contains(Keys.Right) && prevPressedKeys.Contains(Keys.Right))
            {
                level.UpdateMap(2);
                player.Position = level.GetLastCell();
                /*
                Point newPosition = new Point(player.Position.X + 1, player.Position.Y);
                if (!level.CheckWall(newPosition))
                {
                    player.Position = newPosition;
                }
                */
            }

            if (!pressedKeys.Contains(Keys.Up) && prevPressedKeys.Contains(Keys.Up))
            {
                level.UpdateMap(0);
                player.Position = level.GetLastCell();
                /*
                Point newPosition = new Point(player.Position.X, player.Position.Y - 1);
                if (!level.CheckWall(newPosition))
                {
                    player.Position = newPosition;
                }
                */
            }

            if (!pressedKeys.Contains(Keys.Down) && prevPressedKeys.Contains(Keys.Down))
            {
                level.UpdateMap(1);
                player.Position = level.GetLastCell();
                /*
                Point newPosition = new Point(player.Position.X, player.Position.Y + 1);
                if (!level.CheckWall(newPosition))
                {
                    player.Position = newPosition;
                }
                */
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            SpriteTexture.UpdateFrame(elapsed);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();


            level.Draw(spriteBatch);
            player.Draw(spriteBatch);
            //SpriteTexture.DrawFrame(spriteBatch, new Vector2(15, 30));

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
