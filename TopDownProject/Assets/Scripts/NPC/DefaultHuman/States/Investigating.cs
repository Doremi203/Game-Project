using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Investigating : IState, IStateEnterCallbackReciver, IStateExitCallbackReciver, IStateTickCallbackReciver
{

    public event Action OnIvestigatingOver;

    public bool IsOver { get; private set; }

    private NPC_HumanAI ai;
    private Actor npc;
    private NavMeshAgent agent;
    private Vector3 investigationPosition;

    public Investigating(NPC_HumanAI ai, Actor npc, NavMeshAgent agent)
    {
        this.ai = ai;
        this.npc = npc;
        this.agent = agent;
    }

    public void SetInvestigationPosition(Vector3 position)
    {
        investigationPosition = position;
    }

    public void OnEnter()
    {
        agent.speed = ai.ChasingSpeed;
        agent.ResetPath();
        agent.stoppingDistance = UnityEngine.Random.Range(0.25f, 1f);
        IsOver = false;
        agent.SetDestination(investigationPosition);
    }

    public void OnExit()
    {
        agent.stoppingDistance = 0;
        agent.ResetPath();
    }

    public void Tick()
    {
        npc.GetComponent<RotationController>().LookAtIgnoringY(agent.steeringTarget);

        if (IsOver) return;

        float _distanceTarget = GameUtilities.GetDistanceIgnoringY(npc.transform.position, agent.destination);
        if (_distanceTarget <= agent.stoppingDistance)
        {
            IsOver = true;
            OnIvestigatingOver?.Invoke();
        }
    }

}