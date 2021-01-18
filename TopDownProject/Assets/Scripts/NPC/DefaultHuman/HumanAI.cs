using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(WeaponHolder))]
[RequireComponent(typeof(NPCVision))]
public class HumanAI : AI, ISoundsListener
{

    private NavMeshAgent agent;
    private WeaponHolder weaponHolder;
    private NPCVision vision;

    private Chilling chilling;
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

        chilling = new Chilling(agent);
        chasing = new Chasing(this, npc, agent);
        attacking = new Attacking(this, npc, agent, weaponHolder);
        investigating = new Investigating(this, npc, agent);

        soundHeardTrigger = stateMachine.CreateTrigger();
    }

    protected override void SetupStateMachine()
    {
        stateMachine.AddTransition(chilling, chasing, () => vision.IsSeeingPlayer);
        stateMachine.AddTransition(chilling, investigating, () => soundHeardTrigger.IsActive);

        stateMachine.AddTransition(chasing, investigating, () => !vision.IsSeeingPlayer);
        stateMachine.AddTransition(chasing, attacking, () => vision.DistanceToPlayer() < weaponHolder.CurrentWeapon.NPCSettings.AttackDistance);

        stateMachine.AddTransition(attacking, investigating, () => !vision.IsSeeingPlayer);
        stateMachine.AddTransition(attacking, chasing, () => vision.DistanceToPlayer() > weaponHolder.CurrentWeapon.NPCSettings.AttackDistance);

        stateMachine.AddTransition(investigating, chasing, () => vision.IsSeeingPlayer);
        stateMachine.AddTransition(investigating, chilling, () => investigating.IsOver);

        stateMachine.AddGlobalTransition(chilling, () => Player.Instance.Actor.HealthComponent.IsDead);
    }

    protected override IState GetInitialState()
    {
        return chilling;
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