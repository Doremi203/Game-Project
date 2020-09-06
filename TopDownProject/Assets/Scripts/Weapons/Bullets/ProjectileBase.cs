using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ProjectileBase : MonoBehaviour
{

    public Collider Hitbox { get; private set; }

    [SerializeField] private GameObject visuals;
    [SerializeField] private float enableVisualsDelayMin = 0.02f;
    [SerializeField] private float enableVisualsDelayMax = 0.02f;

    private float activationTime;

    protected Rigidbody rb;
    protected float damage;
    protected DamageType damageType;
    protected Team team;
    protected float spawnTime;
    protected Actor owner;

    public void Setup(Actor owner ,Vector3 pushDirection, float pushForce, float damage, DamageType damageType)
    {
        this.damage = damage;
        this.damageType = damageType;
        this.team = owner.Team;
        this.owner = owner;

        Physics.IgnoreCollision(Hitbox, owner.Hitbox);

        rb = GetComponent<Rigidbody>();
        rb.AddForce(pushDirection * pushForce);
        Destroy(this.gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Actor _actor = collision.transform.GetComponent<Actor>();
        if (_actor)
        {
            if (_actor.Team != team) _actor.ApplyDamage(owner, damage, damageType);
        }
        else
        {
            IDamageable damageable = collision.transform.GetComponent<IDamageable>();
            damageable?.ApplyDamage(owner, damage, damageType);
        }

        //OnHit(collision.transform);

        Destroy(this.gameObject);
    }

    protected virtual void Awake() => Hitbox = GetComponent<Collider>();

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

    protected virtual void OnHit(Collider other) { }

}