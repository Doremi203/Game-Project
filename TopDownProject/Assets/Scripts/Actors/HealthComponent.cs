using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEventArgs : EventArgs
{

    private DamageInfo fatalDamageInfo;

    public DeathEventArgs(DamageInfo fatalDamageInfo)
    {
        this.fatalDamageInfo = fatalDamageInfo;
    }

}

public class DamageEventArgs : EventArgs
{

    private DamageInfo info;
    private float healthBefore;
    private float currentHealth;

    public DamageEventArgs(DamageInfo info, float healthBefore, float currentHealth)
    {
        this.info = info;
        this.healthBefore = healthBefore;
        this.currentHealth = currentHealth;
    }

}

public delegate void OnDeath(DamageInfo lastDamageInfo);
public delegate void OnDamaged(DamageInfo info, float healthBefore, float currentHealth);

public class HealthComponent : MonoBehaviour, IDamageable
{

    public event OnDeath Died;
    public event OnDamaged Damaged;

    public float Health { get; private set; }
    public bool IsDead { get; private set; }

    [SerializeField] private int startHealth = 1;
    [SerializeField] private bool debugInvicibility;

    public bool ApplyDamage(DamageInfo info)
    {
        if (debugInvicibility) return false;
        if (IsDead) return false;
        float healthBefore = Health;
        Health = Mathf.Min(0, Health - info.DamageAmount);
        Damaged?.Invoke(info, healthBefore, Health);
        if (Health <= 0)
        {
            IsDead = true;
            Died?.Invoke(info);
        }
        return true;
    }

    private void Start()
    {
        Health = startHealth;
    }

}