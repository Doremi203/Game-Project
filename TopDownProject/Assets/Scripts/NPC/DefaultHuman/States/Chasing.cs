using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chasing : IState, IStateEnterCallbackReciver, IStateTickCallbackReciver
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
        agent.speed = ai.ChasingSpeed; 
    }

    public void Tick()
    {
        npc.GetComponent<RotationController>().LookAtIgnoringY(agent.steeringTarget);
        agent.SetDestination(Player.Instance.transform.position);
    }

}