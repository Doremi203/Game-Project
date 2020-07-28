using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{

    [SerializeField] private WeaponBase startWeapon;

    public void SetWeapon(WeaponBase weapon)
    {

    }

    protected override bool OnPickedUp(Player player)
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        if (startWeapon) SetWeapon(startWeapon);
    }

}