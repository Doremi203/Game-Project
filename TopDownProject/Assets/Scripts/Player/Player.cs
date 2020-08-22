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

    public WeaponHolder weaponHolder { get; private set; }
    public PlayerController controller { get; private set; }

    [SerializeField] private WeaponBase prefabWeaponA;
    [SerializeField] private Ability dash;
    [SerializeField] private LayerMask weaponsMask;

    public void TakeWeapon()
    {
        //if (weaponHolder.currentWeapon) weaponHolder.Drop();
        WeaponBase closestWeapon = FindWeaponAround();
        if (closestWeapon) weaponHolder.EquipWeapon(closestWeapon);
    }

    public void TakeWeaponLeftArm()
    {

    }

    private WeaponBase FindWeaponAround()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, 2f, weaponsMask);
        WeaponBase closestWeapon = null;
        foreach (var item in hits)
        {
            WeaponBase target = item.GetComponent<WeaponBase>();
            if (target == null) continue;
            if (target.IsDrop == false) continue;
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
        controller = this.GetComponent<PlayerController>();
    }

    private void Start()
    {
        // Это для теста. В реальной игре оружия будут появлятся из сохранений.
        weaponHolder.EquipWeapon(Instantiate(prefabWeaponA, this.transform));
    }

    protected override void Death()
    {
        weaponHolder.Drop();
        weaponHolder.DropAlt();
        base.Death();
    }

}