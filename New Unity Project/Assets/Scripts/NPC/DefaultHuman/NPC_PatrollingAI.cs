using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(WeaponHolder))]
public class NPC_PatrollingAI : NPC_BaseAI
{

    protected NavMeshAgent agent;
    protected WeaponHolder weaponHolder;

    protected override void Awake()
    {
        base.Awake();
        agent = this.GetComponent<NavMeshAgent>();
        weaponHolder = this.GetComponent<WeaponHolder>();

        // States
        var patrolling = new Patrolling(npc as HumanNPC, agent, this);
        var chasing = new Chasing(npc as HumanNPC, agent, this);
        var attacking = new Attacking(npc as HumanNPC, agent, weaponHolder, this);

        // Transitions
        At(patrolling, chasing, inAgroRange());
        At(chasing, patrolling, isPlayerFarAway());
        At(chasing, attacking, canShootPlayer());
        At(attacking, chasing, cantShootPlayer());
        At(attacking, chasing, leftShootingRange());

        stateMachine.AddAnyTransition(patrolling, hasNoTarget());

        stateMachine.SetState(patrolling);

        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

        Func<bool> inAgroRange() => () => DistanceToTarget() <= VisionRange;
        Func<bool> isPlayerFarAway() => () => DistanceToTarget() > TargetLostRange;
        Func<bool> canShootPlayer() => () => CanSeeTheTarget();
        Func<bool> cantShootPlayer() => () => !CanSeeTheTarget();
        Func<bool> leftShootingRange() => () => DistanceToTarget() > AttackRange;

        Func<bool> hasNoTarget() => () => Target == null || Target.IsDead;
    }

}

