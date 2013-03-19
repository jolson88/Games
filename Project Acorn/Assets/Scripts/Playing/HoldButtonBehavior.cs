using UnityEngine;
using System.Collections;

public class HoldButtonBehavior : MonoBehaviour
{
    private MessageBus m_messageBus;
    
	// Use this for initialization
	void Start ()
	{
        var go = GameObject.Find("MessageBus");
        m_messageBus = go.GetComponent<MessageBus>();
	}

	// Update is called once per frame
	void Update ()
	{

	}
    
    void OnMouseDown () 
    {
        m_messageBus.QueueMessage(new HoldMessage());
    }
}
