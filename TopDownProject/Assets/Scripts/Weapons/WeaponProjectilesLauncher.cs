using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectilesLauncher : WeaponComponent
{

    public float BulletSpeed => bulletsSpeed;

    [SerializeField] private bool firstBulletIgnoreSpread;
    [SerializeField] private float spreadMultiplier;
    [SerializeField] private float verticalSpreadMultiplier;
    [SerializeField] private int bulletsCount = 1;
    [SerializeField] private float minBulletSpeed;
    [SerializeField] private float bulletsSpeed = 1000f;
    [SerializeField] private ProjectileBase bulletPrefab;
    [SerializeField] private float damage;

    private float nextAccurateBulletTime;

    public override void OnShoot()
    {
        Actor owner = weapon.Owner;

        for (int i = 0; i < bulletsCount; i++)
        {
            Vector3 spreadOffset = (firstBulletIgnoreSpread && Time.time >= nextAccurateBulletTime) ?
                Vector3.zero :
                owner.transform.right * Random.Range(-spreadMultiplier, spreadMultiplier);

            Vector3 verticalOffset = (firstBulletIgnoreSpread && Time.time >= nextAccurateBulletTime) ?
                Vector3.zero :
                owner.transform.up * Random.Range(-verticalSpreadMultiplier, verticalSpreadMultiplier);

            Vector3 force = (owner.transform.forward + spreadOffset + verticalOffset) * Random.Range(minBulletSpeed, bulletsSpeed);

            ProjectileBase newBullet = Instantiate(bulletPrefab, owner.eyesPosition, owner.transform.rotation);
            newBullet.Setup(owner, damage);

            newBullet.Rigidbody.AddForce(force, ForceMode.VelocityChange);
        }

        nextAccurateBulletTime = Time.time + 0.2f;
    }

}