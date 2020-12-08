using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour, IDamageable
{

    public Collider Collider { get; private set; }

    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private float damageMultiplier = 1f;

    public bool ApplyDamage(DamageInfo info)
    {
        info.DamageAmount = info.DamageAmount * damageMultiplier;
        return healthComponent.ApplyDamage(info);
    }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        healthComponent.Died += (DamageInfo info) => Collider.enabled = false;
    }

}