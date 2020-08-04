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

    protected NavMeshAgent agent;
    protected WeaponHolder weaponHolder;

    protected override void Awake()
    {
        base.Awake();
        agent = this.GetComponent<NavMeshAgent>();
        weaponHolder = this.GetComponent<WeaponHolder>();

        // States
        var roaming = new Roaming(npc as HumanNPC, agent, this);
        var chasing = new Chasing(npc as HumanNPC, agent, this);
        var attacking = new Attacking(npc as HumanNPC, agent, weaponHolder, this);
        var investigating = new Investigating(npc as HumanNPC, agent, this);

        // Transitions
        At(roaming, chasing, hasTarget());

        At(chasing, attacking, canShootPlayer());
        At(chasing, investigating, cantSeeTarget());

        At(investigating, chasing, canSeeTarget());

        At(attacking, investigating, cantSeeTarget());
        At(attacking, chasing, cantShootPlayer());

        stateMachine.AddAnyTransition(roaming, hasNoTarget());
        stateMachine.AddAnyTransition(roaming, isPlayerFarAway());
        stateMachine.AddAnyTransition(attacking, canShootPlayer());

        stateMachine.SetState(roaming);

        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

        Func<bool> isPlayerFarAway() => () => DistanceToTarget() > TargetLostRange;

        Func<bool> canShootPlayer() => () => CanSee(Target) && DistanceToTarget() <= AttackRange;
        Func<bool> cantShootPlayer() => () => !CanSee(Target) || DistanceToTarget() > AttackRange;

        Func<bool> hasTarget() => () => Target && !Target.IsDead;
        Func<bool> hasNoTarget() => () => Target == null || Target.IsDead;

        Func<bool> cantSeeTarget() => () => !CanSee(Target);
        Func<bool> canSeeTarget() => () => CanSee(Target);
    }

}