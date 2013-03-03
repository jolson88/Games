using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jarrett.Core;
using Jarrett.Views;

namespace Jarrett
{
    static class JarrettGameLevels
    {
        public static string TicTacToe = "TicTacToe";
        // TODO: Add other game levels here (like pong)
    }

    class JarrettGame : Game, IGame
    {
        ProcessManager m_processManager;
        IResourceManager m_resourceManager;
        GraphicsDeviceManager m_deviceManager;
        GameState m_state;
        ILevelLoader m_levelLoader;
        List<IGameView> m_views;
        int m_nextActorId;
        Dictionary<int, GameActor> m_actors;

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
            m_processManager = new ProcessManager();

            m_views = new List<IGameView>();
            m_actors = new Dictionary<int, GameActor>();

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
            m_resourceManager.Initialize();

            RegisterMessageListeners();
            m_processManager.AttachProcess(new MessageProcessor());
            m_processManager.AttachProcess(new InputProcessor());

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
            m_processManager.UpdateProcesses(gameTime);

            // Update from front to back. Most recently active (like a pause view) will be at the front
            for (int i = 0; i < m_views.Count; i++)
            {
                m_views[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // Draw from back to front. Most recently active (like a pause view) will be drawn last
            for (int i = m_views.Count - 1; i >= 0; i--)
            {
                m_views[i].Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        protected void ChangeGameState(GameState newState)
        {
            m_state = newState;

            switch (newState)
            {
                case GameState.Initialized:
                    ChangeGameState(GameState.MainMenu);
                    break;

                case GameState.MainMenu:
                    m_actors.Clear();
                    m_views.Clear();

                    var menuView = new MainMenuView();
                    menuView.Initialize(this);

                    m_views.Add(menuView);
                    break;

                case GameState.NewGameRequested:
                    Debug.Assert(m_levelLoader != null);
                    m_actors.Clear();
                    m_views.Clear();

                    // Build the level from the current level loader
                    m_views.AddRange(m_levelLoader.LoadViews());
                    foreach (var actor in m_levelLoader.LoadActors())
                    {
                        AddActor(actor);
                    }

                    ChangeGameState(GameState.Running);
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
                var newGameMsg = (NewGameRequestMessage)msg;
                m_levelLoader = new JarrettLevelLoader(this, newGameMsg.LevelName, newGameMsg.HumanPlayers, newGameMsg.TotalPlayers);

                ChangeGameState(GameState.NewGameRequested);
            });
        }

        protected void AddActor(GameActor actor)
        {
            m_actors.Add(m_nextActorId, actor);
            m_nextActorId++;
        }
    }
}
