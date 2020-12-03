using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<float> { }

public class HealthComponent : MonoBehaviour
{

    public float Health { get; private set; }

    [SerializeField] private int startHealth = 1;
    [SerializeField] private bool debugInvicibility;

    public IntEvent OnHealthChanged;

    public void ApplyDamage(float damage)
    {
        if(debugInvicibility == false) Health -= damage;
        OnHealthChanged.Invoke(Health);
    }

    public void Heal(float heal)
    {
        Health += heal;
        OnHealthChanged.Invoke(Health);
    }

    private void Awake() => Health = startHealth;

}