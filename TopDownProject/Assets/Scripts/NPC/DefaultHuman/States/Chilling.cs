using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chilling : IState
{

    private NavMeshAgent agent;

    public Chilling(NavMeshAgent agent) => this.agent = agent;

    public void OnEnter() => agent.ResetPath();
    public void OnExit() { }
    public void Tick() { }

}