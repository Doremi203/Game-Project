using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirearmWeapon : WeaponBase
{

    // Основной класс всех огнестрельных оружий.

    public AmmoType AmmoType => ammoType;
    public int CurrentAmmo => currentAmmo;

    [SerializeField] private float bulletSpreadingMultiplier;
    [SerializeField] private int bulletsCount = 1;
    [SerializeField] private float bulletsSpeed = 1000f;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int startingAmmo;
    [SerializeField] private BulletBase bulletPrefab;
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;

    private int currentAmmo;

    protected override void Awake()
    {
        base.Awake();
        currentAmmo = startingAmmo;
    }

    public override bool CanUse()
    {
        return base.CanUse() && currentAmmo > 0;
    }

    protected override void OnShoot()
    {
        if(!infinityAmmo) currentAmmo--;

        for (int i = 0; i < bulletsCount; i++)
        {
            Vector3 spreadOffset = owner.transform.right * Random.Range(-bulletSpreadingMultiplier, bulletSpreadingMultiplier);
            Vector3 bulletSpawnPosition = owner.eyesPosition;
            BulletBase newBullet = Instantiate(bulletPrefab, bulletSpawnPosition, transform.rotation);
            newBullet.Setup(this, owner.transform.forward + spreadOffset, bulletsSpeed, damage, damageType);
        }

        base.OnShoot();
    }

}