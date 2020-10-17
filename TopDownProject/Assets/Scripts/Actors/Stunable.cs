using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Actor))]
public class Stunable : MonoBehaviour
{

    public UnityEvent StunnedEvent;
    public UnityEvent UnstunnedEvent;

    public bool IsStunned { get; private set; }

    private Actor owner;
    private float wakeUpTime;

    public void Stun(float time)
    {
        if (owner.IsDead) return;
        wakeUpTime = IsStunned ? wakeUpTime + time : Time.time + time;
        IsStunned = true;
        StunnedEvent.Invoke();
    }

    private void Awake() => owner = GetComponent<Actor>();

    private void Update()
    {
        if (!IsStunned) return;
        if (Time.time >= wakeUpTime)
        {
            IsStunned = false;
            if (owner.IsDead) return;
            UnstunnedEvent.Invoke();
        }
    }

}