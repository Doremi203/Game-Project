using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HumanNPC : BaseNPC
{

    [SerializeField] private WeaponBase weaponPrefab;

    private NavMeshAgent agent;
    protected WeaponHolder weaponHolder;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        weaponHolder = GetComponent<WeaponHolder>();

        weaponHolder.EquipWeapon(Instantiate(weaponPrefab, this.transform));
    }

}