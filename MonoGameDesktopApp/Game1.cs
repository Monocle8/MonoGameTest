using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace MonoGameDesktopApp
{
    internal class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch? spriteBatch;
        private Texture2D? square;
        readonly Stopwatch watch = new Stopwatch();
        private int gc0 = 0;
        private int gc1 = 0;
        private int gc2 = 0;
        private int timer = 0;

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
			// Should be 1920x1080
			
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //graphics.ToggleFullScreen();
            graphics.ApplyChanges();

            Console.WriteLine("Initialized...");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            square = Content.Load<Texture2D>("Textures/square");
			
			Console.WriteLine("Content Loaded...");

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (GC.CollectionCount(0) > gc0)
            {
                Console.WriteLine($"{gameTime.TotalGameTime.TotalSeconds:N1}: GC0 collection {gc0 + 1}");
            }
            if (GC.CollectionCount(1) > gc1)
            {
                Console.WriteLine($"{gameTime.TotalGameTime.TotalSeconds:N1}: GC1 collection {gc1 + 1}");
            }
            if (GC.CollectionCount(2) > gc2)
            {
                Console.WriteLine($"{gameTime.TotalGameTime.TotalSeconds:N1}: GC2 collection {gc2 + 1}");
            }

            gc0 = GC.CollectionCount(0);
            gc1 = GC.CollectionCount(0);
            gc2 = GC.CollectionCount(0);

            timer++;
            if (timer > 300)
            {
                Console.WriteLine($"{gameTime.TotalGameTime.TotalSeconds:N1}: Memory usage {GC.GetTotalMemory(false) / 1024}K");
                timer = 0;
            }

            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            watch.Reset();
            watch.Start();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch?.Begin();
            for (int i = 0; i < 300; i++)
            {
                spriteBatch?.Draw(square, new Vector2((i * 6 + 60) - (square!.Width / 2), (i * 3 + 90) - (square!.Height / 2)), Color.Red);
                spriteBatch?.Draw(square, new Vector2((i * 6 + 60) - (square!.Width / 2), (990 - i * 3) - (square!.Height / 2)), Color.Red);
            }
            spriteBatch?.End();

            watch.Stop();
            if (gameTime.TotalGameTime.Seconds > 1 && watch.Elapsed.Milliseconds > 20)
            {
                Console.WriteLine($"{gameTime.TotalGameTime.TotalSeconds:N1}: Draw took {watch.Elapsed.TotalMilliseconds:N2}ms");
            }




            base.Draw(gameTime);
        }
    }
}
