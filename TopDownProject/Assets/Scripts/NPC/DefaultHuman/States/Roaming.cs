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
    private Vector3 startingPoint;

    public Roaming(Actor npc, NavMeshAgent agent)
    {
        this.npc = npc;
        this.agent = agent;
        startingPoint = npc.transform.position;
    }

    public void OnEnter()
    {
        agent.ResetPath();
        SelectNewPosition();
    }

    public void OnExit() => agent.ResetPath();

    public void Tick()
    {
        UpdateRotation();
        if (Time.time >= nextChangeTime) SelectNewPosition();
    }

    private void UpdateRotation()
    {
        Vector3 relativePos = agent.steeringTarget - npc.transform.position;
        relativePos.y = 0;
        if (relativePos != Vector3.zero)
        {
            npc.desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }
    }

    private void SelectNewPosition()
    {
        Vector3 newDestination = new Vector3();
        newDestination.x = startingPoint.x + UnityEngine.Random.Range(-10f, 10f);
        newDestination.z = startingPoint.z + UnityEngine.Random.Range(-10f, 10f);
        agent.SetDestination(newDestination);
        nextChangeTime = Time.time + 2f;
    }

}