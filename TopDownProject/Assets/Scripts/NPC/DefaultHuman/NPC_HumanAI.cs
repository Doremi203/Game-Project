using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(WeaponHolder))]
[RequireComponent(typeof(NPCVision))]
public class NPC_HumanAI : BaseAI, ISoundsListener
{

    public float DefaultSpeed => defaultSpeed;
    public float ChasingSpeed => chasingSpeed;

    [Header("Human")]
    [SerializeField] private AIType AIType; 
    [SerializeField] private float reactionTime;
    [SerializeField] private float defaultSpeed = 3.75f;
    [SerializeField] private float chasingSpeed = 4.75f;

    private NavMeshAgent agent;
    private WeaponHolder weaponHolder;
    private NPCVision vision;

    private IState defaultState;
    private Chasing chasing;
    private Attacking attacking;
    private Investigating investigating;

    private Trigger soundHeardTrigger;

    public void ApplySoundEvent(Actor causer, Vector3 eventPosition)
    {
        if (causer == this) return;
        if (causer.Team == npc.Team) return;
        investigating.SetInvestigationPosition(eventPosition);
        soundHeardTrigger.SetActive();
    }

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
        weaponHolder = GetComponent<WeaponHolder>();
        vision = GetComponent<NPCVision>();

        agent.updateRotation = false;

        // States
        switch (AIType)
        {
            case AIType.Default:
                defaultState = new Chilling(agent);
                break;
            case AIType.Patrolling:
                defaultState = new Patrolling(this, npc, agent);
                break;
            case AIType.Roaming:
                defaultState = new Roaming(this, npc, agent, defaultSpeed);
                break;
            default:
                defaultState = new Chilling(agent);
                break;
        }

        chasing = new Chasing(this, npc, agent);
        attacking = new Attacking(this, npc, agent, weaponHolder);
        investigating = new Investigating(this, npc, agent);

        soundHeardTrigger = stateMachine.CreateTrigger();

        // Transitions
        stateMachine.AddTransition(defaultState, chasing, () => vision.IsSeeingPlayer);
        stateMachine.AddTransition(defaultState, investigating, () => soundHeardTrigger.IsActive);

        stateMachine.AddTransition(chasing, investigating, () => !vision.IsSeeingPlayer);
        stateMachine.AddTransition(chasing, attacking, () => vision.DistanceToPlayer() < weaponHolder.CurrentWeapon.NPCSettings.AttackDistance);

        stateMachine.AddTransition(attacking, investigating, () => !vision.IsSeeingPlayer);
        stateMachine.AddTransition(attacking, chasing, () => vision.DistanceToPlayer() > weaponHolder.CurrentWeapon.NPCSettings.AttackDistance);

        stateMachine.AddTransition(investigating, chasing, () => vision.IsSeeingPlayer);
        stateMachine.AddTransition(investigating, defaultState, () => investigating.IsOver);

        stateMachine.AddGlobalTransition(defaultState, () => Player.Instance.Actor.HealthComponent.IsDead);

        // Start State
        stateMachine.SetState(defaultState);
    }

    protected override void Update()
    {
        base.Update();

        if (vision.IsSeeingPlayer)
        {
            investigating.SetInvestigationPosition(Player.Instance.Actor.transform.position);
        }
    }

}