using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(WeaponHolder))]
public class NPC : Actor
{
   
    private enum NPCState
    {
        Action,
        ActionExiting,
        Moving
    }

    [SerializeField] private NPCHumanAction action;
    [SerializeField] private Animator animator;

    private NavMeshAgent agent;
    private WeaponHolder weaponHolder;
    private NPCState currentState;
    private float timeToExit;

    public void Run()
    {
        SetState(NPCState.ActionExiting);
    }

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        weaponHolder = GetComponent<WeaponHolder>();
    }

    protected override void Start()
    {
        base.Start();
        SetState(NPCState.Action);
    }

    protected override void Update()
    {
        switch (currentState)
        {
            case NPCState.Action:
                break;

            case NPCState.ActionExiting:

                if (Time.time >= timeToExit)
                    SetState(NPCState.Moving);

                break;
            case NPCState.Moving:

                base.Update();

                break;
            default:
                break;
        }
    }

    private void SetState(NPCState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case NPCState.Action:

                weaponHolder.EnableWeaponUsing(false);
                animator.SetBool("chill", true);
                agent.speed = 0f;

                break;
            case NPCState.ActionExiting:

                weaponHolder.EnableWeaponUsing(false);
                animator.SetTrigger("exit");
                animator.SetBool("chill", false);
                agent.speed = 0f;
                timeToExit = Time.time + action.exitTime;

                break;
            case NPCState.Moving:

                weaponHolder.EnableWeaponUsing(true);
                agent.speed = 5f;

                break;
            default:
                break;
        }
    }

}