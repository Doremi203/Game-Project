using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectilesLauncher : MonoBehaviour, IWeaponComponent
{

    public float BulletSpeed => bulletsSpeed;

    [SerializeField] private float bulletSpreadingMultiplier;
    [SerializeField] private int bulletsCount = 1;
    [SerializeField] private float bulletsSpeed = 1000f;
    [SerializeField] private ProjectileBase bulletPrefab;
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;

    public void OnShoot(Weapon weapon)
    {
        Actor _owner = weapon.Owner;
        for (int i = 0; i < bulletsCount; i++)
        {
            Vector3 _spreadOffset = _owner.transform.right * Random.Range(-bulletSpreadingMultiplier, bulletSpreadingMultiplier);
            Vector3 _verticalOffset = _owner.transform.up * Random.Range(-bulletSpreadingMultiplier, bulletSpreadingMultiplier);
            Vector3 _force = (_owner.transform.forward + _spreadOffset + _verticalOffset) * bulletsSpeed;

            ProjectileBase _newBullet = Instantiate(bulletPrefab, _owner.eyesPosition, _owner.transform.rotation);
            _newBullet.Setup(_owner, damage, damageType);

            _newBullet.Rigidbody.AddForce(_force, ForceMode.VelocityChange);
        }
    }

    public bool IsReadyToShoot(Weapon weapon) => true;

    public void OnDroped(Weapon weapon) { }

    public void DrawDebug(Weapon weapon) { }

}