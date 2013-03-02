using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarrett.Core
{
    enum ProcessState
    {
        // Neither dead or alive
        Uninitialized,
        Removed,

        // Living
        Running,
        Paused,

        // Dead
        Succeeded,
        Failed,
        Aborted
    }

    class Process
    {
        ProcessState m_state;
        List<Process> m_children;

        public ProcessState State { get { return m_state; } }
        public bool IsAlive 
        { 
            get 
            { 
                return m_state == ProcessState.Running || m_state == ProcessState.Paused; 
            } 
        }
        public bool IsDead
        {
            get
            {
                return m_state == ProcessState.Succeeded || m_state == ProcessState.Failed || m_state == ProcessState.Aborted;
            }
        }
        public bool IsRemoved { get { return m_state == ProcessState.Removed; } }
        public bool IsPaused { get { return m_state == ProcessState.Paused; } }

        public List<Process> Children { get { return m_children; } }


        public Process()
        {
            m_children = new List<Process>();
        }

        public void Initialize()
        {
            OnInitialize();
            m_state = ProcessState.Running;
        }

        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        public void Succeed()
        {
            Debug.Assert(IsAlive);
            m_state = ProcessState.Succeeded;
        }

        public void Fail()
        {
            Debug.Assert(IsAlive);
            m_state = ProcessState.Failed;
        }

        public void Pause()
        {
            Debug.Assert(m_state == ProcessState.Running);
            m_state = ProcessState.Paused;
        }

        public void Resume()
        {
            Debug.Assert(IsPaused);
            m_state = ProcessState.Running;
        }

        public void AttachChild(Process child)
        {
            m_children.Add(child);
        }

        public void RemoveChildren()
        {
            m_children.Clear();
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
    }
}
