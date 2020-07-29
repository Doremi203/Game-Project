using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirearmWeapon : CooldownWeapon
{

    // Основной класс всех огнестрельных оружий.
    [SerializeField] private float bulletSpreadingMultiplier;
    [SerializeField] private int bulletsCount = 1;
    [SerializeField] private float bulletsSpeed = 1000f;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private BulletBase bulletPrefab;
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;

    protected override void OnShoot()
    {
        // Оставил на случай, если будем удалять перезарядку.
        if (owner.ammoContainer.SpendAmmo(ammoType, 1))
        {
            for (int i = 0; i < bulletsCount; i++)
            {
                Vector3 spreadOffset = transform.right * Random.Range(-bulletSpreadingMultiplier, bulletSpreadingMultiplier);
                Vector3 bulletSpawnPosition = transform.position + transform.forward * 0.5f;
                BulletBase newBullet = Instantiate(bulletPrefab, bulletSpawnPosition, transform.rotation);
                newBullet.Setup(this, transform.forward + spreadOffset, bulletsSpeed, damage, damageType);
            }
        }
    }

    // Можем ли мы стрелять?
    protected override bool CanUse()
    {
        // Можем. Почему нет?
        return true;
    }

}
