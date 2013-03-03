using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Jarrett.Core;

namespace Jarrett.Views
{
    class MainMenuView : IGameView
    {
        static string s_onePlayer = "New Game - One Player";
        static string s_twoPlayers = "New Game - Two Player";

        GraphicsDevice m_device;
        ProcessManager m_processManager;
        SpriteBatch m_batch;
        SpriteFont m_menuFont;
        double m_menuFontHeight;

        int m_currentMenuOption;
        string[] m_menuOptions = { s_onePlayer, s_twoPlayers };

        public void Initialize(IGame game)
        {
            m_device = game.Device;
            m_processManager = new ProcessManager();
            m_batch = new SpriteBatch(game.Device);
            m_menuFont = game.Resources.Load<SpriteFont>("Graphics\\MenuFont");
            RegisterMessageListeners();

            var fontSize = m_menuFont.MeasureString("Hello Word");
            m_menuFontHeight = fontSize.Y;
        }

        public void Update(GameTime gameTime)
        {
            m_processManager.UpdateProcesses(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            m_batch.Begin();

            // TODO: Refactor these options into Screen Elements (Where "Menu" is one screen element)
            m_batch.DrawString(m_menuFont, "Tic Tac Toe", Vector2.Zero, Color.Blue);

            Color fontColor;
            for (int i = 0; i < m_menuOptions.Length; i++)
            {
                fontColor = (i == m_currentMenuOption) ? Color.Red : Color.DarkGray;
                m_batch.DrawString(m_menuFont,
                    m_menuOptions[i],
                    new Vector2(0.0f, m_device.Viewport.Height / 2 + (float)m_menuFontHeight * i),
                    fontColor);
            }

            m_batch.End();
        }

        private void RegisterMessageListeners()
        {
            MessageBus.Get().AddListener<KeyDownMessage>(msg =>
            {
                var keyMsg = (KeyDownMessage)msg;
                if (keyMsg.Key == Keys.Up) 
                {
                    m_currentMenuOption = (m_currentMenuOption + 1) % m_menuOptions.Length;
                }
                if (keyMsg.Key == Keys.Down) 
                {
                    m_currentMenuOption = (m_currentMenuOption - 1 + m_menuOptions.Length) % m_menuOptions.Length;
                }
                if (keyMsg.Key == Keys.Enter)
                {
                    if (m_menuOptions[m_currentMenuOption] == s_onePlayer)
                    {
                        MessageBus.Get().QueueMessage(new NewGameRequestMessage(JarrettGameLevels.TicTacToe, 1));
                    }
                    else
                    {
                        MessageBus.Get().QueueMessage(new NewGameRequestMessage(JarrettGameLevels.TicTacToe, 2));
                    }
                }
            });
        }
    }
}
