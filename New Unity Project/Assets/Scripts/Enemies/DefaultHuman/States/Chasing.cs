using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chasing : IState
{

    private NewTestNPC npc;
    private NavMeshAgent agent;
    private float nextPositionUpdate;
    private float nextFindTargetTime;

    public Chasing(NewTestNPC newTestNPC, NavMeshAgent agent)
    {
        npc = newTestNPC;
        this.agent = agent;
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
        //npc.transform.LookAt(agent.steeringTarget);
        //npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);

        Vector3 relativePos = agent.steeringTarget - npc.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        npc.desireRotation = rotation;

        if (Time.time >= nextPositionUpdate) UpdatePlayerPosition();
        if (Time.time >= nextFindTargetTime) TryFindTarget();
    }

    private void TryFindTarget()
    {
        npc.TryFindTarget();
        nextFindTargetTime = Time.time + 2f;
    }

    private void UpdatePlayerPosition()
    {
        Vector3 targetPosition = new Vector3();
        targetPosition.x = npc.Target.transform.position.x + Random.Range(-1f, 1f);
        targetPosition.z = npc.Target.transform.position.z + Random.Range(-1f, 1f);
        agent.SetDestination(targetPosition);
        nextPositionUpdate = Time.time + 0.1f;
    }

}