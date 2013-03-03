using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jarrett;
using Jarrett.Core;

namespace Jarrett.Views
{
    class HumanTicTacToeView : IGameView
    {
        // TODO: Refactor MainMenuView into base GameView that both it and HumanTicTacToeView derive from
        SpriteBatch m_batch;
        SpriteFont m_font;

        public void Initialize(IGame game)
        {
            m_batch = new SpriteBatch(game.Device);
            m_font = game.Resources.Load<SpriteFont>("Graphics\\MenuFont");
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            m_batch.Begin();
            m_batch.DrawString(m_font, "Playing Tic Tac Toe", Vector2.Zero, Color.Blue);
            m_batch.End();
        }
    }
}
