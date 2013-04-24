using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Acorn.Components
{
    public class PlayerControllerComponent : GameObjectComponent
    {
        private int _playerIndex;
        private int? _currentPlayer;
        private GameObject _stopButton;
        private List<GameObject> _cards;

        public PlayerControllerComponent(int playerIndex)
        {
            _cards = new List<GameObject>();
            _playerIndex = playerIndex;
        }

        public override void Loaded()
        {
            this.GameObject.MessageManager.AddListener<GameObjectLoadedMessage>(msg => OnGameObjectLoaded((GameObjectLoadedMessage)msg));
            this.GameObject.MessageManager.AddListener<StartTurnMessage>(msg => OnStartTurn((StartTurnMessage)msg));
            this.GameObject.MessageManager.AddListener<PointerPressMessage>(msg => OnPointerPress((PointerPressMessage)msg));
        }

        private void OnGameObjectLoaded(GameObjectLoadedMessage msg)
        {
            if (msg.GameObject.Tag.Equals("StopButton"))
            {
                _stopButton = msg.GameObject;
            }
            else if (msg.GameObject.HasComponent<CardComponent>())
            {
                _cards.Add(msg.GameObject);
            }
        }

        private void OnStartTurn(StartTurnMessage msg)
        {
            _currentPlayer = msg.PlayerIndex;
        }

        private void OnPointerPress(PointerPressMessage msg)
        {
            if (_currentPlayer == _playerIndex)
            {
                if (msg.GameObjectId == _stopButton.Id)
                {
                    this.GameObject.MessageManager.QueueMessage(new StopRequestMessage(_playerIndex));
                }
                else if (_cards.Where(go => go.Id == msg.GameObjectId).Count() > 0)
                {
                    var card = _cards.Where(go => go.Id == msg.GameObjectId).First().GetComponent<CardComponent>();
                    this.GameObject.MessageManager.QueueMessage(new CardSelectionRequestMessage(_playerIndex, card.CardIndex));
                }
            }
        }
    }
}
