using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Investigating : IState
{

    public float stuckTime;

    public bool isInvestigatingOver;

    private Actor npc;
    private NavMeshAgent agent;

    public Investigating(Actor npc, NavMeshAgent agent)
    {
        this.npc = npc;
        this.agent = agent;
    }

    public void OnEnter()
    {
        agent.ResetPath();
        agent.stoppingDistance = Random.Range(0.25f, 1f);
        isInvestigatingOver = false;
        Player _player = Player.Instance;
        agent.SetDestination(_player.transform.position);
    }

    public void OnExit()
    {
        agent.stoppingDistance = 0;
        agent.ResetPath();
    }

    public void Tick()
    {
        UpdateRotation();

        if (isInvestigatingOver) return;

        float _distanceTarget = GameUtilities.GetDistance2D(npc.transform.position, agent.destination);
        if (_distanceTarget <= agent.stoppingDistance) isInvestigatingOver = true;
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

}