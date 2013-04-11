using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Jarrett
{
    public class Screen
    {
        protected GraphicsDevice GraphicsDevice { get; private set; }
        protected ContentManager Content { get; private set; }
        protected SpriteBatch Batch { get; private set; }       
        private Background _background;

        public void Load(GraphicsDevice device, ContentManager content)
        {
            this.GraphicsDevice = device;
            this.Content = content;
            this.Batch = new SpriteBatch(device);

            _background = InitializeBackground();
            OnLoad();
        }

        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Batch.Begin();

            if (_background != null)
            {
                Batch.Draw(_background.Texture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            }

            OnDraw(gameTime);
            Batch.End();
        }

        protected virtual Background InitializeBackground() { return null; }
        protected virtual void OnLoad() { }

        // TODO: Remove these if possible (I don't think they will be needed once everything is loaded game objects)
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }
    }
}
