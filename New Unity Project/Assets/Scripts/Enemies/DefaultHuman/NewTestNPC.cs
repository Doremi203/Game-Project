using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NewTestNPC : Actor
{

    public Quaternion desiredRotation;

    [SerializeField] private LayerMask playerDetectionMask;
    [SerializeField] private WeaponBase weaponPrefab;
    [SerializeField] private TextMesh text;

    [SerializeField] private float agroRange;
    public float AgroRange => agroRange;
    [SerializeField] private float deagroRange;
    public float DeagroRange => deagroRange;
    [SerializeField] private float attackRange;
    public float AttackRange => attackRange;

    private NavMeshAgent agent;
    private NewStateMachine stateMachine;
    protected WeaponHolder weaponHolder;

    protected Actor target;
    public Actor Target => target;

    public void SetTarget(Actor target)
    {
        this.target = target;
    }

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        weaponHolder = GetComponent<WeaponHolder>();

        weaponHolder.EquipWeapon(Instantiate(weaponPrefab, this.transform));

        stateMachine = new NewStateMachine();

        // States
        var roaming = new Roaming(this, agent);
        var chasing = new Chasing(this, agent);
        var attacking = new Attacking(this, agent, weaponHolder);

        // Transitions
        At(roaming, chasing, inAgroRange());
        At(chasing, roaming, isPlayerFarAway());
        At(chasing, attacking, canShootPlayer());
        At(attacking, chasing, cantShootPlayer());
        At(attacking, chasing, leftShootingRange());

        stateMachine.AddAnyTransition(roaming, hasNoTarget());

        stateMachine.SetState(roaming);

        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

        Func<bool> inAgroRange() => () => DistanceToThePlayer() <= agroRange;
        Func<bool> isPlayerFarAway() => () => DistanceToThePlayer() > deagroRange;
        Func<bool> canShootPlayer() => () => CanShootThePlayer();
        Func<bool> cantShootPlayer() => () => !CanShootThePlayer();
        Func<bool> leftShootingRange() => () => DistanceToThePlayer() > attackRange;

        Func<bool> hasNoTarget() => () => target == null || target.isDead;

    }

    private void Update()
    {
        stateMachine.Tick();
        text.text = stateMachine.GetCurrentState().ToString();

        this.transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 15f * Time.deltaTime);
    }

    public void TryFindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);

        Actor closestActor = null;

        foreach (var item in hits)
        {
            Actor actor = item.GetComponent<Actor>();
            if (actor)
            {
                if (actor.Team != Team && !actor.isDead)
                {
                    if (closestActor == null)
                    {
                        closestActor = actor;
                    }
                    else
                    {
                        float a = Vector3.Distance(transform.position, actor.transform.position);
                        float b = Vector3.Distance(transform.position, closestActor.transform.position);
                        if (a < b) closestActor = actor;
                    }
                }
            }
        }

        target = closestActor;
    }

    private float DistanceToThePlayer()
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    private bool CanShootThePlayer()
    {
        if (target == null) return false;
        if (target == isDead) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToPlayer > attackRange) return false;

        RaycastHit hit;
        Ray ray = new Ray(transform.position, (target.transform.position - transform.position).normalized * distanceToPlayer);
        if (Physics.Linecast(transform.position, target.transform.position, out hit, playerDetectionMask))
        {
            if (hit.transform == target.transform)
            {
                Debug.DrawRay(transform.position, (target.transform.position - transform.position).normalized * hit.distance, Color.green);                
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, (target.transform.position - transform.position).normalized * hit.distance, Color.red);
            }
        }

        return false;
    }


}