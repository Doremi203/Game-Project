using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponBase : MonoBehaviour
{

    // Основной класс оружия.

    [SerializeField] private string displayName;
    public string DisplayName => displayName;

    protected bool isUsing;
    protected Actor owner;
    public Actor Owner => owner;

    protected WeaponHolder weaponHolder;
    public WeaponHolder WeaponHolder => weaponHolder;

    public UnityEvent OnUsingStartEvent;

    public void SetOwner(WeaponHolder weaponHolder, Actor owner)
    {
        this.owner = owner;
        this.weaponHolder = weaponHolder;
    }

    public void Use(bool b)
    {
        if (CanUse() == false)
        {
            isUsing = false;
            OnUsingEnd();
            return;
        }
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

    protected abstract void OnUsingStart();
    protected abstract void OnUsingEnd();
    protected abstract bool CanUse();

}