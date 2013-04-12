﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Acorn.Hiromi.Processing;

namespace Acorn.Hiromi
{
    public class Screen
    {
        protected ContentManager Content { get; private set; }
        protected SpriteBatch Batch { get; private set; }
        protected ProcessManager ProcessManager { get; private set; }

        private Background _background;

        public void Load(ContentManager content)
        {
            this.Content = content;
            this.Batch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);

            this.ProcessManager = new ProcessManager();
            this.ProcessManager.AttachProcess(new BoundsCheckingProcess());

            _background = InitializeBackground();
            OnLoad();
        }

        public void Update(GameTime gameTime)
        {
            this.ProcessManager.Update(gameTime);

            foreach (var obj in GameObjectService.Instance.GetAllGameObjects())
            {
                obj.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsService.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);
            Batch.Begin();

            if (_background != null)
            {
                Batch.Draw(_background.Texture,
                    new Rectangle(0, 0, GraphicsService.Instance.GraphicsDevice.Viewport.Width, GraphicsService.Instance.GraphicsDevice.Viewport.Height), 
                    Color.White);
            }

            foreach (var obj in GameObjectService.Instance.GetAllGameObjects())
            {
                if (obj.Sprite != null)
                {
                    Batch.Draw(obj.Sprite.Texture,
                        new Vector2(obj.Position.X - obj.Sprite.Center.X, obj.Position.Y - obj.Sprite.Center.Y),
                        Color.White);
                }
            }

            Batch.End();
        }

        protected virtual Background InitializeBackground() { return null; }
        protected virtual void OnLoad() { }
    }
}
