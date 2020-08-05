using System.Collections;
using System.Collections.Generic;
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

    public UnityEvent OnUsingStartEvent;
    public UnityEvent OnShootEvent;

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
            if(b == false)
            {
                OnUsingEnd();
            }
        }
        else
        {
            if (b == true)
            {
                OnUsingStart();
                OnUsingStartEvent.Invoke();
            }
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
        OnShootEvent.Invoke();
    }

    protected virtual void OnUsingStart()
    {
        if (Time.time > nextShootTime)
        {
            nextShootTime = Time.time + cooldown;
            OnShoot();
        }
    }

    protected virtual void OnUsingEnd() { }

}