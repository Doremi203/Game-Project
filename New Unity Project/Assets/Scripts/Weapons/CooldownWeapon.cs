﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CooldownWeapon : WeaponBase
{

    [SerializeField] private float cooldown;
    [SerializeField] private bool isAutomatic;

    private float nextShootTime;

    protected override void OnUsingStart()
    {
        if (Time.time > nextShootTime)
        {
            nextShootTime = Time.time + cooldown;
            OnShoot();
        }
    }

    protected override void OnUsingEnd()
    {

    }

    private void Update()
    {
        if (isAutomatic && isUsing)
        {
            if(Time.time >= nextShootTime)
            {
                nextShootTime = Time.time + cooldown;
                OnShoot();
            }
        }
    }

    protected abstract void OnShoot();

}