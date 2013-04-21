using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Systems;
using Acorn.Screens;

namespace Acorn.Systems
{
    public class GameLogicSystem : GameSystem
    {
        private Random _random;
        private int[] _scores;
        private int _currentPlayer;
        private int _winningPoints;
        private int _runningPoints;
        private Dictionary<int, int?> _cardValues;

        public GameLogicSystem(int cardCount, int winningPoints)
        {
            _random = new Random();
            _scores = new int[] { 0, 0 };
            _currentPlayer = 0;
            _winningPoints = winningPoints;
            _cardValues = new Dictionary<int, int?>();
            for (int i = 0; i < cardCount; i++) { _cardValues.Add(i, null); }
        }

        protected override void OnInitialize()
        {
            this.MessageManager.AddListener<GameStartedMessage>(msg => OnGameStarted((GameStartedMessage)msg));
            this.MessageManager.AddListener<CardSelectionRequestMessage>(msg => OnCardSelectionRequest((CardSelectionRequestMessage)msg));
            this.MessageManager.AddListener<StopRequestMessage>(msg => OnStopRequest((StopRequestMessage)msg));
            this.MessageManager.AddListener<GameOverMessage>(msg => OnGameOver((GameOverMessage)msg));
        }

        private void OnGameStarted(GameStartedMessage msg)
        {
            this.MessageManager.QueueMessage(new StartTurnMessage(_currentPlayer));
        }

        private void OnGameOver(GameOverMessage msg)
        {
            this.MessageManager.QueueMessage(new RequestLoadScreenMessage(new GameOverScreen(msg.WinningPlayerIndex)));
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
                    this.ProcessManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(2), new ActionProcess(() =>
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
            this.MessageManager.QueueMessage(new EndTurnMessage(_currentPlayer, reason));

            if (reason == EndTurnReason.WonPoints)
            {
                _scores[_currentPlayer] += _runningPoints;
                this.MessageManager.QueueMessage(new ScoreChangedMessage(_currentPlayer, _scores[_currentPlayer]));

                if (_scores[_currentPlayer] >= _winningPoints)
                {
                    this.MessageManager.QueueMessage(new GameOverMessage(_currentPlayer));
                    return;
                }
            }

            _currentPlayer = nextPlayer;
            _runningPoints = 0;

            // Delay for two seconds it wasn't a voluntary stop (so player has time to see zero card
            var delay = (reason == EndTurnReason.LostPoints) ? 2 : 0;
            this.ProcessManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(delay), new ActionProcess(() =>
            {
                ShuffleCards();
                this.MessageManager.QueueMessage(new StartTurnMessage(_currentPlayer));
            })));
        }

        private void SelectCard(int cardIndex, int cardValue)
        {
            _cardValues[cardIndex] = cardValue;
            _runningPoints += cardValue;
            this.MessageManager.QueueMessage(new CardSelectedMessage(cardIndex, cardValue));
        }

        private void ShuffleCards()
        {
            foreach (var k in _cardValues.Keys.ToList())
            {
                _cardValues[k] = null;
            }
            this.MessageManager.QueueMessage(new CardsShuffledMessage());
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
