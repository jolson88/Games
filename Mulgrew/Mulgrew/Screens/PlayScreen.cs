using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hiromi;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mulgrew.Screens
{
    class PlayScreen : Screen
    {
        private Camera _camera;
        private SpriteBatch _batch;
        private Texture2D _grid;

        protected override void OnInitialize()
        {
            _batch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);
            _camera = new Camera(this.MessageManager, new Vector2(1600, 900));

            _grid = ContentService.Instance.GetAsset<Texture2D>("Sprites\\Grid");
            
            this.BackgroundColor = Color.CornflowerBlue;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        
        }

        protected override void OnDraw(GameTime gameTime)
        {
            _batch.Begin(_camera);

            _batch.Draw(_grid, new Rectangle(300, 100, _grid.Width, _grid.Height), Color.White);

            _batch.End();
        }
    }
}
