using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletBase : MonoBehaviour, IDamageable
{

    protected Rigidbody rb;
    protected WeaponBase owner;

    public void ApplyDamage(float damage, DamageType damageType)
    {
        Destroy(this.gameObject);
    }

    public void Setup(WeaponBase owner, Vector3 pushDirection, float pushForce)
    {
        this.owner = owner;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(pushDirection * pushForce);
        Destroy(this.gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.transform.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.ApplyDamage(10, null);
        }
        Destroy(this.gameObject);
    }

}