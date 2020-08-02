using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : CooldownWeapon
{

    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;

    protected override void OnShoot()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius);
        foreach (var item in hits)
        {
            Actor actor = item.GetComponent<Actor>();
            if (!actor) continue;

            if (actor == owner) continue;
            if (actor.Team == owner.Team) continue;

            if (Vector3.Angle(transform.forward, actor.transform.position) > attackAngle) continue;

            actor.ApplyDamage(owner, damage, damageType);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(-attackAngle / 2, Vector3.up) * transform.forward * attackRadius);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(attackAngle / 2, Vector3.up) * transform.forward * attackRadius);
    }

}