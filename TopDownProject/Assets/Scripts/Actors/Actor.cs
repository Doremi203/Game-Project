using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(Collider))]
public class Actor : MonoBehaviour, IDamageable
{

    [HideInInspector] public Quaternion desiredRotation;

    public Action<Actor, float, DamageType, Vector3> OnDamageTaken;
    public UnityEvent OnDeath;

    public Vector3 eyesPosition => transform.position + eyesOffset;
    public Team Team => team;
    public float RotationSpeed => rotationSpeed;
    public bool IsDead { get; private set; }
    public Collider Hitbox { get; private set; }

    [Header("Actor Settings")]
    [SerializeField] private bool noDelayRotation;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector3 eyesOffset;
    [SerializeField] private bool shouldDestroy;
    [SerializeField] private float destroyDelay = 1f;
    [SerializeField] private Team team;

    private HealthComponent healthComponent;

    public virtual void ApplyDamage(Actor damageCauser, float damage, DamageType damageType, Vector3 damageDirection)
    {
        healthComponent.ApplyDamage(damage);
        OnDamageTaken?.Invoke(damageCauser, damage, damageType, damageDirection);
    }

    protected virtual void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.OnHealthChanged.AddListener(HealthChanged);
        Hitbox = GetComponent<Collider>();
    }

    protected virtual void Update()
    {
        if (IsDead) return;
        if (transform.rotation == desiredRotation) return;
        if (noDelayRotation)
            this.transform.rotation = desiredRotation;
        else
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }

    protected virtual void Death()
    {
        IsDead = true;
        OnDeath.Invoke();
        if(shouldDestroy) Destroy(this.gameObject, destroyDelay);
    }

    protected virtual void HealthChanged(float health)
    {
        if (IsDead) return;
        if (health <= 0) Death();
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyesPosition, eyesPosition + transform.forward * 2f);
    }

}