using UnityEngine;
using System.Collections;

public class PlayerMessageBehavior : MonoBehaviour
{
    public float SecondsToDisplayMessages;
    
    private ProcessManager m_processes;
    private MessageBus m_messageBus;
	private GUIText m_text;

	// Use this for initialization
	void Start ()
	{
		m_text = this.GetComponent<GUIText> ();
		m_text.text = string.Empty;
        
        m_processes = this.GetComponent<ProcessManager>();
        
        var go = GameObject.Find("MessageBus");
        m_messageBus = go.GetComponent<MessageBus>();
        m_messageBus.AddListener<GameStartedMessage>(msg => OnGameStarted((GameStartedMessage)msg));
        m_messageBus.AddListener<TurnOverMessage>(msg => OnTurnOver((TurnOverMessage)msg));    
	}

    void OnGameStarted(GameStartedMessage msg)
    {
        DisplayMessage(string.Format("Player {0} Goes First!", msg.StartingPlayer + 1));
    }
    
    void OnTurnOver(TurnOverMessage msg)
    {
        if (msg.Reason == TurnOverReason.ZeroCard)
        {
            DisplayMessage(string.Format("Player {0} Drops the Acorns!\n", msg.CurrentPlayer + 1));
            m_processes.AttachProcess(new DelayProcess(SecondsToDisplayMessages, new ActionProcess(() => {
                DisplayMessage(string.Format("Player {0}'s Turn", msg.NextPlayer + 1));
            })));
        } else
        {
            DisplayMessage(string.Format("Player {0}'s Turn", msg.NextPlayer + 1));
        }
    }
    
    void DisplayMessage(string msg)
    {
        m_text.text = msg;
        m_processes.AttachProcess(new DelayProcess(SecondsToDisplayMessages, new ActionProcess(() => m_text.text = "")), true);
    }
}
