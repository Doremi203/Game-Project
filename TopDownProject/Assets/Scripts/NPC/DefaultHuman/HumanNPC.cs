using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanNPC : Actor
{

    [SerializeField] private WeaponBase weaponPrefab;
    [SerializeField] private WeaponBase weaponPrefabAlt;

    protected WeaponHolder weaponHolder;

    protected override void Awake()
    {
        base.Awake();
        weaponHolder = GetComponent<WeaponHolder>();
    }

    private void Start()
    {
        if (weaponPrefab) weaponHolder.EquipWeapon(Instantiate(weaponPrefab));
        if (weaponPrefabAlt) weaponHolder.EquipWeaponAlt(Instantiate(weaponPrefabAlt));
    }

    protected override void Death()
    {
        weaponHolder.Drop();
        weaponHolder.DropAlt();
        base.Death();
    }

}