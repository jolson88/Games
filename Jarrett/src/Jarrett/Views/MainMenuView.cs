using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jarrett.Core;

namespace Jarrett.Views
{
    class MainMenuView : IGameView
    {
        IGame m_game;
        SpriteBatch m_batch;
        SpriteFont m_font;

        public void Initialize(IGame game)
        {
            m_batch = new SpriteBatch(game.Device);
            m_font = game.Resources.Load<SpriteFont>("Graphics\\MenuFont");
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Update Processes
        }

        public void Draw(GameTime gameTime)
        {
            m_batch.Begin();
            m_batch.DrawString(m_font, "Hello World", Vector2.Zero, Color.Red);
            m_batch.End();
        }
    }
}
