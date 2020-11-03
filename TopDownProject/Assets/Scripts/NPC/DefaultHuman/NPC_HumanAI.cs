using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using AdvancedAI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(WeaponHolder))]
public class NPC_HumanAI : NPC_BaseAI, ISoundsListener
{

    public float DefaultSpeed => defaultSpeed;
    public float ChasingSpeed => chasingSpeed;

    [HideInInspector] public Vector3 LastSoundEventPosition;

    [Header("Human")]
    [SerializeField] private AIType AIType; 
    [SerializeField] private float reactionTime;
    [SerializeField] private float defaultSpeed = 3.75f;
    [SerializeField] private float chasingSpeed = 4.75f;

    protected NavMeshAgent agent;
    protected WeaponHolder weaponHolder;
    protected float reactionDelayTime;

    protected override void Awake()
    {
        base.Awake();

        agent = this.GetComponent<NavMeshAgent>();
        weaponHolder = this.GetComponent<WeaponHolder>();

        weaponHolder.OnWeaponChanged.AddListener(weaponHolder_OnWeaponChanged);

        agent.updateRotation = false;

        // States
        IState defaultState;
        switch (AIType)
        {
            case AIType.Default:
                defaultState = new Chilling(agent);
                break;
            case AIType.Patrolling:
                defaultState = new Patrolling(this, npc, agent);
                break;
            case AIType.Roaming:
                defaultState = new Roaming(this, npc, agent);
                break;
            default:
                defaultState = new Chilling(agent);
                break;
        }

        var chasing = new Chasing(this, npc, agent);
        var attacking = new Attacking(this, npc, agent, weaponHolder);
        var investigating = new Investigating(this, npc, agent);
        var soundInvestigating = new SoundEventInvestigating(this, npc, agent);

        investigating.OnIvestigatingOver += () => stateMachine.SetTrigger("InvestigationEnded");
        soundInvestigating.OnIvestigatingOver += () => stateMachine.SetTrigger("SoundInvestigationEnded");

        // Variables

        stateMachine.AddFloat("AttackDistance", 3);
        stateMachine.AddFloat("PlayerDistance", 0);
        stateMachine.AddBool("CanSeePlayer", false);
        stateMachine.AddBool("IsPlayerDead", false);
        stateMachine.AddTrigger("HeardSound");
        stateMachine.AddTrigger("InvestigationEnded");
        stateMachine.AddTrigger("SoundInvestigationEnded");

        // Transitions

        stateMachine.AddTransition(defaultState, chasing, new Condition[1]
        {
            new BoolCondition("CanSeePlayer", true)
        });

        stateMachine.AddTransition(defaultState, soundInvestigating, new Condition[2]
        {
            new BoolCondition("CanSeePlayer", false),
            new TriggerCondition("HeardSound")
        });

        stateMachine.AddTransition(chasing, attacking, new Condition[2]
        {
            new BoolCondition("CanSeePlayer", true),
            new FloatCondition("PlayerDistance", "AttackDistance", FloatConditionType.Less)
        });

        stateMachine.AddTransition(chasing, investigating, new Condition[1] 
        { 
            new BoolCondition("CanSeePlayer", false)
        });

        stateMachine.AddTransition(attacking, chasing, new Condition[2]
        {
            new BoolCondition("CanSeePlayer", true),
            new FloatCondition("PlayerDistance", "AttackDistance", FloatConditionType.Greater)
        });

        stateMachine.AddTransition(attacking, investigating, new Condition[1] 
        { 
            new BoolCondition("CanSeePlayer", false)
        });

        stateMachine.AddTransition(investigating, chasing, new Condition[1]
        {
            new BoolCondition("CanSeePlayer", true)
        });

        stateMachine.AddTransition(investigating, defaultState, new Condition[2]
        {
            new BoolCondition("CanSeePlayer", false),
            new TriggerCondition("InvestigationEnded")
        });

        stateMachine.AddTransition(investigating, soundInvestigating, new Condition[2] 
        {
            new BoolCondition("CanSeePlayer", false),
            new TriggerCondition("HeardSound")
        });

        stateMachine.AddTransition(soundInvestigating, chasing, new Condition[1]
        {
            new BoolCondition("CanSeePlayer", true)
        });

        stateMachine.AddTransition(soundInvestigating, defaultState, new Condition[2]
        {
            new BoolCondition("CanSeePlayer", false),
            new TriggerCondition("SoundInvestigationEnded")
        });

        stateMachine.AddGlobalTransition(defaultState, new Condition[1]
        {
            new BoolCondition("IsPlayerDead", true)
        });

        // Start State

        stateMachine.SetState(defaultState);
    }

    protected override void Update()
    {
        base.Update();

        if (CanSeePlayer())
            reactionDelayTime += Time.deltaTime;
        else
            reactionDelayTime = 0;

        stateMachine.SetBool("CanSeePlayer", reactionDelayTime >= reactionTime);
        stateMachine.SetFloat("PlayerDistance", DistanceToPlayer());
        stateMachine.SetBool("IsPlayerDead", !Player.Instance || Player.Instance.Actor.IsDead);
    }

    public void ApplySoundEvent(Actor causer, Vector3 eventPosition)
    {
        //if (AIType == AIType.Default) return;
        if (causer == this) return;
        if (causer.Team == npc.Team) return;
        LastSoundEventPosition = eventPosition;
        stateMachine.SetTrigger("HeardSound");
    }

    private void weaponHolder_OnWeaponChanged()
    {
        if (weaponHolder.CurrentWeapon)
            stateMachine.SetFloat("AttackDistance", weaponHolder.CurrentWeapon.NPCSettings.AttackDistance);
    }

}