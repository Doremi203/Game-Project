using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TestEnemy : BaseEnemy
{

    protected NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void StartStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {

            { typeof(WanderState), new WanderState(this) },
            { typeof(ChaseState), new ChaseState(this) },
            { typeof(AttackState), new AttackState(this) }

        };
        stateMachine.SetStates(states);
    }

}