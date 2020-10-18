using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AdvancedAI;

public class SoundInvestigating : IState
{

    public event Action OnIvestigatingOver;

    private Actor npc;
    private NavMeshAgent agent;
    private NPC_HumanAI ai;
    private bool isOver;

    public SoundInvestigating(Actor npc, NavMeshAgent agent, NPC_HumanAI ai)
    {
        this.npc = npc;
        this.agent = agent;
        this.ai = ai;
    }

    public void OnEnter()
    {
        agent.ResetPath();
        agent.stoppingDistance = UnityEngine.Random.Range(0.25f, 1f);
        isOver = false;
        agent.SetDestination(ai.LastSoundEventPosition);
    }

    public void OnExit()
    {
        agent.ResetPath();
        agent.stoppingDistance = 0;
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