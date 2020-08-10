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

    [SerializeField] private string displayName;
    [SerializeField] private float cooldown;
    [SerializeField] private bool isAutomatic;
    [SerializeField] private float soundEventDistance;

    public UnityEvent OnUsingStartEvent;
    public UnityEvent OnShootEvent;
    public UnityEvent OnUsingEndEvent;

    protected bool isUsing;
    protected Actor owner;

    protected bool infinityAmmo;

    private float nextShootTime;
    private Rigidbody rb;

    public void Pickup(Actor owner, bool infinityAmmo)
    {
        this.owner = owner;
        rb.isKinematic = true;
        IsDrop = false;
        this.infinityAmmo = infinityAmmo;
    }

    public void Drop()
    {
        rb.isKinematic = false;
        rb.AddForce(owner.transform.forward * 150f);
        this.owner = null;
        transform.parent = null;
        isUsing = false;
        IsDrop = true;
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

    protected virtual void OnDrawGizmos()
    {
        if (!owner) return;
        Handles.DrawWireDisc(owner.transform.position, owner.transform.up, soundEventDistance);
    }

}