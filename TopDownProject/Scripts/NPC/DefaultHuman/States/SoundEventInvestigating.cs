using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AdvancedAI;

public class SoundEventInvestigating : IState
{

    public event Action OnIvestigatingOver;

    private NPC_HumanAI ai;
    private Actor npc;
    private NavMeshAgent agent;
    private bool isOver;
    private Vector3 lastSoundEventPosition;

    public SoundEventInvestigating(NPC_HumanAI ai, Actor npc, NavMeshAgent agent)
    {
        this.ai = ai;
        this.npc = npc;
        this.agent = agent;
    }

    // Test
    public void SetLastSoundEventPosition(Vector3 position) => lastSoundEventPosition = position;

    public void OnEnter()
    {
        //agent.speed = ai.ChasingSpeed;
        (npc as NPC).Run();
        agent.ResetPath();
        agent.stoppingDistance = UnityEngine.Random.Range(0.25f, 1f);
        isOver = false;
        agent.SetDestination(ai.LastSoundEventPosition);
    }

    public void OnExit()
    {
        agent.ResetPath();
        agent.stoppingDistance = 0;
    }

    public void Tick()
    {
        UpdateRotation();

        if (isOver) return;

        float _distanceTarget = GameUtilities.GetDistance2D(npc.transform.position, agent.destination);
        if (_distanceTarget <= agent.stoppingDistance)
        {
            isOver = true;
            OnIvestigatingOver?.Invoke();
        }
    }

    private void UpdateRotation()
    {
        Vector3 relativePos = agent.steeringTarget - npc.transform.position;
        relativePos.y = 0;
        if (relativePos != Vector3.zero)
            npc.desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
    }

}