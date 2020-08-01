using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttack : IState
{

    private Turret turret;
    private WeaponHolder weaponHolder;

    public TurretAttack(Turret turret, WeaponHolder weaponHolder)
    {
        this.turret = turret;
        this.weaponHolder = weaponHolder;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
        weaponHolder.currentWeapon.Use(false);
    }

    public void Tick()
    {
        Vector3 relativePos = turret.Target.transform.position - turret.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        turret.desireRotation = rotation;

        weaponHolder.currentWeapon.Use(weaponHolder.currentWeapon.CanUse());
    }

}