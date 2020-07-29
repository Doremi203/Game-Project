using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirearmWeapon : CooldownWeapon
{

    // Основной класс всех огнестрельных оружий.

    [SerializeField] private float bulletSpreadingMultiplier;
    [SerializeField] private int bulletsCount = 1;
    [SerializeField] private float bulletsSpeed = 1000f;
    [SerializeField] private int magCapacity = 10;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private BulletBase bulletPrefab;
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;

    private int ammoInMagazin;

    protected override void OnShoot()
    {
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

    // Это для перезарядки. Потом доделаю.
    protected override bool CanUse()
    {
        return true;
        if (ammoInMagazin > 0)
        {
            return true;
        }
        else
        {
            Reload();
        }
        return false;
    }

    // Не используется.
    protected virtual void Reload()
    {
        Debug.Log("Reloading...");
        if (owner.ammoContainer.GetAmountOfAmmo(ammoType) <= 0) return;

        if (owner.ammoContainer.SpendAmmo(ammoType, ammoInMagazin))
        {
            ammoInMagazin = magCapacity;
        }
        else
        {
            int i = owner.ammoContainer.GetAmountOfAmmo(ammoType);
            owner.ammoContainer.SpendAmmo(ammoType, i);
            ammoInMagazin = i;
        }
    }

}
