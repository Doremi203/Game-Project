using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponHolder))]
[RequireComponent(typeof(PlayerController))]
public class Player : Actor
{

    public WeaponHolder weaponHolder { get; private set; }
    public PlayerController controller { get; private set; }

    public bool pickupWeapons { get; private set; }

    protected override void Awake()
    {
        weaponHolder = this.GetComponent<WeaponHolder>();
        controller = this.GetComponent<PlayerController>();
    }

}
