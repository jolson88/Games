using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Acorn.Hiromi.Processing
{
    public class Process
    {
        private ProcessState _slate;
        private List<Process> _children;

        public ProcessState State { get { return _slate; } }
        public bool IsAlive
        {
            get
            {
                return _slate == ProcessState.Running || _slate == ProcessState.Paused;
            }
        }
        public bool IsDead
        {
            get
            {
                return _slate == ProcessState.Succeeded || _slate == ProcessState.Failed || _slate == ProcessState.Aborted;
            }
        }
        public bool IsRemoved { get { return _slate == ProcessState.Removed; } }
        public bool IsPaused { get { return _slate == ProcessState.Paused; } }
        public List<Process> Children { get { return _children; } }

        public Process()
        {
            _children = new List<Process>();
            _slate = ProcessState.Uninitialized;
        }

        public void Initialize()
        {
            OnInitialize();
            _slate = ProcessState.Running;
        }

        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        public void Succeed()
        {
            _slate = ProcessState.Succeeded;
        }

        public void Fail()
        {
            _slate = ProcessState.Failed;
        }

        public void Pause()
        {
            _slate = ProcessState.Paused;
        }

        public void Resume()
        {
            _slate = ProcessState.Running;
        }

        public void AttachChild(Process child)
        {
            _children.Add(child);
        }

        public void RemoveChildren()
        {
            _children.Clear();
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
    }
}
