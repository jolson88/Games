using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;
using Hiromi.Components;
using Hiromi.Messaging;
using Hiromi.Systems;
using Acorn.Components;

namespace Acorn.Systems
{
    public class PlayerControlSystem : GameSystem
    {
        private int? _currentPlayer;
        private int _playerIndex;
        private GameObject _stopButton;
        private List<GameObject> _cards;
        private Dictionary<int, Texture2D> _cardSprites;

        public PlayerControlSystem(int playerIndex)
        {
            _playerIndex = playerIndex;
            _cards = new List<GameObject>();

            _cardSprites = new Dictionary<int, Texture2D>();
            _cardSprites.Add(0, ContentService.Instance.GetAsset<Texture2D>(AcornAssets.CardZero));
            _cardSprites.Add(1, ContentService.Instance.GetAsset<Texture2D>(AcornAssets.CardOne));
            _cardSprites.Add(2, ContentService.Instance.GetAsset<Texture2D>(AcornAssets.CardTwo));

            MessageService.Instance.AddListener<GameObjectLoadedMessage>(msg => OnGameObjectLoaded((GameObjectLoadedMessage)msg));
            MessageService.Instance.AddListener<StartTurnMessage>(msg => OnStartTurn((StartTurnMessage)msg));
            MessageService.Instance.AddListener<ButtonPressMessage>(msg => OnButtonPress((ButtonPressMessage)msg));
            MessageService.Instance.AddListener<CardSelectedMessage>(msg => OnCardSelected((CardSelectedMessage)msg));
            MessageService.Instance.AddListener<CardsShuffledMessage>(msg => OnCardsShuffled((CardsShuffledMessage)msg));
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

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (_currentPlayer == _playerIndex)
            {
                if (msg.GameObjectId == _stopButton.Id)
                {
                    MessageService.Instance.QueueMessage(new StopRequestMessage(_playerIndex));
                }
                else if (_cards.Where(go => go.Id == msg.GameObjectId).Count() > 0)
                {
                    var card = _cards.Where(go => go.Id == msg.GameObjectId).First().GetComponent<CardComponent>();
                    MessageService.Instance.QueueMessage(new CardSelectionRequestMessage(_playerIndex, card.CardIndex));
                }
            }
        }

        private void OnCardSelected(CardSelectedMessage msg)
        {
            if (_currentPlayer == _playerIndex)
            {
                var obj = _cards.Where(go => go.GetComponent<CardComponent>().CardIndex == msg.CardIndex).First();
                var spriteComponent = obj.GetComponent<SpriteComponent>();
                spriteComponent.Texture = _cardSprites[msg.CardValue];
            }
        }

        private void OnCardsShuffled(CardsShuffledMessage msg)
        {
            if (_currentPlayer == _playerIndex)
            {
                foreach (var obj in _cards)
                {
                    var spriteComponent = obj.GetComponent<SpriteComponent>();
                    spriteComponent.Texture = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.CardBack);
                }
            }
        }
    }
}
