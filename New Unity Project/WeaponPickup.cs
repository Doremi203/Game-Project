using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{

    // Забейте пока на этот класс, он мёртв внутри.

    [SerializeField] private WeaponBase startWeapon;

    public void SetWeapon(WeaponBase weapon)
    {

    }

    public override void Interact(Player player)
    {
        base.Interact(player);
    }

    private void Awake()
    {
        if (startWeapon) SetWeapon(startWeapon);
    }

}