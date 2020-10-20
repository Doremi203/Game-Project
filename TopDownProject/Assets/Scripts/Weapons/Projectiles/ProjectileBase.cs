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
    [SerializeField] private bool isAbleToPenetrate;
    [SerializeField] private float maxPenetrationDistance;
    [SerializeField] private LayerMask penetrationLayerMask;
    [SerializeField] private LayerMask penetrationHitLayerMask;
    [SerializeField] private LineRenderer penetrationEffect;
    [SerializeField] private bool destroyOnHit = true;
    [SerializeField] private bool destroyByTime = true;

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

        Physics.IgnoreCollision(Hitbox, owner.Hitbox);

        if (destroyByTime) Destroy(this.gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DamageInfo info = new DamageInfo(owner, this.gameObject, this.transform.forward, damage, damageType);

        Actor _actor = collision.transform.GetComponent<Actor>();

        if (_actor)
        {
            if (_actor.Team != team) _actor.ApplyDamage(info);
        }
        else
            collision.transform.GetComponent<IDamageable>()?.ApplyDamage(info);

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

        if (isAbleToPenetrate && false)
        {
            Ray firstRay = new Ray(transform.position + transform.forward * 0.1f, this.transform.forward);
            RaycastHit firstHit;
            float distance = maxPenetrationDistance;
            if (Physics.Raycast(firstRay, out firstHit, maxPenetrationDistance, penetrationLayerMask))
            {
                distance = firstHit.distance;
            }

            Ray ray = new Ray(this.transform.position + transform.forward * distance, -transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxPenetrationDistance, penetrationLayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 4f);
                Ray finalRay = new Ray(hit.point, transform.forward);
                RaycastHit finalHit;
                if (Physics.Raycast(finalRay, out finalHit, hit.distance, penetrationHitLayerMask))
                {
                    Actor actor = finalHit.transform.GetComponent<Actor>();
                    if (actor)
                    {
                        if (actor.Team != team) actor.ApplyDamage(info);
                    }
                    else
                        finalHit.transform.GetComponent<IDamageable>()?.ApplyDamage(info);
                }
            }
        }

        OnHit(collision);

        if(destroyOnHit) Destroy(this.gameObject);
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

}