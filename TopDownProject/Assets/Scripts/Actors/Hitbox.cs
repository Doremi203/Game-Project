using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour, IDamageable
{

    public Collider Collider { get; private set; }

    [SerializeField] private Actor actor;
    [SerializeField] private float damageMultiplier = 1f;

    public bool ApplyDamage(DamageInfo info)
    {
        info.DamageAmount *= damageMultiplier;
        return ((IDamageable)actor).ApplyDamage(info);
    }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        actor.OnDeath += ActorDeath;
    }

    private void ActorDeath(DamageInfo obj) => Collider.enabled = false;

}