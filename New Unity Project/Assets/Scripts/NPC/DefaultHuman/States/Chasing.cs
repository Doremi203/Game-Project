using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chasing : IState
{

    private HumanNPC npc;
    private NavMeshAgent agent;
    private NPC_BaseAI ai;
    private float nextPositionUpdate;
    private float nextFindTargetTime;

    public Chasing(HumanNPC newTestNPC, NavMeshAgent agent, NPC_BaseAI ai)
    {
        this.npc = newTestNPC;
        this.agent = agent;
        this.ai = ai;
    }

    public void OnEnter()
    {
        agent.enabled = true;
        nextPositionUpdate = 0;
        nextFindTargetTime = 0;
    }

    public void OnExit()
    {
        agent.enabled = false;
    }

    public void Tick()
    {
        Vector3 relativePos = agent.steeringTarget - npc.transform.position;
        relativePos.y = 0;
        Vector3 rotation = Quaternion.LookRotation(relativePos, Vector3.up).eulerAngles;
        npc.desiredRotation = rotation;

        if (Time.time >= nextPositionUpdate) UpdatePlayerPosition();
        if (Time.time >= nextFindTargetTime) TryFindTarget();
    }

    private void TryFindTarget()
    {
        ai.Target = ai.GetClosestEnemyActor();
        nextFindTargetTime = Time.time + 2f;
    }

    private void UpdatePlayerPosition()
    {
        Vector3 targetPosition = new Vector3();
        targetPosition.x = ai.Target.transform.position.x + Random.Range(-1f, 1f);
        targetPosition.z = ai.Target.transform.position.z + Random.Range(-1f, 1f);
        agent.SetDestination(targetPosition);
        nextPositionUpdate = Time.time + 0.1f;
    }

}