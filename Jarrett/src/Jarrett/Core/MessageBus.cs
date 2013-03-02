using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarrett.Core
{
    class MessageBus
    {
        private static MessageBus s_bus;
        public static MessageBus Get()
        {
            if (s_bus == null)
            {
                s_bus = new MessageBus();
            }
            return s_bus;
        }

        private Dictionary<Type, List<Action<Message>>> m_messageListeners;
        private List<Queue<Message>> m_messageQueues;
        private int m_currentMessageQueue;

        public void Initialize()
        {
            m_messageListeners = new Dictionary<Type, List<Action<Message>>>();
            m_messageQueues = new List<Queue<Message>>() { new Queue<Message>(), new Queue<Message>() };
        }

        public void AddListener<T>(Action<Message> listener) where T : Message
        {
            if (!m_messageListeners.Keys.Contains(typeof(T)))
            {
                m_messageListeners[typeof(T)] = new List<Action<Message>>();
            }

            m_messageListeners[typeof(T)].Add(listener);
        }

        public void ProcessMessages()
        {
            var processQueue = m_currentMessageQueue;

            // Make sure any new messages coming in from processing these messages goes
            // to a different queue
            m_currentMessageQueue = (m_currentMessageQueue + 1) % m_messageQueues.Count;

            while (m_messageQueues[processQueue].Count > 0)
            {
                var msg = m_messageQueues[processQueue].Dequeue();
                ProcessMessage(msg);
            }
        }

        public void QueueMessage(Message msg)
        {
            m_messageQueues[m_currentMessageQueue].Enqueue(msg);
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

        private MessageBus() { }
    }
}
