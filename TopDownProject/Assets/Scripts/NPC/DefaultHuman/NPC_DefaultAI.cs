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
        var chilling = new Chilling(npc as HumanNPC, agent, this);
        var chasing = new Chasing(npc as HumanNPC, agent, this);
        var attacking = new Attacking(npc as HumanNPC, agent, weaponHolder, this);
        var investigating = new Investigating(npc as HumanNPC, agent, this);

        // Transitions
        At(chilling, chasing, canSeeTarget());
        At(chilling, investigating, shouldInvistigateSound());

        At(chasing, attacking, canShootPlayer());
        At(attacking, chasing, cantShootPlayer());

        At(chasing, investigating, cantSeeTarget());

        At(investigating, chilling, shouldReturn());
        At(investigating, chasing, canSeeTarget());

        stateMachine.SetState(chilling);

        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

        Func<bool> canShootPlayer() => () => CanSee(Target) && DistanceToTarget() <= weaponHolder.currentWeapon.NpcAttackDistance;
        Func<bool> cantShootPlayer() => () => !CanSee(Target) || DistanceToTarget() > weaponHolder.currentWeapon.NpcAttackDistance;

        Func<bool> cantSeeTarget() => () => !CanSee(Target);
        Func<bool> canSeeTarget() => () => CanSee(Target);

        Func<bool> shouldInvistigateSound() => () => Time.time < LastSoundEventExpireTime;
        Func<bool> shouldReturn() => () => investigating.stuckTime > 2f && !CanSee(Target);

    }

}