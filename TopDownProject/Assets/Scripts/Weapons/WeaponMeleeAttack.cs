using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeleeAttack : WeaponComponent
{

    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private float absoluteRadius = 0.5f;
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;
    [SerializeField] private float rigidbodyPushForse = 100f;
    [SerializeField] private float attackDelay = 0.05f;
    [SerializeField] private LayerMask layerMask;

    private bool rightAttack;

    public override void OnShoot() => StartCoroutine(Attack());

    public override void OnDropped()
    {
        rightAttack = false;
        StopAllCoroutines();
    }

    private IEnumerator Attack()
    {
        Actor owner = weapon.Owner;

        yield return new WaitForSeconds(attackDelay);

        Collider[] hits = Physics.OverlapSphere(owner.transform.position, attackRadius);
        foreach (var item in hits)
        {
            Rigidbody _targetRigidbody = item.GetComponent<Rigidbody>();
            if (_targetRigidbody)
            {
                Vector3 targetDir = _targetRigidbody.transform.position - owner.transform.position;
                float angle = Vector3.Angle(targetDir, owner.transform.forward);
                if (angle <= attackAngle)
                    _targetRigidbody.AddForce(owner.transform.forward * rigidbodyPushForse);
            }

            Actor _targetActor = item.GetComponent<Actor>();
            if (!_targetActor) continue;

            if (_targetActor == owner) continue;
            if (_targetActor.Team == owner.Team) continue;

            RaycastHit _hit;
            if (Physics.Linecast(owner.eyesPosition, _targetActor.eyesPosition, out _hit, layerMask))
            {
                if (_hit.transform != _targetActor.transform) continue;
            }

            float _distance = GameUtilities.GetDistance2D(owner.transform.position, _targetActor.transform.position);

            if (_distance > absoluteRadius)
            {
                Vector3 targetDir = _targetActor.transform.position - owner.transform.position;
                float angle = Vector3.Angle(targetDir, owner.transform.forward);
                if (angle > attackAngle) continue;
            }

            Vector3 damageDirection = owner.transform.forward + ((rightAttack ? owner.transform.right : -owner.transform.right) * 0.5f);
            damageDirection = damageDirection.normalized;

            DamageInfo info = new DamageInfo(owner, weapon.gameObject, damageDirection, damage, damageType);
            _targetActor.ApplyDamage(info);
        }

        rightAttack = !rightAttack;
    }

    public void DrawDebug()
    {
        return;
        if (weapon == null) return;
        Actor _owner = weapon.Owner;
        if (weapon.Owner == null) return;
        Gizmos.DrawWireSphere(_owner.transform.position, attackRadius);
        Gizmos.DrawWireSphere(_owner.transform.position, absoluteRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_owner.transform.position, _owner.transform.position + Quaternion.AngleAxis(-attackAngle, Vector3.up) * _owner.transform.forward * attackRadius);
        Gizmos.DrawLine(_owner.transform.position, _owner.transform.position + Quaternion.AngleAxis(attackAngle, Vector3.up) * _owner.transform.forward * attackRadius);
    }

}