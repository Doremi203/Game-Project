using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{

    // Основной класс оружия.

    [SerializeField] private string displayName; 

    protected bool isUsing;
    protected WeaponHolder owner;

    public void SetOwner(WeaponHolder owner)
    {
        this.owner = owner;
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
            }
        }
        isUsing = b;
    }

    protected abstract void OnUsingStart();
    protected abstract void OnUsingEnd();
    protected abstract bool CanUse();

}