using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jarrett.Core;
using Jarrett.Views;

namespace Jarrett
{
    class JarrettGame : Game, IGame
    {
        IResourceManager m_resourceManager;
        GraphicsDeviceManager m_deviceManager;
        GameState m_state;
        List<IGameView> m_gameViews;

        public GraphicsDevice Device
        {
            get { return m_deviceManager.GraphicsDevice; }
        }

        public IResourceManager Resources
        {
            get { return m_resourceManager; }
        }

        public JarrettGame()
        {
            m_deviceManager = new GraphicsDeviceManager(this);
            m_resourceManager = new JarrettResourceManager(this.Content);

            m_gameViews = new List<IGameView>();

            ChangeGameState(GameState.Initializing);
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

            m_resourceManager.Initialize();

            ChangeGameState(GameState.Initialized);
        }

        protected override void LoadContent()
        {
            m_resourceManager.LoadContent();
        }

        protected override void UnloadContent()
        {
            m_resourceManager.UnloadContent();
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
                    break;

                case GameState.Running:
                    // TODO: Update Processes
                    break;

                default:
                    break;
            }

            // Update from front to back. Most recently active (like a pause view) will be at the front
            for (int i = 0; i < m_gameViews.Count; i++)
            {
                m_gameViews[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw from back to front. Most recently active (like a pause view) will be drawn last
            for (int i = m_gameViews.Count - 1; i >= 0; i--)
            {
                m_gameViews[i].Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        protected void ChangeGameState(GameState newState)
        {
            m_state = newState;

            switch (newState)
            {
                case GameState.MainMenu:
                    m_gameViews.Clear();

                    var menuView = new MainMenuView();
                    menuView.Initialize(this);

                    m_gameViews.Add(menuView);
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
                // TODO: Set a game loader that OnUpdate will use to build the game
                // Then, OnUpdate will change game state to Running instead of here.
                ChangeGameState(GameState.Running);
            });
        }
    }
}
