using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<float> { }

public class HealthComponent : MonoBehaviour
{

    [SerializeField]
    private int startHealth = 1;

    private float health;

    public IntEvent OnHealthChanged;

    public void ApplyDamage(float damage)
    {
        health -= damage;
        OnHealthChanged.Invoke(health);
    }

    public void Heal(float heal)
    {
        health += heal;
        OnHealthChanged.Invoke(health);
    }

    private void Awake()
    {
        health = startHealth;
    }

}