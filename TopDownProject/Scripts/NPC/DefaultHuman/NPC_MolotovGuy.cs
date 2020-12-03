using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPC_MolotovGuy : HumanoidAI
{

    protected NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();

        agent = this.GetComponent<NavMeshAgent>();

        agent.updateRotation = false;

        var roaming = new Roaming(this, npc, agent, 4f);

        stateMachine.SetState(roaming);
    }

}