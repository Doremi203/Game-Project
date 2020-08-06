﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirearmWeapon : WeaponBase
{

    // Основной класс всех огнестрельных оружий.

    public AmmoType AmmoType => ammoType;

    [SerializeField] private float bulletSpreadingMultiplier;
    [SerializeField] private int bulletsCount = 1;
    [SerializeField] private float bulletsSpeed = 1000f;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private BulletBase bulletPrefab;
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;

    protected override void OnShoot()
    {
        base.OnShoot();
        if (weaponHolder.ammoContainer.SpendAmmo(ammoType, 1))
        {
            for (int i = 0; i < bulletsCount; i++)
            {
                Vector3 spreadOffset = owner.transform.right * Random.Range(-bulletSpreadingMultiplier, bulletSpreadingMultiplier);
                Vector3 bulletSpawnPosition = owner.eyesPosition + owner.transform.forward * 1f;
                BulletBase newBullet = Instantiate(bulletPrefab, bulletSpawnPosition, transform.rotation);
                newBullet.Setup(this, owner.transform.forward + spreadOffset, bulletsSpeed, damage, damageType);
            }
        }
    }

}