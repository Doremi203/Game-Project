using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{

    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;
    [SerializeField] private float attackDelay = 0.05f;
    [SerializeField] private LayerMask layerMask;

    public override void Drop()
    {
        base.Drop();
        this.StopAllCoroutines();
    }

    protected override void OnShoot()
    {
        base.OnShoot();
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackDelay);
        Collider[] hits = Physics.OverlapSphere(owner.transform.position, attackRadius);
        foreach (var item in hits)
        {
            Actor _targetActor = item.GetComponent<Actor>();
            if (!_targetActor) continue;

            if (_targetActor == owner) continue;
            if (_targetActor.Team == owner.Team) continue;

            RaycastHit _hit;
            if (Physics.Linecast(owner.eyesPosition, _targetActor.eyesPosition, out _hit, layerMask))
            {
                if (_hit.transform != _targetActor.transform) continue;
            }

            Vector3 targetDir = _targetActor.transform.position - owner.transform.position;
            float angle = Vector3.Angle(targetDir, owner.transform.forward);

            if (angle > attackAngle) continue;

            _targetActor.ApplyDamage(owner, damage, damageType);
        }
    }

#if UNITY_EDITOR

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (!owner) return;
        Gizmos.DrawWireSphere(owner.transform.position, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(owner.transform.position, owner.transform.position + Quaternion.AngleAxis(-attackAngle, Vector3.up) * owner.transform.forward * attackRadius);
        Gizmos.DrawLine(owner.transform.position, owner.transform.position + Quaternion.AngleAxis(attackAngle, Vector3.up) * owner.transform.forward * attackRadius);
    }

#endif

}