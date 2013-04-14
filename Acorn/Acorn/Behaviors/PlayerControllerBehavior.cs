using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Messaging;

namespace Acorn.Behaviors
{
    public class PlayerControllerBehavior : GameObjectBehavior
    {
        public int PlayerIndex { get; set; }

        private int? _currentPlayer;
        private GameObject _stopButton;
        private Dictionary<int, GameObject> _cards;

        public PlayerControllerBehavior(int playerIndex)
        {
            this.PlayerIndex = playerIndex;
            _currentPlayer = null;
        }

        protected override void OnInitialize()
        {
            _stopButton = GameObjectService.Instance.GetAllGameObjectsWithTag("StopButton").First();
            _cards = GameObjectService.Instance.GetAllGameObjectsWithTag("Card").ToDictionary(obj => obj.Id);

            MessageService.Instance.AddListener<ButtonPressMessage>(msg => OnButtonPress((ButtonPressMessage)msg));
            MessageService.Instance.AddListener<StartTurnMessage>(msg => OnStartTurn((StartTurnMessage)msg));
        }

        private void OnStartTurn(StartTurnMessage msg)
        {
            _currentPlayer = msg.PlayerIndex;
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (_currentPlayer == this.PlayerIndex)
            {
                if (msg.GameObjectId == _stopButton.Id)
                {
                    MessageService.Instance.QueueMessage(new StopRequestMessage(this.PlayerIndex));
                }
                else if (_cards.ContainsKey(msg.GameObjectId))
                {
                    var card = _cards[msg.GameObjectId].GetBehavior<CardBehavior>();
                    MessageService.Instance.QueueMessage(new CardSelectionRequestMessage(this.PlayerIndex, card.CardIndex));
                }
            }
        }
    }
}
