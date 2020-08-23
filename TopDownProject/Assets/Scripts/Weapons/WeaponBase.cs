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
    public bool IsDrop { get; private set; } = true;
    public float NpcAttackDistance => npcAttackDistance;
    public float NpcAttackAngle => npcAttackAngle;
    public AnimationType AnimationType => animationType;

    [SerializeField] private string displayName;
    [SerializeField] private float cooldown;
    [SerializeField] private bool isAutomatic;
    [SerializeField] private float soundEventDistance;
    [SerializeField] private float npcAttackDistance;
    [SerializeField] private float npcAttackAngle;
    [SerializeField] private AnimationType animationType;

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
        this.owner = owner;
        rb.detectCollisions = false;
        rb.isKinematic = true;
        IsDrop = false;
        this.infinityAmmo = infinityAmmo;
        OnEquiped.Invoke();
    }

    public virtual void Drop()
    {
        rb.detectCollisions = true;
        rb.isKinematic = false;
        //rb.AddForce(owner.transform.forward * 150f);
        this.owner = null;
        transform.parent = null;
        isUsing = false;
        IsDrop = true;
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
        return Time.time > nextShootTime;
    }

    // На будущее.
    public virtual bool CanPickup()
    {
        return true;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (isAutomatic && isUsing && CanUse())
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

#if UNITY_EDITOR

    protected virtual void OnDrawGizmos()
    {
        if (!owner) return;
        Handles.DrawWireDisc(owner.transform.position, owner.transform.up, soundEventDistance);
    }

#endif

}