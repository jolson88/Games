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
        int m_counter;
        SpriteBatch m_batch;
        SpriteFont m_font;

        public void Initialize(IGame game)
        {
            m_batch = new SpriteBatch(game.Device);
            m_font = game.Resources.Load<SpriteFont>("Graphics\\MenuFont");
            RegisterMessageListeners();
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Update Processes
        }

        public void Draw(GameTime gameTime)
        {
            m_batch.Begin();
            m_batch.DrawString(m_font, "Hello World: " + m_counter, Vector2.Zero, Color.Red);
            m_batch.End();
        }

        private void RegisterMessageListeners()
        {
            MessageBus.Get().AddListener<KeyDownMessage>(msg =>
            {
                var keyMsg = (KeyDownMessage)msg;
                if (keyMsg.Key == Keys.Up) { m_counter++; }
                if (keyMsg.Key == Keys.Down) { m_counter--; }
            });
        }
    }
}
