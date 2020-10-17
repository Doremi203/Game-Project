using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmoContainer : WeaponComponent
{

    public int CurrentAmmo { get; private set; }

    [SerializeField] private int startingAmmoMin;
    [SerializeField] private int startingAmmoMax;

    public override bool CanShoot() => CurrentAmmo > 0;

    public override void OnShoot()
    {
        if (!weapon.WeaponHolder.InfinityAmmo) CurrentAmmo--;
    }

    public override bool CanPickup() => CurrentAmmo > 0;

    private void Start() => CurrentAmmo = Random.Range(startingAmmoMin, startingAmmoMax + 1);

}