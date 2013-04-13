using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acorn.Hiromi;
using Acorn.Hiromi.Messaging;

namespace Acorn.Behaviors
{
    public class PlayerControllerBehavior : GameObjectBehavior
    {
        public int PlayerIndex { get; set; }

        private GameObject _stopButton;
        private Dictionary<int, GameObject> _cards;

        public PlayerControllerBehavior(int playerIndex)
        {
            this.PlayerIndex = playerIndex;

            _stopButton = GameObjectService.Instance.GetAllGameObjectsWithTag("StopButton").First();
            _cards = GameObjectService.Instance.GetAllGameObjectsWithTag("Card").ToDictionary(obj => obj.Id);

            MessageService.Instance.AddListener<ButtonPressMessage>(msg => OnButtonPress((ButtonPressMessage)msg));
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (msg.GameObjectId == _stopButton.Id)
            {
                System.Diagnostics.Debug.WriteLine("Stop button clicked");
            }
            else if (_cards.ContainsKey(msg.GameObjectId))
            {
                var card = _cards[msg.GameObjectId].GetBehavior<CardBehavior>();
                System.Diagnostics.Debug.WriteLine(string.Format("Card {0} clicked", card.CardIndex));
            }
        }
    }
}
