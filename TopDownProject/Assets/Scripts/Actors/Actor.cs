using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
[RequireComponent(typeof(HealthComponent))]
public class Actor : MonoBehaviour
{

    public static Action<Actor> ActorDied;

    public Vector3 EyesPosition => transform.position + eyesOffset;
    public Team Team => team;
    public Hitbox[] Hitboxes { get; private set; }
    public HealthComponent HealthComponent { get; private set; }

    [SerializeField] private Vector3 eyesOffset;
    [SerializeField] private Team team;

    protected virtual void Awake()
    {
        HealthComponent = GetComponent<HealthComponent>();
        HealthComponent.Died += (DamageInfo info) => ActorDied.Invoke(this);
        Hitboxes = GetComponentsInChildren<Hitbox>(true);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(EyesPosition, EyesPosition + transform.forward * 2f);
    }

}