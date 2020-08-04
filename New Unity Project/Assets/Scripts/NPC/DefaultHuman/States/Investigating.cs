using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Investigating : IState
{
    private HumanNPC npc;
    private NavMeshAgent agent;
    private NPC_BaseAI ai;

    public Investigating(HumanNPC newTestNPC, NavMeshAgent agent, NPC_BaseAI ai)
    {
        this.npc = newTestNPC;
        this.agent = agent;
        this.ai = ai;
    }

    public void OnEnter()
    {
        agent.enabled = true;
    }

    public void OnExit()
    {
        agent.enabled = false;
    }

    public void Tick()
    {
        Vector3 relativePos = agent.steeringTarget - npc.transform.position;
        relativePos.y = 0;
        if (relativePos != Vector3.zero)
        {
            npc.desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }

        Vector2 npcPosition = default;
        npcPosition.x = npc.transform.position.x;
        npcPosition.y = npc.transform.position.z;

        Vector2 targetPosition = default;
        targetPosition.x = ai.TargetLastKnownPosition.x;
        targetPosition.y = ai.TargetLastKnownPosition.z;

        agent.SetDestination(ai.TargetLastKnownPosition);

        if (Vector2.Distance(npcPosition, targetPosition) < 0.5f)
        {
            ai.Target = null;
        }
    }

}