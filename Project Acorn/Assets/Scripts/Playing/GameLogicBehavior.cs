using UnityEngine;
using System.Collections.Generic;

class GameStartedMessage : Message
{
    public GameStartedMessage(int startingPlayer)
    {
        this.StartingPlayer = startingPlayer;
    }
    
    public int StartingPlayer { get; set; } 
}

class HoldMessage : Message
{
}

class SelectCardMessage : Message
{
    public int CardIndex { get; set; }
}

class CardSelectedMessage : Message
{
    public CardSelectedMessage(int cardIndex, int pointValue)
    {
        this.CardIndex = cardIndex;
        this.PointValue = pointValue;
    }
    
    public int CardIndex { get; set; }

    public int PointValue { get; set; }
}

class CardsShuffledMessage : Message
{
}

class PointsScoredMessage : Message
{
    public PointsScoredMessage(int player, int totalPoints)
    {
        this.Player = player;
        this.TotalPoints = totalPoints;
    }
    
    public int Player { get; set; }

    public int TotalPoints { get; set; }
}

enum TurnOverReason
{
    Hold,
    ZeroCard
}

class TurnOverMessage : Message
{
    public TurnOverMessage(int currentPlayer, int nextPlayer, TurnOverReason reason)
    {
        this.CurrentPlayer = currentPlayer;
        this.NextPlayer = nextPlayer;
        this.Reason = reason;
    }
    
    public int CurrentPlayer { get; set; }

    public int NextPlayer { get; set; }

    public TurnOverReason Reason { get; set; }
}

public class GameLogicBehavior : MonoBehaviour
{
    public int SingleValuePercentage;
    public int DoubleValuePercentage;
    public int ZeroValuePercentage;
    public int WinningPointTotal;
    public int VisibleCardCount;
    private int[] m_playerScores;
    private ProcessManager m_processes;
    private MessageBus m_messageBus;
    private GameContext m_context;
    private int[] m_cardValues;
    private int m_selectedCount;
    private int m_currentPlayer;
    private int m_runningPoints;
    private bool m_turnIsOver;
    
    // Use this for initialization
    void Start()
    {   
        m_playerScores = new int[] { 0, 0 };
        m_cardValues = new int[] { 0, 0, 0, 0 };
        
        // What's the context?
        var go = new GameObject("GameContext");
        go.AddComponent<GameContext>();
        m_context = go.GetComponent<GameContext>();
        
        m_processes = this.GetComponent<ProcessManager>();
        
        // Start the message processing yo!
        go = GameObject.Find("MessageBus");
        m_messageBus = go.GetComponent<MessageBus>();
        
        // What shall we do? 
        m_messageBus.AddListener<SelectCardMessage>(msg => this.OnCardSelected((SelectCardMessage)msg));
        m_messageBus.AddListener<HoldMessage>(msg => this.OnHold());

        ShuffleCards();        
        m_messageBus.QueueMessage(new GameStartedMessage(0));
    }
    
    void OnHold()
    {
        if (!m_turnIsOver)
        {
            // Increment score
            m_playerScores [m_currentPlayer] += m_runningPoints;
            m_messageBus.QueueMessage(new PointsScoredMessage(m_currentPlayer, m_playerScores [m_currentPlayer]));
            
            if (m_playerScores [m_currentPlayer] >= WinningPointTotal)
            {
                m_context.WinningPlayer = m_currentPlayer;
                
                DontDestroyOnLoad(m_context.gameObject);
                Application.LoadLevel("game_over");
            } else
            {
                EndTurn(TurnOverReason.Hold);
            }
        }
    }
 
    void OnCardSelected(SelectCardMessage msg)
    {
        if (!m_turnIsOver)
        {
            var points = m_cardValues [msg.CardIndex];
            m_messageBus.QueueMessage(new CardSelectedMessage(msg.CardIndex, points));
            
            m_selectedCount++;
            if (points == 0)
            {
                EndTurn(TurnOverReason.ZeroCard);
            } else
            {
                m_runningPoints += points;
                if (m_selectedCount == m_cardValues.Length)
                {
                    m_processes.AttachProcess(new DelayProcess(1f, new ActionProcess(() => {
                        ShuffleCards();
                    })));
                }
            }
        }
    }
       
    void EndTurn(TurnOverReason reason)
    {  
        m_turnIsOver = true;
        
        // If it was because of a zero card, give enough time for players to see zero card
        float delayTime = 0f;
        if (reason == TurnOverReason.ZeroCard) delayTime = 2f;
        
        m_processes.AttachProcess(new DelayProcess(delayTime, new ActionProcess(() => {
            ShuffleCards();
            m_turnIsOver = false;
        })));
        
        m_selectedCount = 0;
        m_runningPoints = 0;
        
        var previousPlayer = m_currentPlayer;
        m_currentPlayer = (m_currentPlayer + 1) % m_playerScores.Length;
        
        m_messageBus.QueueMessage(new TurnOverMessage(previousPlayer, m_currentPlayer, reason));
    }
    
    void ShuffleCards()
    {
        m_selectedCount = 0;
        for (int i = 0; i < m_cardValues.Length; i++)
        {
            m_cardValues [i] = GetRandomCardValue();
        }
        
        m_messageBus.QueueMessage(new CardsShuffledMessage());
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
