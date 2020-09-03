using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public abstract class AbilityBase : MonoBehaviour
{

    public float Cooldown => cooldown;

    [SerializeField] private float cooldown;

    protected Actor owner;

    private float nextAvaliableTime;

    public void Use()
    {
        if (CanUse() == false) return;
        nextAvaliableTime = Time.time + cooldown;
        OnUse();
    }

    protected virtual void Awake() => owner = this.GetComponent<Actor>();

    protected virtual bool CanUse() => Time.time >= nextAvaliableTime;

    protected abstract void OnUse();

}