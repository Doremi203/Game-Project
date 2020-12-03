using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AdvancedAI;

public class Chilling : IState
{

    private NavMeshAgent agent;
    private Animator animator;

    public Chilling(NavMeshAgent agent, Animator animator)
    {
        this.agent = agent;
        this.animator = animator;
    }

    public void OnEnter()
    {
        agent.ResetPath();
        //animator.SetBool("chill", true);
    }

    public void OnExit()
    {
        //animator.SetBool("chill", false);
        agent.GetComponent<NPC>().Run();
    }

    public void Tick() { }

}