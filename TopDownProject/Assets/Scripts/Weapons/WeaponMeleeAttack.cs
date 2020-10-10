﻿using System.Collections;
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

    public override void OnShoot() => StartCoroutine(Attack());

    public override void OnDropped() => StopAllCoroutines();

    private IEnumerator Attack()
    {
        Actor _owner = weapon.Owner;

        yield return new WaitForSeconds(attackDelay);

        Collider[] hits = Physics.OverlapSphere(_owner.transform.position, attackRadius);
        foreach (var item in hits)
        {
            Rigidbody _targetRigidbody = item.GetComponent<Rigidbody>();
            if (_targetRigidbody)
            {
                Vector3 targetDir = _targetRigidbody.transform.position - _owner.transform.position;
                float angle = Vector3.Angle(targetDir, _owner.transform.forward);
                if (angle <= attackAngle)
                    _targetRigidbody.AddForce(_owner.transform.forward * rigidbodyPushForse);
            }

            Actor _targetActor = item.GetComponent<Actor>();
            if (!_targetActor) continue;

            if (_targetActor == _owner) continue;
            if (_targetActor.Team == _owner.Team) continue;

            RaycastHit _hit;
            if (Physics.Linecast(_owner.eyesPosition, _targetActor.eyesPosition, out _hit, layerMask))
            {
                if (_hit.transform != _targetActor.transform) continue;
            }

            float _distance = GameUtilities.GetDistance2D(_owner.transform.position, _targetActor.transform.position);

            if (_distance > absoluteRadius)
            {
                Vector3 targetDir = _targetActor.transform.position - _owner.transform.position;
                float angle = Vector3.Angle(targetDir, _owner.transform.forward);
                if (angle > attackAngle) continue;
            }

            DamageInfo info = new DamageInfo(_owner, weapon.gameObject, _owner.transform.forward, damage, damageType);
            _targetActor.ApplyDamage(info);
        }
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