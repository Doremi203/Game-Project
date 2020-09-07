using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoundInvestigating : IState
{
    public bool isInvestigatingOver;

    private Actor npc;
    private NavMeshAgent agent;
    private NPC_BaseAI ai;

    public SoundInvestigating(Actor npc, NavMeshAgent agent, NPC_BaseAI ai)
    {
        this.npc = npc;
        this.agent = agent;
        this.ai = ai;
    }

    public void OnEnter()
    {
        agent.stoppingDistance = Random.Range(0.25f, 1f);
        isInvestigatingOver = false;
        agent.SetDestination(ai.LastSoundEventPosition);
    }

    public void OnExit() => agent.stoppingDistance = 0;

    public void Tick()
    {
        UpdateRotation();

        if (isInvestigatingOver) return;

        float _distanceTarget = GameUtilities.GetDistance2D(npc.transform.position, agent.destination);
        if (_distanceTarget <= agent.stoppingDistance) isInvestigatingOver = true;
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