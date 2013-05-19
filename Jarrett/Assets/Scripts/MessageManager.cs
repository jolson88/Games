using UnityEngine;
using System;
using System.Collections.Generic;

public class MessageManager : MonoBehaviour 
{
	private Dictionary<Type, List<object>> _messageListeners;
	private Dictionary<Type, Action<Message>> _messageSenders;
	private List<Queue<Message>> _messageQueues;
	private int _currentMessageQueue;

	void Awake ()
	{
	    _messageListeners = new Dictionary<Type, List<object>>();
	    _messageSenders = new Dictionary<Type, Action<Message>>();
	    _messageQueues = new List<Queue<Message>>() { new Queue<Message>(), new Queue<Message>() };
	}
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	    var processQueue = _currentMessageQueue;
	
	    // Make sure any new messages coming in from processing these messages goes
	    // to a different queue
	    _currentMessageQueue = (_currentMessageQueue + 1) % _messageQueues.Count;
	
	    while (_messageQueues[processQueue].Count > 0)
	    {
	        var msg = _messageQueues[processQueue].Dequeue();
	        ProcessMessage(msg);
	    }	
	}

	public void AddListener<T>(Action<T> listener) where T : Message
	{
	    if (!_messageListeners.ContainsKey(typeof(T)))
	    {
	        _messageListeners[typeof(T)] = new List<object>();
	        _messageSenders[typeof(T)] = CreateMessageSender<T>();
	    }
	
	    _messageListeners[typeof(T)].Add(listener);
	}
	
	public void QueueMessage(Message msg)
	{
	    _messageQueues[_currentMessageQueue].Enqueue(msg);
	}
	
	public void TriggerMessage(Message msg)
	{
	    // Immediately process
	    ProcessMessage(msg);
	}
	
	private void ProcessMessage(Message msg)
	{
	    if (_messageListeners.ContainsKey(msg.GetType()))
	    {
	        _messageSenders[msg.GetType()](msg);
	    }
	}
	
	private Action<Message> CreateMessageSender<T>() where T : Message
	{
	    return new Action<Message>(param => {
	        foreach (var listener in _messageListeners[typeof(T)])
	        {
	            ((Action<T>)listener)((T)param);
	        }
	    });
	}
}

public class Message
{
}
