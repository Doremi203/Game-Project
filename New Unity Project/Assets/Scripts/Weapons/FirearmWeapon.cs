using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirearmWeapon : CooldownWeapon
{

    [SerializeField] private AmmoType ammoType;
    [SerializeField] private BulletBase bulletPrefab;

    protected override void OnShoot()
    {
        if (owner.ammoContainer.SpendAmmo(ammoType, 1))
        {
            BulletBase newBullet = Instantiate(bulletPrefab, transform.position + transform.forward * 0.5f, transform.rotation);
            newBullet.Setup(this, 1000f);
        }
    }

    protected override bool CanUse()
    {
        return true;
    }

}
