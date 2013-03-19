using UnityEngine;
using System.Collections.Generic;

class GameStartedMessage : Message { }

class HoldMessage : Message { }

class SelectCardMessage : Message
{
    public int CardIndex { get; set; }
}

class CardSelectedMessage : Message
{
    public int CardIndex { get; set; }
    public int PointValue { get; set; }
}

class CardsShuffledMessage : Message { }

class PointsScoredMessage : Message
{
    public int Player { get; set; }
    public int TotalPoints { get; set; }
}

enum TurnOverReason { Hold, ZeroCard }
class TurnOverMessage : Message 
{
    public int CurrentPlayer { get; set; }
    public int NextPlayer { get; set; } 
    public TurnOverReason Reason { get; set; }
}

enum GameState
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
    public int VisibleCardCount;
    
    // TODO: Turn private once messages are fully implemented
    public List<int> PlayerScores;

    private MessageBus m_messageBus;
    private GameContext m_context;
    private GameState m_state;
    private Dictionary<int, CardBehavior> m_cards; // TODO: Get rid of m_cards once message-driven
    private int m_selectedCount;
    private int m_currentPlayer;
    private PlayerMessageBehavior m_message; // TODO: Get rid of m_message once message-driven
    private int m_runningPoints;

    // Use this for initialization
    void Start()
    {   
        var pMsg = GameObject.Find("PlayerMessage");
        m_message = pMsg.GetComponent<PlayerMessageBehavior>();

        m_cards = new Dictionary<int, CardBehavior>();
        PlayerScores = new List<int>();
        
        // Start with two players
        PlayerScores.Add(0);
        PlayerScores.Add(0);
        
        // Find all the cards
        foreach (var obj in GameObject.FindGameObjectsWithTag("Card"))
        {
            var card = obj.GetComponent<CardBehavior>();
            m_cards.Add(card.CardIndex, card);
        }

        // What's the context?
        var go = new GameObject("GameContext");
        go.AddComponent<GameContext>();
        m_context = go.GetComponent<GameContext>();
        
        // Start the message processing yo!
        go = GameObject.Find("MessageBus");
        m_messageBus = go.GetComponent<MessageBus>();
        
        // What shall we do? 
        m_messageBus.AddListener<SelectCardMessage>(msg => this.CardSelected((SelectCardMessage)msg));
        m_messageBus.AddListener<HoldMessage>(msg => this.Hold());
        
        m_state = GameState.Initialized;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (m_state == GameState.Initialized)
        {
            m_message.DisplayMessage(string.Format("Player {0}'s Turn", m_currentPlayer + 1));
            ReshuffleCards();
            
            m_messageBus.QueueMessage(new GameStartedMessage());
            m_state = GameState.Playing;
        }
    }
    
    void Hold()
    {
        if (m_state == GameState.Playing)
        {
            // Increment score
            PlayerScores [m_currentPlayer] += m_runningPoints;
            
            if (PlayerScores [m_currentPlayer] >= WinningPointTotal)
            {
                m_state = GameState.GameOver;
                m_context.WinningPlayer = m_currentPlayer;
                
                DontDestroyOnLoad(m_context.gameObject);
                Application.LoadLevel("game_over");
            } else
            {
                EndTurn();
            }
        }
    }
 
    void CardSelected(SelectCardMessage msg)
    {
        if (m_state == GameState.Playing)
        {
            var points = m_cards[msg.CardIndex].CardValue;
            m_selectedCount++;
            
            if (points == 0)
            {
                EndTurn();
            } else
            {
                m_runningPoints += points;
                if (m_selectedCount == m_cards.Values.Count)
                {
                    m_selectedCount = 0;
                    ReshuffleCards();
                }
            }
        }
    }
       
    void EndTurn()
    {   
        ReshuffleCards();
        
        m_selectedCount = 0;
        m_currentPlayer = (m_currentPlayer + 1) % PlayerScores.Count;
        m_runningPoints = 0;

        m_message.DisplayMessage(string.Format("Player {0}'s Turn", m_currentPlayer + 1));
    }
    
    void ReshuffleCards()
    {
        // TODO: Get rid of this method once m_cards is elimated from class
        
        ShuffleCardValues();
        foreach (var card in m_cards.Values)
        {
            card.Reset();
        }
    }
    
    void ShuffleCardValues()
    {
        foreach (var card in m_cards.Values)
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
        } else if (rand < SingleValuePercentage + DoubleValuePercentage)
        {
            return 2;
        } else
        {
            return 0;
        }
    }
}
