using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acorn.Hiromi;
using Acorn.Hiromi.Messaging;

namespace Acorn.Objects
{
    public class StopButton : GameObject
    {
        public Sprite NormalSprite { get; set; }
        public Sprite HoverSprite { get; set; }

        protected override void OnInitialize()
        {
            MessageService.Instance.AddListener<PointerEnterMessage>(msg => OnPointerEnter((PointerEnterMessage)msg));
            MessageService.Instance.AddListener<PointerExitMessage>(msg => OnPointerExit((PointerExitMessage)msg));
            MessageService.Instance.AddListener<PointerPressMessage>(msg => OnPointerPress((PointerPressMessage)msg));
        }

        private void OnPointerEnter(PointerEnterMessage msg)
        {
            if (msg.GameObjectId == this.Id)
            {
                this.Sprite = this.HoverSprite;
            }
        }

        private void OnPointerExit(PointerExitMessage msg)
        {
            if (msg.GameObjectId == this.Id)
            {
                this.Sprite = this.NormalSprite;
            }
        }

        private void OnPointerPress(PointerPressMessage msg)
        {
            if (msg.GameObjectId == this.Id)
            {
                // TODO: Add game logic later.
                System.Diagnostics.Debug.WriteLine("Stop Button Clicked");
            }
        }
    }
}
