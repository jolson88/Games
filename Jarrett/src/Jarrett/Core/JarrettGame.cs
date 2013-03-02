using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jarrett.Core
{
    public enum GameState
    {
        Initializing,
        Initialized,
        MainMenu,
        GameStarting,
        Running,
        Exiting
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class JarrettGame : Game
    {
        GraphicsDeviceManager m_deviceManager;
        GameState m_state;
   
        public JarrettGame()
        {
            m_deviceManager = new GraphicsDeviceManager(this);
            ChangeGameState(GameState.Initializing);
            
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
            base.Initialize();

            MessageBus.Get().Initialize();
            RegisterMessageListeners();

            ChangeGameState(GameState.Initialized);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
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
            MessageBus.Get().ProcessMessages();

            switch (m_state)
            {
                case GameState.Initialized:
                    ChangeGameState(GameState.MainMenu);
                    break;

                case GameState.MainMenu:
                    // TEMPORARY for testing
                    MessageBus.Get().QueueMessage(new NewGameRequestMessage());

                    break;

                case GameState.Running:
                    // TODO: Update Processes
                    break;

                default:
                    break;
            }

            // TODO: Update Game Views

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Render game views

            base.Draw(gameTime);
        }

        protected void ChangeGameState(GameState newState)
        {
            m_state = newState;

            switch (newState)
            {
                case GameState.MainMenu:
                    break;

                case GameState.Running:
                    break;

                default:
                    break;
            }
        }

        protected void RegisterMessageListeners()
        {
            MessageBus.Get().AddListener<NewGameRequestMessage>(msg =>
            {
                ChangeGameState(GameState.Running);
            });
        }
    }
}
