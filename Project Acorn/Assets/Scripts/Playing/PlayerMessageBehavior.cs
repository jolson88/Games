using UnityEngine;
using System.Collections;

public class PlayerMessageBehavior : MonoBehaviour
{
    private MessageBus m_messageBus;
	private GUIText m_text;

	// Use this for initialization
	void Start ()
	{
		m_text = this.GetComponent<GUIText> ();
		m_text.text = string.Empty;
        
        var go = GameObject.Find("MessageBus");
        m_messageBus = go.GetComponent<MessageBus>();
        m_messageBus.AddListener<GameStartedMessage>(msg => OnGameStarted((GameStartedMessage)msg));
        m_messageBus.AddListener<TurnOverMessage>(msg => OnTurnOver((TurnOverMessage)msg));    
	}

    void OnGameStarted(GameStartedMessage msg)
    {
        m_text.text = string.Format("Player {0} Goes First!", msg.StartingPlayer + 1);
    }
    
    void OnTurnOver(TurnOverMessage msg)
    {
        m_text.text = string.Format("Player {0}'s Turn", msg.NextPlayer + 1);
    }
}
