using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCooldown : WeaponComponent
{

    [SerializeField] private float cooldown;

    private float nextShootTime;

    public override bool CanShoot() => Time.time >= nextShootTime;

    public override void OnShoot() => nextShootTime = Time.time + cooldown;

}