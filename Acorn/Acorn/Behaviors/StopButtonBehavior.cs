using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acorn.Hiromi;
using Acorn.Hiromi.Messaging;

namespace Acorn.Behaviors
{
    public class StopButtonBehavior : GameObjectBehavior
    {
        public StopButtonBehavior()
        {
            MessageService.Instance.AddListener<ButtonPressMessage>(msg => OnButtonPress((ButtonPressMessage)msg));
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (msg.GameObjectId == this.GameObject.Id)
            {
                // TODO: Add game logic later.
                System.Diagnostics.Debug.WriteLine("Stop Button Just Clicked!!!");
            }
        }
    }
}
