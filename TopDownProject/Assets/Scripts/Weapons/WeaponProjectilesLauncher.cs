using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectilesLauncher : WeaponComponent
{

    public float BulletSpeed => bulletsSpeed;

    [SerializeField] private float spreadMultiplier;
    [SerializeField] private float verticalSpreadMultiplier;
    [SerializeField] private int bulletsCount = 1;
    [SerializeField] private float bulletsSpeed = 1000f;
    [SerializeField] private ProjectileBase bulletPrefab;
    [SerializeField] private float damage;

    public override void OnShoot()
    {
        Actor _owner = weapon.Owner;
        for (int i = 0; i < bulletsCount; i++)
        {
            Vector3 _spreadOffset = _owner.transform.right * Random.Range(-spreadMultiplier, spreadMultiplier);
            Vector3 _verticalOffset = _owner.transform.up * Random.Range(-verticalSpreadMultiplier, verticalSpreadMultiplier);
            Vector3 _force = (_owner.transform.forward + _spreadOffset + _verticalOffset) * bulletsSpeed;

            ProjectileBase _newBullet = Instantiate(bulletPrefab, _owner.eyesPosition, _owner.transform.rotation);
            _newBullet.Setup(_owner, damage);

            _newBullet.Rigidbody.AddForce(_force, ForceMode.VelocityChange);
        }
    }

}