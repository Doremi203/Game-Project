using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(WeaponHolder))]
[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour
{

    public static Player Instance;

    public Action<Weapon> OnClosestWeaponUpdated;

    public Actor Actor { get; private set; }
    public WeaponHolder WeaponHolder { get; private set; }

    [SerializeField] private LayerMask weaponsMask;
    [SerializeField] private BoolParameter invincibility;

    private Weapon closestWeapon;

    public void TakeWeapon()
    {
        Weapon closestWeapon = FindWeaponAround();

        if (closestWeapon == null) return;

        WeaponHolder.EquipWeapon(closestWeapon);
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

    private void Awake()
    {
        Player.Instance = this;
        Actor = GetComponent<Actor>();
        WeaponHolder = GetComponent<WeaponHolder>();
    }

    private void Update()
    {
        Weapon _weapon = FindWeaponAround();
        if (closestWeapon != _weapon)
        {
            closestWeapon = _weapon;
            OnClosestWeaponUpdated?.Invoke(closestWeapon);
        }
    }

}