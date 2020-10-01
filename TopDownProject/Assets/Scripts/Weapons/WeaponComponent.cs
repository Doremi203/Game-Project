using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponComponent : MonoBehaviour
{

    protected Weapon weapon;

    protected virtual void Awake() => weapon = this.GetComponent<Weapon>();
    protected virtual bool CanShoot() => true;
    protected virtual void OnShoot() { }
    protected virtual void OnDroped() { }
    protected virtual void OnEquiped() { }
    protected virtual void DrawDebugs() { }

}