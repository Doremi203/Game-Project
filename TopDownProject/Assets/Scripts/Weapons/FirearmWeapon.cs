using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirearmWeapon : WeaponBase
{

    // Основной класс всех огнестрельных оружий.

    public AmmoType AmmoType => ammoType;
    public int CurrentAmmo { get; private set; }

    [SerializeField] private float bulletSpreadingMultiplier;
    [SerializeField] private int bulletsCount = 1;
    [SerializeField] private float bulletsSpeed = 1000f;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int startingAmmoMin;
    [SerializeField] private int startingAmmoMax;
    [SerializeField] private ProjectileBase bulletPrefab;
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;

    protected override void Awake()
    {
        base.Awake();
        CurrentAmmo = Random.Range(startingAmmoMin, startingAmmoMax + 1);
    }

    public override bool CanUse() => base.CanUse() && CurrentAmmo > 0;

    protected override void OnShoot()
    {
        if (!infinityAmmo) CurrentAmmo--;

        for (int i = 0; i < bulletsCount; i++)
        {
            Vector3 _spreadOffset = owner.transform.right * Random.Range(-bulletSpreadingMultiplier, bulletSpreadingMultiplier);

            ProjectileBase _newBullet = Instantiate(bulletPrefab, owner.eyesPosition, Quaternion.identity);
            _newBullet.Setup(owner, owner.transform.forward + _spreadOffset, bulletsSpeed, damage, damageType);
        }

        base.OnShoot();
    }

}