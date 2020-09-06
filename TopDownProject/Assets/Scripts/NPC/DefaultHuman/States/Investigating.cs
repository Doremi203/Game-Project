using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Investigating : IState
{

    public float stuckTime;

    private Actor npc;
    private NavMeshAgent agent;
    private NPC_BaseAI ai;
    private float nextFindTargetTime;
    private float nextFindPositionTime;

    public Investigating(Actor npc, NavMeshAgent agent, NPC_BaseAI ai)
    {
        this.npc = npc;
        this.agent = agent;
        this.ai = ai;
    }

    public void OnEnter()
    {
        agent.enabled = true;
        stuckTime = 0;
        nextFindTargetTime = 0;
        FindPosition();
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

        if (agent.velocity.magnitude <= 0)
        {
            stuckTime += Time.deltaTime;
        }
        else
        {
            stuckTime = 0;
        }

        if (Utils.GetDistance2D(npc.transform.position, agent.destination) > 0.5f)
        {
            if (Time.time >= nextFindPositionTime) FindPosition();
        }

        if (Time.time >= nextFindTargetTime) TryFindTarget();
    }

    private void TryFindTarget()
    {
        if(ai.Target == null) ai.Target = ai.GetClosestEnemyActor();
        nextFindTargetTime = Time.time + 0.5f;
    }

    private void FindPosition()
    {
        Vector3 target = ai.TargetLastKnownPosition;
        //target.x = ai.TargetLastKnownPosition.x + Random.Range(-2f, 2f);
        //target.z = ai.TargetLastKnownPosition.z + Random.Range(-2f, 2f);
        agent.SetDestination(target);
        nextFindPositionTime = Time.time + 0.5f;
    }

}