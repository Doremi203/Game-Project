using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponHolder))]
public class Turret : MonoBehaviour
{

    [SerializeField] private float fireRate;

    private WeaponHolder weaponHolder;
    private float nextFireTime;

    private void Awake()
    {
        weaponHolder = this.GetComponent<WeaponHolder>();
    }

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            weaponHolder.currentWeapon.Use(true);
            nextFireTime = Time.time + fireRate;
        }
        else
        {
            weaponHolder.currentWeapon.Use(false);
        }
    }

}
