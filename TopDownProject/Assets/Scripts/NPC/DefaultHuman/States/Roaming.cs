using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Roaming : IState, IStateEnterCallbackReciver, IStateTickCallbackReciver
{

    private NPC_HumanAI ai;
    private Actor npc;
    private NavMeshAgent agent;
    private float nextChangeTime;
    private Vector3 startingPoint;
    private float speed;

    public Roaming(NPC_HumanAI ai, Actor npc, NavMeshAgent agent, float speed)
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

    public void Tick()
    {
        npc.GetComponent<RotationController>().LookAtIgnoringY(agent.steeringTarget);
        if (Time.time >= nextChangeTime) SelectNewPosition();
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