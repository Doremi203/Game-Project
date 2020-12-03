using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AdvancedAI;

public class Roaming : IState
{

    private HumanoidAI ai;
    private Actor npc;
    private NavMeshAgent agent;
    private float nextChangeTime;
    private Vector3 startingPoint;
    private float speed;

    public Roaming(HumanoidAI ai, Actor npc, NavMeshAgent agent, float speed)
    {
        this.ai = ai;
        this.npc = npc;
        this.agent = agent;
        this.speed = speed;
    }

    public void OnEnter()
    {
        agent.speed = speed;
        agent.ResetPath();
        SelectNewPosition();
    }

    public void OnExit() { }

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