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

    private float activationTime;

    protected float damage;
    protected DamageType damageType;
    protected Team team;
    protected float spawnTime;
    protected Actor owner;

    public void Setup(Actor owner, float damage, DamageType damageType)
    {
        this.owner = owner;
        this.team = owner.Team;
        this.damage = damage;
        this.damageType = damageType;

        Physics.IgnoreCollision(Hitbox, owner.Hitbox);

        Destroy(this.gameObject, 2f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Actor _actor = collision.transform.GetComponent<Actor>();
        if (_actor)
        {
            if (_actor.Team != team) _actor.ApplyDamage(owner, damage, damageType, transform.forward);
        }
        else
        {
            IDamageable damageable = collision.transform.GetComponent<IDamageable>();
            damageable?.ApplyDamage(owner, damage, damageType, transform.forward);
        }

        Vector3 _hitPosition = collision.GetContact(0).point;
        Quaternion _effectRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);
        Quaternion _decalRotation = Quaternion.LookRotation(-collision.GetContact(0).normal);

        SurfaceType _surfaceType = defaultSurfaceType;

        Surface _surface = collision.transform.GetComponent<Surface>();
        if (_surface) _surfaceType = _surface.SurfaceType;

        ParticleSystem _effect;
        GameObject _decalProjector;

        if (_surfaceType.HitEffect)
        {
            _effect = Instantiate(_surfaceType.HitEffect, _hitPosition, _effectRotation);
            Destroy(_effect.gameObject, 1f);
        }

        if (_surfaceType.HitDecalProjector)
        {
            _decalProjector = Instantiate(_surfaceType.HitDecalProjector.gameObject, _hitPosition, _decalRotation);
            Destroy(_decalProjector.gameObject, 300f);
        }

        Destroy(this.gameObject);
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

}