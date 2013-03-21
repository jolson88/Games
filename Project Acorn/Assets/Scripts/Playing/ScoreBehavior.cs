using UnityEngine;
using System.Collections;

public class ScoreBehavior : MonoBehaviour
{
	public int PlayerIndex;

    private MessageBus m_messageBus;
	private GUIText m_text;

	// Use this for initialization
	void Start ()
	{
        var go = GameObject.Find("MessageBus");
        m_messageBus = go.GetComponent<MessageBus>();
        m_messageBus.AddListener<PointsScoredMessage>(msg => OnPointsScored((PointsScoredMessage)msg));
        
		m_text = this.GetComponent<GUIText> ();
        UpdateScore(0);
	}

    void OnPointsScored(PointsScoredMessage msg)
    {
        if (msg.Player == this.PlayerIndex)
        {
            UpdateScore(msg.TotalPoints);
        }
    }
    
    void UpdateScore(int score)
    {
        m_text.text = string.Format ("P{0} Score: {1}", PlayerIndex + 1, score);
    }
}
