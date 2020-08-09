using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chilling : IState
{

    private HumanNPC npc;
    private NavMeshAgent agent;
    private NPC_BaseAI ai;
    private Vector3 startingPoint;
    private Quaternion startingRotation;
    private float nextFindTargetTime;

    public Chilling(HumanNPC newTestNPC, NavMeshAgent agent, NPC_BaseAI ai)
    {
        this.npc = newTestNPC;
        this.agent = agent;
        this.ai = ai;
        startingPoint = npc.transform.position;
        startingRotation = npc.transform.rotation;
    }

    public void OnEnter()
    {
        agent.enabled = true;
        agent.SetDestination(startingPoint);
        nextFindTargetTime = 0;
    }

    public void OnExit()
    {
        agent.enabled = false;
    }

    public void Tick()
    {
        if (Vector3.Distance(npc.transform.position, startingPoint) > 0.2f)
        {
            Vector3 relativePos = agent.steeringTarget - npc.transform.position;
            relativePos.y = 0;
            if (relativePos != Vector3.zero)
            {
                npc.desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
            }
        }
        else
        {
            npc.desiredRotation = startingRotation;
        }
        if (Time.time >= nextFindTargetTime) TryFindTarget();
    }

    private void TryFindTarget()
    {
        ai.Target = ai.GetClosestEnemyActor();
        nextFindTargetTime = Time.time + 0.5f;
    }

}