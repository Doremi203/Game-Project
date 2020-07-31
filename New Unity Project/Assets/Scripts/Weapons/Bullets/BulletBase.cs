using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletBase : MonoBehaviour, IDamageable
{

    [SerializeField] private TrailRenderer trail;

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
        trail.startColor = owner.Owner.Team.bulletsColor;
        trail.endColor = owner.Owner.Team.bulletsColor;
        Destroy(this.gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<BulletBase>()) return;

        if (owner != null)
            if (other.transform == owner.Owner.transform) return;

        if (other.transform.GetComponent<Actor>()?.Team == owner.Owner.Team) return;

        IDamageable damageable = other.transform.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.ApplyDamage(damage, damageType);
        }

        Destroy(this.gameObject);
    }

}