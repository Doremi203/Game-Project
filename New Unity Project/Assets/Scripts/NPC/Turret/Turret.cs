using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponHolder))]
public class Turret : BaseNPC
{

    [SerializeField] private WeaponBase weaponPrefab;

    private WeaponHolder weaponHolder;

    protected override void Awake()
    {
        base.Awake();
        weaponHolder = this.GetComponent<WeaponHolder>();

        weaponHolder.EquipWeapon(Instantiate(weaponPrefab, this.transform));

        // States
        var turretIdle = new TurretIdle(this);
        var turretAttack = new TurretAttack(this, weaponHolder);

        // Transitions
        At(turretIdle, turretAttack, inAgroRange());
        At(turretIdle, turretAttack, leftShootingRange());

        stateMachine.AddAnyTransition(turretIdle, hasNoTarget());

        stateMachine.SetState(turretIdle);

        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

        Func<bool> inAgroRange() => () => TargetDistance() <= VisionRange;
        Func<bool> leftShootingRange() => () => TargetDistance() > AttackRange;
        Func<bool> hasNoTarget() => () => Target == null || Target.IsDead;

    }

}