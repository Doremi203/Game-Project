using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Roaming : IState
{

    private Actor npc;
    private NavMeshAgent agent;
    private float nextChangeTime;
    private Vector3? startingPoint;

    public Roaming(Actor npc, NavMeshAgent agent)
    {
        this.npc = npc;
        this.agent = agent;
    }

    public void OnEnter()
    {
        nextChangeTime = 0;
        if (startingPoint == null) startingPoint = npc.transform.position; ;
    }

    public void OnExit() { }

    public void Tick()
    {
        Vector3 relativePos = agent.steeringTarget - npc.transform.position;
        relativePos.y = 0;
        if (relativePos != Vector3.zero)
        {
            npc.desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }

        if (Time.time >= nextChangeTime) SelectNewPosition();
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