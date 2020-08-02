using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Roaming : IState
{

    private HumanNPC npc;
    private NavMeshAgent agent;
    private NPC_BaseAI ai;
    private float nextChangeTime;
    private float nextFindTargetTime;

    public Roaming(HumanNPC newTestNPC, NavMeshAgent agent, NPC_BaseAI ai)
    {
        this.npc = newTestNPC;
        this.agent = agent;
        this.ai = ai;
    }

    public void OnEnter()
    {
        agent.enabled = true;
        nextChangeTime = 0;
        nextFindTargetTime = 0;
    }

    public void OnExit()
    {
        agent.enabled = false;
    }

    public void Tick()
    {
        Vector3 relativePos = agent.steeringTarget - npc.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        npc.desiredRotation = rotation;

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
        newDestination.x = npc.transform.position.x + UnityEngine.Random.Range(-10f, 10f);
        newDestination.z = npc.transform.position.z + UnityEngine.Random.Range(-10f, 10f);
        agent.SetDestination(newDestination);
        nextChangeTime = Time.time + 2f;
    }

}