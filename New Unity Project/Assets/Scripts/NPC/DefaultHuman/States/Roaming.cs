using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Roaming : IState
{

    private NewTestNPC npc;
    private NavMeshAgent agent;
    private float nextChangeTime;
    private float nextFindTargetTime;

    public Roaming(NewTestNPC newTestNPC, NavMeshAgent agent)
    {
        npc = newTestNPC;
        this.agent = agent;
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
        //npc.transform.LookAt(agent.steeringTarget);
        //npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);

        Vector3 relativePos = agent.steeringTarget - npc.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        npc.desireRotation = rotation;

        if (Time.time >= nextChangeTime) SelectNewPosition();
        if (Time.time >= nextFindTargetTime) TryFindTarget();
    }

    private void TryFindTarget()
    {
        /*
        Collider[] hits = Physics.OverlapSphere(npc.transform.position, npc.AttackRange);

        Actor closestActor = null;

        foreach (var item in hits)
        {
            Actor actor = item.GetComponent<Actor>();
            if (actor)
            {
                if(actor.Team != npc.Team && !actor.isDead)
                {
                    if (closestActor == null)
                    {
                        closestActor = actor;
                    }
                    else
                    {
                        float a = Vector3.Distance(npc.transform.position, actor.transform.position);
                        float b = Vector3.Distance(npc.transform.position, closestActor.transform.position);
                        if (a < b) closestActor = actor;
                    }
                }
            }
        }

        npc.SetTarget(closestActor);
        */

        npc.TryFindTarget();
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