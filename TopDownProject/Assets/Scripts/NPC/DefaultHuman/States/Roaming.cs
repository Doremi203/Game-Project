using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AdvancedAI;

public class Roaming : IState
{

    private NPC_HumanAI ai;
    private Actor npc;
    private NavMeshAgent agent;
    private float nextChangeTime;
    private Vector3 startingPoint;

    public Roaming(NPC_HumanAI ai, Actor npc, NavMeshAgent agent)
    {
        this.ai = ai;
        this.npc = npc;
        this.agent = agent;
    }

    public void OnEnter()
    {
        agent.speed = ai.DefaultSpeed;
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