using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Jarrett.Core
{
    class ProcessManager
    {
        List<Process> m_processes;

        public ProcessManager()
        {
            m_processes = new List<Process>();
        }

        public void UpdateProcesses(GameTime gameTime)
        {
            foreach (var process in m_processes)
            {
                if (process.State == ProcessState.Uninitialized)
                {
                    process.Initialize();
                }

                if (process.State == ProcessState.Running)
                {
                    process.Update(gameTime);
                }

                if (process.IsDead && process.State == ProcessState.Succeeded)
                {
                    m_processes.AddRange(process.Children);
                }
            }

            m_processes.RemoveAll(p => p.IsDead);
        }

        public void AttachProcess(Process process)
        {
            m_processes.Add(process);
        }

        public int GetProcessCount()
        {
            return m_processes.Count;
        }
    }
}
