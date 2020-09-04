using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBase : MonoBehaviour
{

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
        rb = GetComponent<Rigidbody>();
        rb.AddForce(pushDirection * pushForce);
        Destroy(this.gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<ProjectileBase>()) return;

        Actor _otherActor = other.transform.GetComponent<Actor>();
        //if (_otherActor == owner) return;
        if (_otherActor?.Team == team) return;

        IDamageable damageable = other.transform.GetComponent<IDamageable>();
        damageable?.ApplyDamage(owner, damage, damageType);

        OnHit(other);

        /*
        GameObject _gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _gameObject.transform.position = this.transform.position;
        _gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        */

        Destroy(this.gameObject);
    }

    protected virtual void Awake()
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