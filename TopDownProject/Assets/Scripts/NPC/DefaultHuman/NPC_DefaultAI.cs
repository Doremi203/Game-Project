using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(WeaponHolder))]
public class NPC_DefaultAI : NPC_BaseAI
{

    [SerializeField] private AIType AIType;

    protected NavMeshAgent agent;
    protected WeaponHolder weaponHolder;

    protected override void Awake()
    {
        base.Awake();
        agent = this.GetComponent<NavMeshAgent>();
        weaponHolder = this.GetComponent<WeaponHolder>();

        agent.updateRotation = false;

        // States
        IState defaultState;
        switch (AIType)
        {
            case AIType.Default:
                defaultState = new Chilling(agent);
                break;
            case AIType.Patrolling:
                defaultState = new Chilling(agent);
                break;
            default:
                defaultState = new Chilling(agent);
                break;
        }

        var chilling = new Chilling(agent);
        var chasing = new Chasing(npc, agent);
        var attacking = new Attacking(npc, agent, weaponHolder, this);
        var investigating = new Investigating(npc, agent);
        var soundInvestigating = new SoundInvestigating(npc, agent, this);

        // Transitions
        stateMachine.AddTransition(chilling, chasing, CanSeePlayer);
        stateMachine.AddTransition(chilling, soundInvestigating, ShouldInvistigateSound);

        stateMachine.AddTransition(chasing, attacking, CanShootPlayer);
        stateMachine.AddTransition(attacking, chasing, CantShootPlayer);

        stateMachine.AddTransition(chasing, investigating, CantSeePlayer);

        stateMachine.AddTransition(investigating, chasing, CanSeePlayer);
        stateMachine.AddTransition(investigating, soundInvestigating, ShouldInvistigateSound);
        stateMachine.AddTransition(investigating, chilling, () => investigating.isInvestigatingOver);

        stateMachine.AddTransition(soundInvestigating, chasing, CanSeePlayer);
        stateMachine.AddTransition(soundInvestigating, chilling, () => soundInvestigating.isInvestigatingOver);

        stateMachine.AddAnyTransition(chilling, IsPlayerDead);

        // Default State
        stateMachine.SetState(chilling);
    }

    private bool CantSeePlayer() => !CanSeePlayer();

    private bool CanShootPlayer()
    {
        float _weaponReachDistance = weaponHolder.currentWeapon.NpcAttackDistance;
        return CanSeePlayer() && DistanceToPlayer() <= _weaponReachDistance;
    }

    private bool CantShootPlayer() => !CanShootPlayer();

    private bool ShouldInvistigateSound() => Time.time < LastSoundEventExpireTime;

    private bool IsPlayerDead() => !Player.Instance || Player.Instance.IsDead;

}