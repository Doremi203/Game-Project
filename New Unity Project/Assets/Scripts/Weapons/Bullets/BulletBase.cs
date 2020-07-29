using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletBase : MonoBehaviour, IDamageable
{

    protected Rigidbody rb;
    protected WeaponBase owner;
    protected float damage;
    protected DamageType damageType;

    public void ApplyDamage(float damage, DamageType damageType)
    {
        Destroy(this.gameObject);
    }

    public void Setup(WeaponBase owner, Vector3 pushDirection, float pushForce, float damage, DamageType damageType)
    {
        this.owner = owner;
        this.damage = damage;
        this.damageType = damageType;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(pushDirection * pushForce);
        Destroy(this.gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.transform.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.ApplyDamage(damage, damageType);
        }
        Destroy(this.gameObject);
    }

}