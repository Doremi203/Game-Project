using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public abstract class WeaponBase : MonoBehaviour
{

    // Основной класс оружия.
    public Actor Owner => owner;
    public string DisplayName => displayName;
    public float NpcAttackDistance => npcAttackDistance;
    public float NpcAttackAngle => npcAttackAngle;
    public WeaponAnimationType AnimationType => animationType;
    public WeaponState State { get; private set; }

    [SerializeField] private string displayName;
    [SerializeField] private float cooldown;
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private float soundEventDistance;
    [SerializeField] private float npcAttackDistance;
    [SerializeField] private float npcAttackAngle;
    [SerializeField] private WeaponAnimationType animationType;

    // Может лучше переименовать и сделать просто ShootEvent.
    public UnityEvent OnUsingStartEvent;
    public UnityEvent OnShootEvent;
    public UnityEvent OnUsingEndEvent;
    public UnityEvent OnDropped;
    public UnityEvent OnEquiped;

    protected bool isUsing;
    protected Actor owner;
    protected bool infinityAmmo;

    private float nextShootTime;
    private Rigidbody rb;

    public virtual void Pickup(Actor owner, bool infinityAmmo)
    {
        ChangeState(WeaponState.Equiped);
        this.owner = owner;

        this.infinityAmmo = infinityAmmo;

        OnEquiped.Invoke();
    }

    public virtual void Drop()
    {
        ChangeState(WeaponState.Drop);

        this.owner = null;
        transform.parent = null;

        isUsing = false;

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

    public virtual bool CanUse() => Time.time > nextShootTime;

    public virtual bool CanPickup() => State == WeaponState.Drop;

    protected virtual void Awake() => rb = GetComponent<Rigidbody>();

    protected virtual void Update()
    {
        if (weaponType == WeaponType.Automatic && isUsing && CanUse())
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

    protected virtual void OnUsingEnd() => OnUsingEndEvent.Invoke();

    private void ChangeState(WeaponState state)
    {
        State = state;
        switch (state)
        {   
            case WeaponState.Drop:

                rb.isKinematic = false;
                break;

            case WeaponState.Equiped:

                rb.isKinematic = true;
                break;

            default:

                break;
        }
    }

#if UNITY_EDITOR

    protected virtual void OnDrawGizmos()
    {
        if (!owner) return;
        Handles.DrawWireDisc(owner.transform.position, owner.transform.up, soundEventDistance);
    }

#endif

}