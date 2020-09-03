using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rocket : ProjectileBase
{

    public UnityEvent OnExplosion;

    [SerializeField] private float rocketConstantForce = 1200f;
    [SerializeField] private float explosionRadius = 3.5f;

    public override void ApplyDamage(Actor damageCauser, float damage, DamageType damageType)
    {
        base.ApplyDamage(damageCauser, damage, damageType);
        Explosion();
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * rocketConstantForce * Time.fixedDeltaTime);
    }

    protected override void OnHit(Collider other)
    {
        base.OnHit(other);
        Explosion();
    }

    private void Explosion()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, explosionRadius);
        foreach (var item in hits)
        {
            Actor actor = item.GetComponent<Actor>();
            if (!actor) continue;
            actor.ApplyDamage(owner, damage, damageType);
        }

        OnExplosion.Invoke();
    }

}