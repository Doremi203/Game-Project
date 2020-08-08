using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{

    // Забейте пока на этот класс, он мёртв внутри.

    [SerializeField] private WeaponBase startWeapon;

    private WeaponBase currentWeapon;

    private void Awake()
    {
        currentWeapon = Instantiate(startWeapon, this.transform);
        currentWeapon.transform.position = this.transform.position;
        currentWeapon.transform.rotation = Quaternion.identity;
    }

    protected override bool OnPickedUp(Player player)
    {
        return true;
    }

}