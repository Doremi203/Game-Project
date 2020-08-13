using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : WeaponBase
{

    [SerializeField] private Collider shieldCollision;

    public override void Pickup(Actor owner, bool infinityAmmo)
    {
        base.Pickup(owner, infinityAmmo);
        shieldCollision.enabled = true;
    }

    public override void Drop()
    {
        base.Drop();
        shieldCollision.enabled = false;
    }

}