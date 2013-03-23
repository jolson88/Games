using UnityEngine;
using System;
using System.Collections.Generic;

public enum ProcessState
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

public class ProcessManager : MonoBehaviour
{
    private List<Process> m_processesToAttach;
    private List<Process> m_processesToRemove;
    private List<Process> m_processes;
    
    void Start()
    {
        m_processesToAttach = new List<Process>();
        m_processesToRemove = new List<Process>();
        m_processes = new List<Process>();
    }

    void Update()
    {
        m_processes.RemoveAll(p => m_processesToRemove.Contains(p));
        m_processes.AddRange(m_processesToAttach);
        
        m_processesToRemove.Clear();
        m_processesToAttach.Clear();
        foreach (var process in m_processes)
        {
            if (process.State == ProcessState.Uninitialized)
            {
                process.Initialize();
            }
            
            if (process.State == ProcessState.Running)
            {
                process.Update();
            }
            
            if (process.IsDead && process.State == ProcessState.Succeeded)
            {
                process.Children.ForEach(p => AttachProcess(p));
            }
        }
        m_processes.RemoveAll(p => p.IsDead);
    }
    
    public void AttachProcess(Process process, bool replaceExistingProcesses = false)
    {
        m_processesToAttach.Add(process);
        if (replaceExistingProcesses)
        {
            m_processesToRemove.AddRange(m_processes.FindAll(p => p.GetType() == process.GetType()));
        }
    }
    
    public int GetProcessCount()
    {
        return m_processes.Count;
    }
}

public class Process
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
        m_state = ProcessState.Uninitialized;
    }
    
    public void Initialize()
    {
        OnInitialize();
        m_state = ProcessState.Running;
    }
    
    public void Update()
    {
        OnUpdate();
    }
    
    public void Succeed()
    {
        m_state = ProcessState.Succeeded;
    }
    
    public void Fail()
    {
        m_state = ProcessState.Failed;
    }
    
    public void Pause()
    {
        m_state = ProcessState.Paused;
    }
    
    public void Resume()
    {
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
    protected virtual void OnUpdate() { }
}

public class DelayProcess : Process
{
    float timeRemaining;
    
    public DelayProcess(float timeInSeconds, Process processToExecute)
    {
        this.timeRemaining = timeInSeconds;
        this.AttachChild(processToExecute);
    }
    
    protected override void OnUpdate()
    {       
        base.OnUpdate();
        
        this.timeRemaining -= Time.deltaTime;
        if (this.timeRemaining <= 0)
        {
            this.Succeed();
        }
    }
}

public class ActionProcess : Process
{
    bool m_oneTime;
    Action m_action;
    
    public ActionProcess(Action executeAction, bool oneTime = true)
    {
        this.m_oneTime = oneTime;
        this.m_action = executeAction;
    }
    
    protected override void OnUpdate()
    {
        base.OnUpdate();
        
        this.m_action();
        if (this.m_oneTime)
        {
            this.Succeed();
        }
    }
}
