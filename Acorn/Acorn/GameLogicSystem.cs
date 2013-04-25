using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Acorn.Screens;

namespace Acorn
{
    public class GameLogicSystem
    {
        private MessageManager _messageManager;
        private ProcessManager _processManager;
        private Random _random;
        private int[] _scores;
        private int _currentPlayer;
        private int _winningPoints;
        private int _runningPoints;
        private Dictionary<int, int?> _cardValues;

        public GameLogicSystem(MessageManager messageManager, ProcessManager processManager, int cardCount, int winningPoints)
        {
            _messageManager = messageManager;
            _processManager = processManager;
            _random = new Random();
            _scores = new int[] { 0, 0 };
            _currentPlayer = 0;
            _winningPoints = winningPoints;
            _cardValues = new Dictionary<int, int?>();
            for (int i = 0; i < cardCount; i++) { _cardValues.Add(i, null); }

            _messageManager.AddListener<GameStartedMessage>(msg => OnGameStarted((GameStartedMessage)msg));
            _messageManager.AddListener<CardSelectionRequestMessage>(msg => OnCardSelectionRequest((CardSelectionRequestMessage)msg));
            _messageManager.AddListener<StopRequestMessage>(msg => OnStopRequest((StopRequestMessage)msg));
            _messageManager.AddListener<GameOverMessage>(msg => OnGameOver((GameOverMessage)msg));
        }

        private void OnGameStarted(GameStartedMessage msg)
        {
            _messageManager.QueueMessage(new StartTurnMessage(_currentPlayer));
        }

        private void OnGameOver(GameOverMessage msg)
        {
            _messageManager.QueueMessage(new RequestLoadScreenMessage(new GameOverScreen(msg.WinningPlayerIndex)));
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
                else if (AllCardsAreSelected())
                {
                    // Delay so player has time to see what card turned over
                    _processManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(2), new ActionProcess(() =>
                    {
                        ShuffleCards();
                    })));
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

        private void EndPlayerTurn(EndTurnReason reason)
        {
            var nextPlayer = (_currentPlayer == 0) ? 1 : 0;
            _messageManager.QueueMessage(new EndTurnMessage(_currentPlayer, reason));

            if (reason == EndTurnReason.WonPoints)
            {
                _scores[_currentPlayer] += _runningPoints;
                _messageManager.QueueMessage(new ScoreChangedMessage(_currentPlayer, _scores[_currentPlayer]));

                if (_scores[_currentPlayer] >= _winningPoints)
                {
                    _messageManager.QueueMessage(new GameOverMessage(_currentPlayer));
                    return;
                }
            }

            _currentPlayer = nextPlayer;
            _runningPoints = 0;

            // Delay for two seconds it wasn't a voluntary stop (so player has time to see zero card
            var delay = (reason == EndTurnReason.LostPoints) ? 2 : 0;
            _processManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(delay), new ActionProcess(() =>
            {
                ShuffleCards();
                _messageManager.QueueMessage(new StartTurnMessage(_currentPlayer));
            })));
        }

        private void SelectCard(int cardIndex, int cardValue)
        {
            _cardValues[cardIndex] = cardValue;
            _runningPoints += cardValue;
            _messageManager.QueueMessage(new CardSelectedMessage(cardIndex, cardValue));
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
