using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(WeaponHolder))]
[RequireComponent(typeof(PlayerController))]
public class Player : Actor
{

    public static Player Instance;

    public Action<Weapon> OnClosestWeaponUpdated;

    public WeaponHolder weaponHolder { get; private set; }

    [SerializeField] private LayerMask weaponsMask;
    [SerializeField] private BoolParameter invincibility;

    private Weapon closestWeapon;

    public void TakeWeapon()
    {
        Weapon closestWeapon = FindWeaponAround();

        if (closestWeapon == null) return;

        weaponHolder.EquipWeapon(closestWeapon);
    }

    private Weapon FindWeaponAround()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, 1.5f, weaponsMask);
        Weapon closestWeapon = null;
        foreach (var item in hits)
        {
            Weapon target = item.GetComponent<Weapon>();
            if (target == null) continue;
            if (target.CanPickup() == false) continue;
            if (closestWeapon == null)
            {
                closestWeapon = target;
            }
            else
            {
                float a = Vector3.Distance(this.transform.position, closestWeapon.transform.position);
                float b = Vector3.Distance(this.transform.position, target.transform.position);
                if (a > b) closestWeapon = target;
            }
        }
        return closestWeapon;
    }

    protected override void Awake()
    {
        base.Awake();
        Player.Instance = this;
        weaponHolder = this.GetComponent<WeaponHolder>();
    }

    protected override void Update()
    {
        base.Update();
        Weapon _weapon = FindWeaponAround();
        if (closestWeapon != _weapon)
        {
            closestWeapon = _weapon;
            OnClosestWeaponUpdated?.Invoke(closestWeapon);
        }
    }

    protected override void HealthChanged(float health)
    {
        if (invincibility.GetValue()) return;
        base.HealthChanged(health);
    }

}