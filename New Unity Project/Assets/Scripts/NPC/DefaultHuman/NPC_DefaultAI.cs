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

        // Transitions
        At(roaming, chasing, inAgroRange());
        At(chasing, roaming, isPlayerFarAway());
        At(chasing, attacking, canShootPlayer());
        At(attacking, chasing, cantShootPlayer());
        At(attacking, chasing, leftShootingRange());

        stateMachine.AddAnyTransition(roaming, hasNoTarget());

        stateMachine.SetState(roaming);

        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

        Func<bool> inAgroRange() => () => DistanceToTarget() <= VisionRange;
        Func<bool> isPlayerFarAway() => () => DistanceToTarget() > TargetLostRange;
        Func<bool> canShootPlayer() => () => CanSeeTheTarget();
        Func<bool> cantShootPlayer() => () => !CanSeeTheTarget();
        Func<bool> leftShootingRange() => () => DistanceToTarget() > AttackRange;

        Func<bool> hasNoTarget() => () => Target == null || Target.IsDead;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (stateMachine == null) return;

        IState currentState = stateMachine.GetCurrentState();

        if (currentState == null) return;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontSize = 25;

        Handles.Label(transform.position, currentState.ToString(), style);
    }

}