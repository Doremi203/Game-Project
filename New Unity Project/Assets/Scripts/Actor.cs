using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HealthComponent))]
public class Actor : MonoBehaviour, IDamageable
{

    public delegate void ActorDeath(Actor actor);
    public event ActorDeath OnActorDeath;
    public UnityEvent OnDeath;

    private bool isDead;
    public bool IsDead => isDead;

    [SerializeField] private Vector3 eyesOffset;
    protected Vector3 eyesPosition => transform.position + eyesOffset;

    [SerializeField] private float destroyDelay = 1f;

    [SerializeField] private Team team;
    public Team Team => team;

    private HealthComponent healthComponent;

    public virtual void ApplyDamage(Actor damageCauser, float damage, DamageType damageType)
    {
        healthComponent.ApplyDamage(damage);
    }

    protected virtual void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.OnHealthChanged.AddListener(HealthChanged);
    }

    protected virtual void Death()
    {
        isDead = true;
        OnDeath.Invoke();
        if (OnActorDeath != null) OnActorDeath(this);
        Destroy(this.gameObject, destroyDelay);
    }

    protected virtual void HealthChanged(float health)
    {
        if (isDead)
        {
            return;
        }
        else
        {
            if (health <= 0)
            {
                Death();
            }
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawLine(eyesPosition, eyesPosition + transform.forward);
    }

}