using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chilling : IState
{

    private NavMeshAgent agent;

    public Chilling(NavMeshAgent agent) => this.agent = agent;

    public void OnEnter() => agent.isStopped = true;
    public void OnExit() => agent.isStopped = false;
    public void Tick() { }

}