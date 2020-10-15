using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Weapon : MonoBehaviour
{

    public Actor Owner => owner;
    public WeaponHolder WeaponHolder => weaponHolder;
    public string DisplayName => displayName;
    public float BonusAimDistance => bonusAimDistance;
    public WeaponAnimationType AnimationType => animationType;
    public WeaponNPCSettings NPCSettings => npcSettings;
    public bool IsUsing => isUsing;

    public WeaponState State { get; private set; }

    [SerializeField] private string displayName;
    [SerializeField] private float bonusAimDistance;
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private WeaponAnimationType animationType;
    [SerializeField] private WeaponNPCSettings npcSettings;

    public UnityEvent OnShootEvent;
    public UnityEvent OnDropped;
    public UnityEvent OnEquiped;

    protected bool isUsing;
    protected Actor owner;
    protected WeaponHolder weaponHolder;

    private float nextShootTime;
    private Rigidbody rb;
    private WeaponComponent[] weaponComponents;

    public virtual void Pickup(WeaponHolder weaponHolder)
    {
        ChangeState(WeaponState.Equiped);

        this.weaponHolder = weaponHolder;
        this.owner = weaponHolder.Owner;

        foreach (var item in weaponComponents) item.OnPickedUp();

        OnEquiped.Invoke();
    }

    public virtual void Drop()
    {
        ChangeState(WeaponState.Drop);

        this.weaponHolder = null;
        this.owner = null;

        isUsing = false;

        foreach (var item in weaponComponents) item.OnDropped();

        OnDropped.Invoke();
    }

    public void Use(bool b)
    {
        if (CanUse() == false)
        {
            if (isUsing)
            {
                isUsing = false;
                OnUsingEnd();
            }
            return;
        }

        if (isUsing)
        {
            if (b == false) OnUsingEnd();
        }
        else
        {
            if (b == true) OnUsingStart();
        }
        isUsing = b;
    }

    public virtual bool CanUse()
    {
        foreach (WeaponComponent item in weaponComponents)
            if (item.CanShoot() == false) return false;
        return true;
    }

    public virtual bool CanPickup()
    {
        foreach (WeaponComponent item in weaponComponents)
            if (item.CanPickup() == false) return false;
        return State == WeaponState.Drop;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        weaponComponents = GetComponents<WeaponComponent>();
    }

    protected virtual void Update()
    {
        if (weaponType == WeaponType.Automatic && isUsing && CanUse()) OnShoot();
    }

    protected virtual void OnShoot() 
    {
        foreach (var item in weaponComponents) item.OnShoot();
        OnShootEvent.Invoke();
    }

    protected virtual void OnUsingStart() => OnShoot();

    protected virtual void OnUsingEnd() { }

    private void ChangeState(WeaponState state)
    {
        State = state;
        switch (state)
        {   
            case WeaponState.Drop:

                rb.isKinematic = false;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                break;

            case WeaponState.Equiped:

                rb.isKinematic = true;
                rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                break;

            default:

                break;
        }
    }

}

[System.Serializable]
public struct WeaponNPCSettings
{

    public float AttackDistance;
    public float AttackAngle;
    public float FirstAttackDelay;
}