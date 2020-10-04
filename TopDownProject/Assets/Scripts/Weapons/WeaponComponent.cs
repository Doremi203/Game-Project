using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponComponent : MonoBehaviour
{

    protected Weapon weapon;

    public virtual bool CanShoot() => true;
    public virtual bool CanPickup() => true;
    public virtual void OnShoot() { }
    public virtual void OnPickedUp() { }
    public virtual void OnDropped() { }
    public virtual void OnEquiped() { }
    public virtual void DrawDebugs() { }

    protected virtual void Awake() => weapon = this.GetComponent<Weapon>();

}