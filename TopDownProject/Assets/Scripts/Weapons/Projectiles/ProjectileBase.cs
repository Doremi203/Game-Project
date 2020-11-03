using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ProjectileBase : MonoBehaviour
{

    public Collider Hitbox { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    [SerializeField] private GameObject visuals;
    [SerializeField] private float enableVisualsDelayMin = 0.02f;
    [SerializeField] private float enableVisualsDelayMax = 0.02f;
    [SerializeField] private SurfaceType defaultSurfaceType;
    [SerializeField] private bool spawnParticlesOnHit = true;
    [SerializeField] private bool spawnDecailsOnHit = true;
    [SerializeField] private DamageType damageType;
    [SerializeField] private bool destroyOnHit = true;
    [SerializeField] private bool destroyByTime = true;
    [SerializeField] private DestroyBehaviour destroyBehaviour;

    private float activationTime;

    protected float damage;
    protected Team team;
    protected float spawnTime;
    protected Actor owner;

    public void Setup(Actor owner, float damage)
    {
        this.owner = owner;
        this.team = owner.Team;
        this.damage = damage;

        foreach (var item in owner.Hitboxes)
            Physics.IgnoreCollision(Hitbox, item.Collider);

        if (destroyByTime) Destroy(this.gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DamageInfo info = new DamageInfo(owner, this.gameObject, this.transform.forward, damage, damageType);

        IDamageable damageable = collision.transform.GetComponent<IDamageable>();
        bool isDamageSet = false;

        if (damageable != null)
            isDamageSet = damageable.ApplyDamage(info);

        Vector3 _hitPosition = collision.GetContact(0).point;
        Quaternion _effectRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);
        Quaternion _decalRotation = Quaternion.LookRotation(-collision.GetContact(0).normal);

        SurfaceType _surfaceType = defaultSurfaceType;

        Surface _surface = collision.transform.GetComponent<Surface>();
        if (_surface) _surfaceType = _surface.SurfaceType;

        ParticleSystem _effect;
        GameObject _decalProjector;

        if (_surfaceType.HitEffect && spawnParticlesOnHit)
        {
            Instantiate(_surfaceType.HitEffect, _hitPosition, _effectRotation);
        }

        if (_surfaceType.HitDecalProjector && spawnDecailsOnHit)
        {
            // Should be parented by the object bullet hit
            _decalProjector = Instantiate(_surfaceType.HitDecalProjector.gameObject, _hitPosition, _decalRotation);
        }

        OnHit(collision);

        switch (destroyBehaviour)
        {
            case DestroyBehaviour.DontDestroy:
                break;
            case DestroyBehaviour.OnHit:
                Destroy(this.gameObject);
                break;
            case DestroyBehaviour.OnDamageSet:
                if(isDamageSet) Destroy(this.gameObject);
                break;
            default:
                break;
        }

    }

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Hitbox = GetComponent<Collider>();
    }

    protected virtual void Start()
    {
        activationTime = Random.Range(enableVisualsDelayMin, enableVisualsDelayMax);
        spawnTime = Time.time;
        visuals.SetActive(false);
    }

    protected virtual void Update()
    {
        if (!visuals.activeSelf && Time.time >= spawnTime + activationTime) visuals.SetActive(true);
    }

    protected virtual void OnHit(Collision collision) { }

    private enum DestroyBehaviour
    {
        DontDestroy,
        OnHit,
        OnDamageSet
    }

}