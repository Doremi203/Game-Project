using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponBase : MonoBehaviour
{

    // Основной класс оружия.
    public Actor Owner => owner;
    public WeaponHolder WeaponHolder => weaponHolder;
    public string DisplayName => displayName;

    [SerializeField] private string displayName;
    [SerializeField] private float cooldown;
    [SerializeField] private bool isAutomatic;
    [SerializeField] private float soundEventDistance;

    public UnityEvent OnUsingStartEvent;
    public UnityEvent OnShootEvent;
    public UnityEvent OnUsingEndEvent;

    protected bool isUsing;
    protected Actor owner;
    protected WeaponHolder weaponHolder;

    private float nextShootTime;

    public void SetOwner(WeaponHolder weaponHolder, Actor owner)
    {
        this.owner = owner;
        this.weaponHolder = weaponHolder;
    }

    public void Use(bool b)
    {
        /*
        if (CanUse() == false)
        {
            isUsing = false;
            OnUsingEnd();
            return;
        }
        */
        if (isUsing)
        {
            if(b == false) OnUsingEnd();
        }
        else
        {
            if (b == true) OnUsingStart();
        }
        isUsing = b;
    }

    public virtual bool CanUse()
    {
        return Time.time > nextShootTime;
    }

    private void Update()
    {
        if (isAutomatic && isUsing)
        {
            if (Time.time >= nextShootTime)
            {
                nextShootTime = Time.time + cooldown;
                OnShoot();
            }
        }
    }

    protected virtual void OnShoot() 
    {
        SoundEventGenerator.GenerateSoundEvent(owner, owner.transform.position, soundEventDistance);
        OnShootEvent.Invoke();
    }

    protected virtual void OnUsingStart()
    {
        if (Time.time > nextShootTime)
        {
            nextShootTime = Time.time + cooldown;
            OnShoot();
        }
        OnUsingStartEvent.Invoke();
    }

    protected virtual void OnUsingEnd() 
    {
        OnUsingEndEvent.Invoke();
    }

    protected virtual void OnDrawGizmos()
    {
        if (!owner) return;
        Handles.DrawWireDisc(owner.transform.position, owner.transform.up, soundEventDistance);
    }

}