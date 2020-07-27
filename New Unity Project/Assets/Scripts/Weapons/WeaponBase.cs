using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{

    [SerializeField] private string displayName; 

    protected bool isUsing;
    protected Player owner;

    public void SetOwner(Player owner)
    {
        this.owner = owner;
    }

    public void Use(bool b)
    {
        if (CanUse() == false) return;
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