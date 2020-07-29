using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponHolder))]
public class Turret : MonoBehaviour
{

    [SerializeField] private float fireRate;
    [SerializeField] private WeaponBase weaponPrefab;

    private WeaponHolder weaponHolder;
    private float nextFireTime;

    private void Awake()
    {
        weaponHolder = this.GetComponent<WeaponHolder>();
        weaponHolder.EquipWeapon(Instantiate(weaponPrefab, this.transform));
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
