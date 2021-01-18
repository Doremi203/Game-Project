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
    [SerializeField] private LayerMask raycastLayer;

    private float nextAccurateBulletTime;

    public override void OnShoot()
    {
        Actor owner = weapon.Owner;

        for (int i = 0; i < bulletsCount; i++)
        {
            Vector3 spreadOffset = (firstBulletIgnoreSpread && Time.time > nextAccurateBulletTime) ?
                Vector3.zero :
                owner.transform.right * Random.Range(-spreadMultiplier, spreadMultiplier);

            Vector3 verticalOffset = (firstBulletIgnoreSpread && Time.time > nextAccurateBulletTime) ?
                Vector3.zero :
                owner.transform.up * Random.Range(-verticalSpreadMultiplier, verticalSpreadMultiplier);

            Ray bulletRay = new Ray(owner.EyesPosition, owner.transform.forward + spreadOffset + verticalOffset);
      
            if (Physics.Raycast(bulletRay, out RaycastHit hit, 2f, raycastLayer))
            {
                Debug.DrawLine(bulletRay.origin, hit.point, Color.green, 5f);
                Debug.Log(hit.transform.gameObject.name);
                DamageInfo info = new DamageInfo(owner, gameObject, owner.transform.forward, damage, DamageType.Bullet);
                hit.transform.GetComponent<IDamageable>()?.ApplyDamage(info);
            }
            else
            {
                Debug.DrawLine(bulletRay.origin, bulletRay.origin + bulletRay.direction * 2f, Color.red, 5f);
                ProjectileBase newBullet = Instantiate(bulletPrefab, owner.EyesPosition + bulletRay.direction * 2f, owner.transform.rotation);
                newBullet.Setup(owner, damage);

                Vector3 force = (owner.transform.forward + spreadOffset + verticalOffset) * Random.Range(minBulletSpeed, bulletsSpeed);
                newBullet.Rigidbody.AddForce(force, ForceMode.VelocityChange);
            }
        }

        nextAccurateBulletTime = Time.time + 0.2f;
    }

}