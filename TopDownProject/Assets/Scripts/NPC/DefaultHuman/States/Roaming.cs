using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Roaming : IState
{

    private Actor npc;
    private NavMeshAgent agent;
    private NPC_BaseAI ai;
    private float nextChangeTime;
    private float nextFindTargetTime;
    private Vector3? startingPoint;

    public Roaming(Actor npc, NavMeshAgent agent, NPC_BaseAI ai)
    {
        this.npc = npc;
        this.agent = agent;
        this.ai = ai;
    }

    public void OnEnter()
    {
        //agent.enabled = true;
        nextChangeTime = 0;
        nextFindTargetTime = 0;
        if (startingPoint == null) startingPoint = npc.transform.position; 
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

        if (Time.time >= nextChangeTime) SelectNewPosition();
        if (Time.time >= nextFindTargetTime) TryFindTarget();
    }

    private void TryFindTarget()
    {
        ai.Target = ai.GetClosestEnemyActor();
        nextFindTargetTime = Time.time + 0.5f;
    }

    private void SelectNewPosition()
    {
        Vector3 newDestination = new Vector3();
        newDestination.x = startingPoint.Value.x + UnityEngine.Random.Range(-10f, 10f);
        newDestination.z = startingPoint.Value.z + UnityEngine.Random.Range(-10f, 10f);
        agent.SetDestination(newDestination);
        nextChangeTime = Time.time + 2f;
    }

}