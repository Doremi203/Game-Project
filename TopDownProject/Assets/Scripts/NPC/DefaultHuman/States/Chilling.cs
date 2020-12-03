using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AdvancedAI;

public class Chilling : IState
{

    private NavMeshAgent agent;

<<<<<<< Updated upstream
    public Chilling(NavMeshAgent agent) => this.agent = agent;
=======
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
>>>>>>> Stashed changes

    public void OnEnter() => agent.ResetPath();
    public void OnExit() { }
    public void Tick() { }

}