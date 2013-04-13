using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Acorn.Hiromi.Messaging;

namespace Acorn.Hiromi.Processing
{
    public class BoundsCheckingProcess : Process
    {
        protected override void OnUpdate(GameTime gameTime)
        {
            var viewport = GraphicsService.Instance.GraphicsDevice.Viewport;
            foreach (var obj in GameObjectService.Instance.GetAllGameObjects())
            {
                var boundingRect = new Rectangle() 
                {
                    X = (int)(obj.Position.X * viewport.Width),
                    Y = (int)(obj.Position.Y * viewport.Height)
                };
                if (obj.Sprite != null)
                {
                    boundingRect.Width = obj.Sprite.Texture.Width;
                    boundingRect.Height = obj.Sprite.Texture.Height;
                }
                if (!boundingRect.Intersects(viewport.Bounds))
                {
                    MessageService.Instance.QueueMessage(new OffScreenMessage(obj.Id));
                }
            }
        }
    }
}
