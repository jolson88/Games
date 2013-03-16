using UnityEngine;
using System.Collections.Generic;

public enum GameState
{
		Initialized,
		Playing,
		GameOver
}

public class GameLogicBehavior : MonoBehaviour
{
		public int SingleValuePercentage;
		public int DoubleValuePercentage;
		public int ZeroValuePercentage;
		public int WinningPointTotal;
		public List<int> PlayerScores;
		private GameState m_state;
		private Dictionary<int, CardBehavior> m_cards;
		private int m_selectedCount;
		private int m_currentPlayer;
		private PlayerMessageBehavior m_message;
		private int m_runningPoints;

		public void CardSelected (int cardIndex)
		{
				if (m_state == GameState.Playing) {
						var points = m_cards [cardIndex].CardValue;
						m_selectedCount++;
		
						if (points == 0) {
								EndTurn ();
						} else {
								m_runningPoints += points;
								if (m_selectedCount == m_cards.Values.Count) {
										m_selectedCount = 0;
										ReshuffleCards ();
								}
						}
				}
		}

		public void Hold ()
		{
				if (m_state == GameState.Playing) {
						// Increment score
						PlayerScores [m_currentPlayer] += m_runningPoints;

						if (PlayerScores [m_currentPlayer] >= WinningPointTotal) {
								m_state = GameState.GameOver;
								m_message.DisplayMessage (string.Format ("Player {0} Wins!!!", m_currentPlayer + 1));
						} else {
								EndTurn ();
						}
				}
		}

		// Use this for initialization
		void Start ()
		{	
				var msg = GameObject.Find ("PlayerMessage");
				m_message = msg.GetComponent<PlayerMessageBehavior> ();

				m_cards = new Dictionary<int, CardBehavior> ();
				PlayerScores = new List<int> ();
		
				// Start with two players
				PlayerScores.Add (0);
				PlayerScores.Add (0);
		
				// Find all the cards
				foreach (var go in GameObject.FindGameObjectsWithTag("Card")) {
						var card = go.GetComponent<CardBehavior> ();
						m_cards.Add (card.CardIndex, card);
				}

				m_state = GameState.Initialized;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (m_state == GameState.Initialized) {
						m_message.DisplayMessage (string.Format ("Player {0}'s Turn", m_currentPlayer + 1));
						ReshuffleCards ();
						m_state = GameState.Playing;
				}
		}
	
		void EndTurn ()
		{	
				ReshuffleCards ();
		
				m_selectedCount = 0;
				m_currentPlayer = (m_currentPlayer + 1) % PlayerScores.Count;
				m_runningPoints = 0;

				m_message.DisplayMessage (string.Format ("Player {0}'s Turn", m_currentPlayer + 1));
		}
	
		void ReshuffleCards ()
		{
				ShuffleCardValues ();
				foreach (var card in m_cards.Values) {
						card.Reset ();
				}
		}
	
		void ShuffleCardValues ()
		{
				foreach (var card in m_cards.Values) {
						card.CardValue = GetRandomCardValue ();
				}
		}
	
		int GetRandomCardValue ()
		{
				var rand = Random.Range (0, 100);
				if (rand < SingleValuePercentage) {
						return 1;
				} else if (rand < SingleValuePercentage + DoubleValuePercentage) {
						return 2;
				} else {
						return 0;
				}
		}
}
