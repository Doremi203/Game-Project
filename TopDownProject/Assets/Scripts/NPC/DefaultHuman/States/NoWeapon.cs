using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NoWeapon : IState
{

    private Actor npc;
    private NavMeshAgent agent;

    public NoWeapon(Actor npc, NavMeshAgent agent)
    {
        this.npc = npc;
        this.agent = agent;
    }

    public void OnEnter() => agent.ResetPath();

    public void OnExit() => agent.ResetPath();

    public void Tick() 
    {
        Vector3 relativePos = Player.Instance.Actor.transform.position - npc.transform.position;
        relativePos.y = 0;
        if (relativePos != Vector3.zero)
            npc.desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
    }

}