using Hiromi;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FunPhysics
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FunPhysicsGame : Game
    {
        GraphicsDeviceManager _graphics;
        ScreenManager _screenManager;

        public FunPhysicsGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _screenManager = new ScreenManager();
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            GraphicsService.Instance.GraphicsDevice = this.GraphicsDevice;
            ContentService.Instance.Content = this.Content;

            _screenManager.LoadScreen(new Screens.MainScreen());
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
            _screenManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _screenManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
