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

    // Я не уверен, что это должно быть тут, но пусть пока будет.
    private void Update()
    {
        if (weaponHolder.currentWeapon != null) weaponHolder.currentWeapon.Use(Input.GetMouseButton(0));
    }

}
