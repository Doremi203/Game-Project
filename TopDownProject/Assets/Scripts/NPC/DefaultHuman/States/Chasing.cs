using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chasing : IState
{

    private Actor npc;
    private NavMeshAgent agent;
    private NPC_BaseAI ai;
    private float nextPositionUpdate;

    public Chasing(Actor npc, NavMeshAgent agent, NPC_BaseAI ai)
    {
        this.npc = npc;
        this.agent = agent;
        this.ai = ai;
    }

    public void OnEnter()
    {
        agent.enabled = true;
        nextPositionUpdate = 0;
    }

    public void OnExit()
    {
        //agent.enabled = false;
    }

    public void Tick()
    {
        Vector3 relativePos = agent.steeringTarget - npc.transform.position;
        relativePos.y = 0;
        if (relativePos != Vector3.zero)
        {
            npc.desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }

        if (Time.time >= nextPositionUpdate) UpdateTargetPosition();
    }

    private void UpdateTargetPosition()
    {
        Vector3 targetPosition = ai.TargetLastKnownPosition;
        agent.SetDestination(targetPosition);
        nextPositionUpdate = Time.time + 0f;
    }

}