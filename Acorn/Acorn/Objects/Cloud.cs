using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Acorn.Behaviors;
using Acorn.Hiromi;
using Acorn.Hiromi.Messaging;
using Acorn.Hiromi.Processing;

namespace Acorn.Objects
{
    public class Cloud : GameObject
    {
        private bool _canBeReflected = true;

        public Cloud() : base()
        {
            this.AddComponent(new MovementBehavior(new Vector2(-50, 0)));

            MessageService.Instance.AddListener<OffScreenMessage>(msg => { OnOffScreen((OffScreenMessage)msg); });
        }

        // TODO: Turn into a generic WrapScreenBehavior component
        public void OnOffScreen(OffScreenMessage msg)
        {
            if (_canBeReflected && msg.GameObjectId == this.Id)
            {
                this.Position = new Vector2(GraphicsService.Instance.GraphicsDevice.Viewport.Width, this.Position.Y);
                _canBeReflected = false;
            }

            // Make sure we don't keep resetting ourselves by introducing a delay
            this.ProcessManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(1), 
                new ActionProcess(() => { _canBeReflected = true; })));
        }
    }
}
