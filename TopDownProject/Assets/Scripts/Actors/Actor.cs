using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
[RequireComponent(typeof(HealthComponent))]
public class Actor : MonoBehaviour, IDamageable
{

    [HideInInspector] public Quaternion desiredRotation;

    public Action<DamageInfo> OnDamageTaken;
    public Action<DamageInfo> OnDeath;
    public UnityEvent DeathEvent;

    public Vector3 eyesPosition => transform.position + eyesOffset;
    public Team Team => team;
    public float RotationSpeed => rotationSpeed;
    public bool IsDead { get; private set; }
    public Hitbox[] Hitboxes { get; private set; }
    public DamageInfo LastDamageInfo { get; private set; }

    [Header("Actor Settings")]
    [SerializeField] private bool noDelayRotation;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector3 eyesOffset;
    [SerializeField] private bool shouldDestroy;
    [SerializeField] private float destroyDelay = 1f;
    [SerializeField] private Team team;

    private HealthComponent healthComponent;

    public virtual bool ApplyDamage(DamageInfo info)
    {
        if (IsDead) return false;
        healthComponent.ApplyDamage(info.DamageAmount);
        OnDamageTaken?.Invoke(info);
        LastDamageInfo = info;
        if (healthComponent.Health <= 0) Death();
        return true;
    }

    protected virtual void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        Hitboxes = GetComponentsInChildren<Hitbox>(true);
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
        OnDeath?.Invoke(LastDamageInfo);
        DeathEvent.Invoke();
        if(shouldDestroy) Destroy(this.gameObject, destroyDelay);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyesPosition, eyesPosition + transform.forward * 2f);
    }

}