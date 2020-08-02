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
        Collider[] hits = Physics.OverlapSphere(owner.transform.position, attackRadius);
        foreach (var item in hits)
        {
            Actor actor = item.GetComponent<Actor>();
            if (!actor) continue;

            if (actor == owner) continue;
            if (actor.Team == owner.Team) continue;

            if (Vector3.Angle(owner.transform.forward, actor.transform.position) > attackAngle) continue;

            actor.ApplyDamage(owner, damage, damageType);
        }
    }

    private void OnDrawGizmos()
    {
        if (!owner) return;
        Gizmos.DrawWireSphere(owner.transform.position, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(owner.transform.position, owner.transform.position + Quaternion.AngleAxis(-attackAngle / 2, Vector3.up) * owner.transform.forward * attackRadius);
        Gizmos.DrawLine(owner.transform.position, owner.transform.position + Quaternion.AngleAxis(attackAngle / 2, Vector3.up) * owner.transform.forward * attackRadius);
    }

}