using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

class Message
{
}

class MessageBus : MonoBehaviour
{
    private Dictionary<Type, List<Action<Message>>> m_messageListeners;
    private List<Queue<Message>> m_messageQueues;
    private int m_currentMessageQueue;
   
    
    // Use this for initialization
    void Start()
    {
        m_messageListeners = new Dictionary<Type, List<Action<Message>>>();
        m_messageQueues = new List<Queue<Message>>() { new Queue<Message>(), new Queue<Message>() };        
    }
    
    // Update is called once per frame
    void Update()
    {
        var processQueue = m_currentMessageQueue;
        
        // Make sure any new messages coming in from processing these messages goes
        // to a different queue
        m_currentMessageQueue = (m_currentMessageQueue + 1) % m_messageQueues.Count;
        
        while (m_messageQueues[processQueue].Count > 0)
        {
            var msg = m_messageQueues [processQueue].Dequeue();
            ProcessMessage(msg);
        }        
    }

    public void AddListener<T>(Action<Message> listener) where T : Message
    {
        if (!m_messageListeners.Keys.Contains(typeof(T)))
        {
            m_messageListeners [typeof(T)] = new List<Action<Message>>();
        }
        
        m_messageListeners [typeof(T)].Add(listener);
    }

    public void QueueMessage(Message msg)
    {
        m_messageQueues [m_currentMessageQueue].Enqueue(msg);
    }
    
    public void TriggerMessage(Message msg)
    {
        // Immediately process
        ProcessMessage(msg);
    }
    
    private void ProcessMessage(Message msg)
    {
        if (m_messageListeners.Keys.Contains(msg.GetType()))
        {
            foreach (var listener in m_messageListeners[msg.GetType()])
            {
                listener(msg);
            }
        }
    }
}
