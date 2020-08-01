using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NewTestNPC : BaseNPC
{

    [SerializeField] private WeaponBase weaponPrefab;
    [SerializeField] private TextMesh text;

    private NavMeshAgent agent;
    protected WeaponHolder weaponHolder;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        weaponHolder = GetComponent<WeaponHolder>();

        weaponHolder.EquipWeapon(Instantiate(weaponPrefab, this.transform));

        // States
        var roaming = new Roaming(this, agent);
        var chasing = new Chasing(this, agent);
        var attacking = new Attacking(this, agent, weaponHolder);

        // Transitions
        At(roaming, chasing, inAgroRange());
        At(chasing, roaming, isPlayerFarAway());
        At(chasing, attacking, canShootPlayer());
        At(attacking, chasing, cantShootPlayer());
        At(attacking, chasing, leftShootingRange());

        stateMachine.AddAnyTransition(roaming, hasNoTarget());

        stateMachine.SetState(roaming);

        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

        Func<bool> inAgroRange() => () => TargetDistance() <= VisionRange;
        Func<bool> isPlayerFarAway() => () => TargetDistance() > TargetLostRange;
        Func<bool> canShootPlayer() => () => CanSeeTheTarget();
        Func<bool> cantShootPlayer() => () => !CanSeeTheTarget();
        Func<bool> leftShootingRange() => () => TargetDistance() > AttackRange;

        Func<bool> hasNoTarget() => () => Target == null || Target.IsDead;

    }

    /*
    public override void ApplyDamage(Actor damageCauser, float damage, DamageType damageType)
    {
        base.ApplyDamage(damageCauser, damage, damageType);
        target = damageCauser;
    }
    */

    protected override void Update()
    {
        base.Update();
        text.text = stateMachine.GetCurrentState().ToString();
    }

}