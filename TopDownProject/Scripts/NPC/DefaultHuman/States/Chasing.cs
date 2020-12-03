using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AdvancedAI;

public class Chasing : IState
{

    private NPC_HumanAI ai;
    private Actor npc;
    private NavMeshAgent agent;

    public Chasing(NPC_HumanAI ai, Actor npc, NavMeshAgent agent)
    {
        this.ai = ai;
        this.npc = npc;
        this.agent = agent;
    }

    public void OnEnter()
    {
        agent.ResetPath();
        //agent.speed = ai.ChasingSpeed; 
    }

    public void OnExit() { }

    public void Tick()
    {
        UpdateRotation();
        SetDestianation();
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

    private void SetDestianation()
    {
        Player _player = Player.Instance;
        agent.SetDestination(_player.transform.position);
    }

}