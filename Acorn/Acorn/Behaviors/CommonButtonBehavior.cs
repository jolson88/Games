using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acorn.Hiromi;
using Acorn.Hiromi.Messaging;

namespace Acorn.Behaviors
{
    public class CommonButtonBehavior : BehaviorComponent
    {
        public Sprite FocusSprite { get; set; }
        public Sprite NonFocusSprite { get; set; }

        public CommonButtonBehavior()
        {
            MessageService.Instance.AddListener<PointerExitMessage>(msg => OnPointerExit((PointerExitMessage)msg));
            MessageService.Instance.AddListener<PointerPressMessage>(msg => OnPointerPress((PointerPressMessage)msg));
            MessageService.Instance.AddListener<PointerReleaseMessage>(msg => OnPointerRelease((PointerReleaseMessage)msg));
        }

        private void OnPointerExit(PointerExitMessage msg)
        {
            if (msg.GameObjectId == this.GameObject.Id)
            {
                this.GameObject.Sprite = this.NonFocusSprite;
            }
        }

        private void OnPointerPress(PointerPressMessage msg)
        {
            if (msg.GameObjectId == this.GameObject.Id)
            {
                this.GameObject.Sprite = this.FocusSprite;
                MessageService.Instance.TriggerMessage(new ButtonPressMessage(this.GameObject.Id));
            }
        }

        private void OnPointerRelease(PointerReleaseMessage msg)
        {
            if (msg.GameObjectId == this.GameObject.Id)
            {
                this.GameObject.Sprite = this.NonFocusSprite;
            }
        }
    }
}
