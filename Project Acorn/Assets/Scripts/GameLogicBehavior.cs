using UnityEngine;
using System.Collections.Generic;

public class GameLogicBehavior : MonoBehaviour
{
	public int SingleValuePercentage;
	public int DoubleValuePercentage;
	public int ZeroValuePercentage;
	
	private Dictionary<int, CardBehavior> m_cards;
	private int m_selectedCount;
	private List<int> m_playerScores;
	private int m_currentPlayer;
	
	public void CardSelected(int cardIndex)
	{
		var points = m_cards[cardIndex].CardValue;
		m_selectedCount++;
		
		if (points == 0)
		{
			EndTurn();
		}
		else
		{
			m_playerScores[m_currentPlayer] += points;
			print (string.Format("Player {0} has {1} points", m_currentPlayer + 1, m_playerScores[m_currentPlayer]));
			
			if (m_selectedCount == m_cards.Values.Count)
			{
				m_selectedCount = 0;
				ReshuffleCards();
			}
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		m_cards = new Dictionary<int, CardBehavior>();
		m_playerScores = new List<int>();
		
		// Start with two players
		m_playerScores.Add(0);
		m_playerScores.Add(0);
		
		// Find all the cards
		foreach(var go in GameObject.FindGameObjectsWithTag("Card"))
		{
			var card = go.GetComponent<CardBehavior>();
			m_cards.Add(card.CardIndex, card);
		}
		
		ReshuffleCards();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void EndTurn()
	{	
		ReshuffleCards();
		
		m_selectedCount = 0;
		m_currentPlayer = (m_currentPlayer + 1) % m_playerScores.Count;
		print (string.Format("Player {0}'s Turn", m_currentPlayer + 1));
	}
	
	void ReshuffleCards()
	{
		ShuffleCardValues();
		foreach(var card in m_cards.Values)
		{
			card.Reset();
		}
	}
	
	void ShuffleCardValues()
	{
		foreach(var card in m_cards.Values)
		{
			card.CardValue = GetRandomCardValue();
		}
	}
	
	int GetRandomCardValue()
	{
		var rand = Random.Range(0, 100);
		if (rand < SingleValuePercentage)
		{
			return 1;
		}
		else if (rand < SingleValuePercentage + DoubleValuePercentage)
		{
			return 2;
		}
		else
		{
			return 0;
		}
	}
}
