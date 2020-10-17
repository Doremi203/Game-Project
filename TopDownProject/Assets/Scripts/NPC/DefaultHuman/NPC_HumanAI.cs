using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(WeaponHolder))]
public class NPC_HumanAI : NPC_BaseAI
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
                defaultState = new Patrolling(npc, agent);
                break;
            case AIType.Roaming:
                defaultState = new Roaming(npc, agent);
                break;
            default:
                defaultState = new Chilling(agent);
                break;
        }

        var chasing = new Chasing(npc, agent);
        var attacking = new Attacking(npc, agent, weaponHolder);
        var investigating = new Investigating(npc, agent);
        var soundInvestigating = new SoundInvestigating(npc, agent, this);
        var noWeapon = new NoWeapon(npc, agent);

        // Transitions
        stateMachine.AddTransition(defaultState, chasing, CanSeePlayer);
        stateMachine.AddTransition(defaultState, soundInvestigating, ShouldInvistigateSound);

        stateMachine.AddTransition(chasing, attacking, CanShootPlayer);
        stateMachine.AddTransition(attacking, chasing, CantShootPlayer);

        stateMachine.AddTransition(chasing, investigating, CantSeePlayer);

        stateMachine.AddTransition(investigating, chasing, CanSeePlayer);
        stateMachine.AddTransition(investigating, soundInvestigating, ShouldInvistigateSound);
        stateMachine.AddTransition(investigating, defaultState, () => investigating.isInvestigatingOver);

        stateMachine.AddTransition(soundInvestigating, chasing, CanSeePlayer);
        stateMachine.AddTransition(soundInvestigating, defaultState, () => soundInvestigating.isInvestigatingOver);

         stateMachine.AddAnyTransition(defaultState, IsPlayerDead);
        // Test. This will probably cause a constant switch between default state and noWeapon state
       // stateMachine.AddAnyTransition(noWeapon, () => !weaponHolder.CurrentWeapon);

        // Default State
        stateMachine.SetState(defaultState);
    }

    private bool CantSeePlayer() => !CanSeePlayer();

    private bool CanShootPlayer()
    {
        float _weaponReachDistance = weaponHolder.CurrentWeapon.NPCSettings.AttackDistance;
        return CanSeePlayer() && DistanceToPlayer() <= _weaponReachDistance;
    }

    private bool CantShootPlayer() => !CanShootPlayer();

    private bool ShouldInvistigateSound() => Time.time < LastSoundEventExpireTime;

    private bool IsPlayerDead() => !Player.Instance || Player.Instance.Actor.IsDead;

}