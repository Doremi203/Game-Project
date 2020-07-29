using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(WeaponHolder))]
public abstract class BaseEnemy : Actor
{

    [SerializeField] private WeaponBase weaponPrefab;

    [SerializeField] private float detectRadius;
    public float DetectRadius => detectRadius;

    protected WeaponHolder weaponHolder;
    public WeaponHolder WeaponHolder => weaponHolder;

    protected StateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = GetComponent<StateMachine>();
        weaponHolder = GetComponent<WeaponHolder>();
        weaponHolder.EquipWeapon(Instantiate(weaponPrefab, this.transform));
        StartStateMachine();
    }

    protected abstract void StartStateMachine();

}