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
        if (relativePos != Vector3.zero)
        {
            Vector3 rotation = Vector3.zero;
            rotation = Quaternion.LookRotation(relativePos, Vector3.up).eulerAngles;
            //npc.desiredRotation = rotation;
            npc.transform.eulerAngles = Vector3.Lerp(npc.transform.eulerAngles, rotation, npc.RotationSpeed * Time.deltaTime);
        }

        if (Time.time >= nextPositionUpdate) UpdateTargetPosition();
        if (Time.time >= nextFindTargetTime) TryFindTarget();
    }

    private void TryFindTarget()
    {
        ai.Target = ai.GetClosestEnemyActor();
        nextFindTargetTime = Time.time + 2f;
    }

    private void UpdateTargetPosition()
    {
        Vector3 targetPosition = ai.Target.transform.position;
        agent.SetDestination(targetPosition);
        nextPositionUpdate = Time.time + 0.5f;
    }

}