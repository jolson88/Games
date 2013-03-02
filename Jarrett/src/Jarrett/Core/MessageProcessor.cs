using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Jarrett.Core
{
    class MessageProcessor : Process
    {
        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            MessageBus.Get().ProcessMessages();
        }
    }
}
