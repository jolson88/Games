using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acorn.Hiromi;
using Acorn.Hiromi.Messaging;
using Acorn.Behaviors;

namespace Acorn.Objects
{
    public class StopButton : GameObject
    {
        public Sprite NonFocusSprite { set { _buttonBehavior.NonFocusSprite = value; } }
        public Sprite FocusSprite { set { _buttonBehavior.FocusSprite = value; } }

        private CommonButtonBehavior _buttonBehavior;

        protected override void OnInitialize()
        {
            _buttonBehavior = new CommonButtonBehavior();
            this.AddComponent(_buttonBehavior);

            MessageService.Instance.AddListener<ButtonPressMessage>(msg => OnButtonPress((ButtonPressMessage)msg));
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (msg.GameObjectId == this.Id)
            {
                // TODO: Add game logic later.
                System.Diagnostics.Debug.WriteLine("Stop Button Clicked");
            }
        }
    }
}
