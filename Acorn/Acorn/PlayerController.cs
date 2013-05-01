﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Acorn.Components;

namespace Acorn
{
    public class PlayerController
    {
        private MessageManager _messageManager;
        private int _playerIndex;
        private int? _currentPlayer;
        private GameObject _stopButton;
        private List<GameObject> _cards;

        public PlayerController(int playerIndex, MessageManager messageManager)
        {
            _cards = new List<GameObject>();
            _playerIndex = playerIndex;

            _messageManager = messageManager;
            _messageManager.AddListener<NewGameObjectMessage>(OnNewGameObject);
            _messageManager.AddListener<StartTurnMessage>(OnStartTurn);
            _messageManager.AddListener<PointerPressMessage>(OnPointerPress);
        }

        public void Update(GameTime gameTime) { }

        private void OnNewGameObject(NewGameObjectMessage msg)
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
                    _messageManager.QueueMessage(new StopRequestMessage(_playerIndex));
                }
                else if (_cards.Where(go => go.Id == msg.GameObjectId).Count() > 0)
                {
                    var card = _cards.Where(go => go.Id == msg.GameObjectId).First().GetComponent<CardComponent>();
                    _messageManager.QueueMessage(new CardSelectionRequestMessage(_playerIndex, card.CardIndex));
                }
            }
        }
    }
}