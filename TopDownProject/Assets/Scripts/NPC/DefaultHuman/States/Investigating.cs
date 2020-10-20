using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AdvancedAI;

public class Investigating : IState
{

    public event Action OnIvestigatingOver;

    private NPC_HumanAI ai;
    private Actor npc;
    private NavMeshAgent agent;
    private bool isOver;

    public Investigating(NPC_HumanAI ai, Actor npc, NavMeshAgent agent)
    {
        this.ai = ai;
        this.npc = npc;
        this.agent = agent;
    }

    public void OnEnter()
    {
        agent.speed = ai.ChasingSpeed;
        agent.ResetPath();
        agent.stoppingDistance = UnityEngine.Random.Range(0.25f, 1f);
        isOver = false;
        agent.SetDestination(Player.Instance.transform.position);
    }

    public void OnExit()
    {
        agent.stoppingDistance = 0;
        agent.ResetPath();
    }

    public void Tick()
    {
        UpdateRotation();

        if (isOver) return;

        float _distanceTarget = GameUtilities.GetDistance2D(npc.transform.position, agent.destination);
        if (_distanceTarget <= agent.stoppingDistance)
        {
            isOver = true;
            OnIvestigatingOver?.Invoke();
        }
    }

    private void UpdateRotation()
    {
        Vector3 relativePos = agent.steeringTarget - npc.transform.position;
        relativePos.y = 0;
        if (relativePos != Vector3.zero)
        {
            npc.desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }
    }

}