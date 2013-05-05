using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Acorn.States;

namespace Acorn
{
    public class AcornGameLogic
    {
        private MessageManager _messageManager;
        private ProcessManager _processManager;
        private Random _random;
        private int[] _scores;
        private int _currentPlayer;
        private int? _winningPlayer;
        private int _winningPoints;
        private int _runningPoints;
        private Dictionary<int, int?> _cardValues;

        public AcornGameLogic(MessageManager messageManager, ProcessManager processManager, int cardCount, int winningPoints)
        {
            _messageManager = messageManager;
            _processManager = processManager;
            _random = new Random();
            _scores = new int[] { 0, 0 };
            _currentPlayer = 0;
            _winningPoints = winningPoints;
            _cardValues = new Dictionary<int, int?>();
            for (int i = 0; i < cardCount; i++) { _cardValues.Add(i, null); }

            _messageManager.AddListener<GameStartedMessage>(OnGameStarted);
            _messageManager.AddListener<CardSelectionRequestMessage>(OnCardSelectionRequest);
            _messageManager.AddListener<StopRequestMessage>(OnStopRequest);
            _messageManager.AddListener<GameOverMessage>(OnGameOver);
            _messageManager.AddListener<EndTurnConfirmationMessage>(OnEndTurnConfirmation);
            _messageManager.AddListener<CardShuffleRequestMessage>(OnCardShuffleRequested);
        }

        private void OnGameStarted(GameStartedMessage msg)
        {
            _messageManager.QueueMessage(new StartTurnMessage(_currentPlayer));
        }

        private void OnGameOver(GameOverMessage msg)
        {
            _messageManager.QueueMessage(new RequestChangeStateMessage(new GameOverState(msg.WinningPlayerIndex)));
        }

        private void OnCardSelectionRequest(CardSelectionRequestMessage msg)
        {
            if (msg.PlayerIndex == _currentPlayer && !CardHasBeenSelected(msg.CardIndex))
            {
                var cardValue = GetNextRandomCardValue();
                SelectCard(msg.CardIndex, cardValue);
                if (cardValue == 0)
                {
                    EndPlayerTurn(EndTurnReason.LostPoints);
                }
            }
        }

        private void OnStopRequest(StopRequestMessage msg)
        {
            if (msg.PlayerIndex == _currentPlayer)
            {
                EndPlayerTurn(EndTurnReason.WonPoints);
            }
        }

        private void OnCardShuffleRequested(CardShuffleRequestMessage msg)
        {
            if (AllCardsAreSelected() && msg.PlayerIndex == _currentPlayer)
            {
                ShuffleCards();
            }
        }

        private void OnEndTurnConfirmation(EndTurnConfirmationMessage msg)
        {
            if (msg.PlayerIndex == _currentPlayer)
            {
                if (_winningPlayer.HasValue)
                {
                    _messageManager.QueueMessage(new GameOverMessage(_currentPlayer));
                }
                else
                {
                    var nextPlayer = (_currentPlayer == 0) ? 1 : 0;

                    _currentPlayer = nextPlayer;
                    _runningPoints = 0;

                    ShuffleCards();
                    _messageManager.QueueMessage(new StartTurnMessage(_currentPlayer));
                }
            }
        }

        private void EndPlayerTurn(EndTurnReason reason)
        {
            if (reason == EndTurnReason.LostPoints)
            {
                _messageManager.QueueMessage(new EndTurnMessage(_currentPlayer, reason));
            }
            if (reason == EndTurnReason.WonPoints)
            {
                _scores[_currentPlayer] += _runningPoints;
                if (_scores[_currentPlayer] >= _winningPoints)
                {
                    _winningPlayer = _currentPlayer;
                    reason = EndTurnReason.WonGame;
                }

                _messageManager.QueueMessage(new EndTurnMessage(_currentPlayer, reason));
                _messageManager.QueueMessage(new ScoreChangedMessage(_currentPlayer, _scores[_currentPlayer]));
            }
        }

        private void SelectCard(int cardIndex, int cardValue)
        {
            _cardValues[cardIndex] = cardValue;
            _runningPoints += cardValue;
            _messageManager.QueueMessage(new CardSelectedMessage(cardIndex, cardValue, _currentPlayer));
        }

        private void ShuffleCards()
        {
            foreach (var k in _cardValues.Keys.ToList())
            {
                _cardValues[k] = null;
            }
            _messageManager.QueueMessage(new CardsShuffledMessage());
        }

        private int GetNextRandomCardValue()
        {
            var r = _random.Next(100);
            if (r > 75)
            {
                return 2;
            }
            else if (r > 50)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        private bool CardHasBeenSelected(int cardIndex)
        {
            return _cardValues[cardIndex].HasValue;
        }

        private bool AllCardsAreSelected()
        {
            return _cardValues.Values.Where(v => v.HasValue).Count() == _cardValues.Count;
        }
    }
}
