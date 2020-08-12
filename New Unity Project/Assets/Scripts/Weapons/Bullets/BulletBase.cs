using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletBase : MonoBehaviour, IDamageable
{

    [SerializeField] private TrailRenderer trail;
    [SerializeField] private float activationTime = 0.02f;

    protected Rigidbody rb;
    protected WeaponBase owner;
    protected float damage;
    protected DamageType damageType;
    protected Team team;

    protected float spawnTime;

    public virtual void ApplyDamage(Actor damageCauser, float damage, DamageType damageType)
    {
        //Destroy(this.gameObject);
    }

    public void Setup(WeaponBase owner, Vector3 pushDirection, float pushForce, float damage, DamageType damageType)
    {
        this.owner = owner;
        this.damage = damage;
        this.damageType = damageType;
        this.team = owner.Owner.Team;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(pushDirection * pushForce);
        trail.startColor = owner.Owner.Team.bulletsColor;
        trail.endColor = owner.Owner.Team.bulletsColor;
        Destroy(this.gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {

        // (other.transform.GetComponent<BulletBase>()) return;

        BulletBase bulletBase = other.transform.GetComponent<BulletBase>();
        if (bulletBase != null)
        {
            if (bulletBase.owner != null && bulletBase.owner.Owner != null)
            {
                if (team == bulletBase.owner.Owner.Team) return;
            }
        }

        if (owner != null && owner.Owner != null)
        {
            if (other.transform == owner.Owner.transform) return;
        }

        if (other.transform.GetComponent<Actor>()?.Team == team) return;

        IDamageable damageable = other.transform.GetComponent<IDamageable>();

        if (damageable != null) damageable.ApplyDamage(owner.Owner, damage, damageType);

        OnHit(other);

        Destroy(this.gameObject);
    }

    protected virtual void Awake()
    {
        spawnTime = Time.time;
        trail.enabled = false;
    }

    protected virtual void Update()
    {
        if (!trail.enabled && Time.time >= spawnTime + activationTime) trail.enabled = true;
    }

    protected virtual void OnHit(Collider other)
    {

    }

}