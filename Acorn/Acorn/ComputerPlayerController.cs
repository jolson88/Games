using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Microsoft.Xna.Framework;

namespace Acorn
{
    public class ComputerPlayerController : IPlayerController
    {
        private Random _random;
        private ProcessManager _processManager;
        private MessageManager _messageManager;
        private int _playerIndex;
        private int _currentPlayer;
        private int _currentCardIndex;
        private int _currentScore;
        private int _tempAcorns;

        public ComputerPlayerController(int playerIndex, MessageManager messageManager)
        {
            _random = new Random();
            _playerIndex = playerIndex;
            _messageManager = messageManager;
            _processManager = new ProcessManager();

            _messageManager.AddListener<StartTurnMessage>(OnStartTurn);
            _messageManager.AddListener<CardSelectedMessage>(OnCardSelected);
            _messageManager.AddListener<ScoreChangedMessage>(OnScoreChanged);
        }

        public void Update(GameTime gameTime)
        {
            _processManager.Update(gameTime);
        }

        private void OnStartTurn(StartTurnMessage msg)
        {
            _tempAcorns = 0;
            _currentCardIndex = 0;
            _currentPlayer = msg.PlayerIndex;

            if (_currentPlayer == _playerIndex)
            {
                _processManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(1.5), new ActionProcess(() => MakeNextChoice())));
            }
        }

        private void OnCardSelected(CardSelectedMessage msg)
        {
            if (msg.PlayerIndex == _playerIndex)
            {
                _currentCardIndex = (_currentCardIndex >= 3) ? 0 : _currentCardIndex + 1;

                if (msg.CardValue > 0)
                {
                    _tempAcorns += msg.CardValue;
                    _processManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(1.5), new ActionProcess(() => MakeNextChoice())));
                }
            }
        }

        private void OnScoreChanged(ScoreChangedMessage msg)
        {
            if (msg.PlayerIndex == _playerIndex)
            {
                _currentScore = msg.Score;
            }
        }

        private void MakeNextChoice()
        {
            if (_currentScore + _tempAcorns >= 8)
            {
                _messageManager.QueueMessage(new StopRequestMessage(_playerIndex));
            }

            // For now, this computer is dumb, just a 50-50 chance to continuing
            var rand = _random.NextDouble();
            if (rand > 0.5 && _currentCardIndex > 0)
            {
                _messageManager.QueueMessage(new StopRequestMessage(_playerIndex));
            }
            else
            {
                SelectCard(_currentCardIndex);
            }
        }

        private void SelectCard(int cardIndex)
        {
            _messageManager.QueueMessage(new CardSelectionRequestMessage(_playerIndex, _currentCardIndex));
        }
    }
}
