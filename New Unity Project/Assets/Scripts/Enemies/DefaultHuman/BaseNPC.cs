using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNPC : Actor
{

    [HideInInspector] public Actor Target;
    [HideInInspector] public Quaternion desireRotation;

    public float RotationSpeed => rotationSpeed;
    public float VisionRange => visionRange;
    public float TargetLostRange => targetLostRange;
    public float AttackRange => attackRange;

    [Header("Vision Raycast")]
    [SerializeField] private LayerMask detectionMask;
    [Header("BaseNPC")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float visionRange;
    [SerializeField] private float targetLostRange;
    [SerializeField] private float attackRange;

    protected NewStateMachine stateMachine = new NewStateMachine();

    protected virtual void Update()
    {
        stateMachine.Tick();
        this.transform.rotation = Quaternion.Lerp(transform.rotation, desireRotation, 15f * Time.deltaTime);
    }

    public void TryFindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, visionRange);

        Actor closestActor = null;

        foreach (var item in hits)
        {
            Actor actor = item.GetComponent<Actor>();
            if (actor)
            {
                if (actor.Team != Team && !actor.IsDead)
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

        Target = closestActor;
    }

    public float TargetDistance()
    {
        return Vector3.Distance(transform.position, Target.transform.position);
    }

    public bool CanSeeTheTarget()
    {
        if (Target == null) return false;
        if (Target == IsDead) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, Target.transform.position);
        if (distanceToPlayer > attackRange) return false;

        RaycastHit hit;
        Ray ray = new Ray(transform.position, (Target.transform.position - transform.position).normalized * distanceToPlayer);
        if (Physics.Linecast(transform.position, Target.transform.position, out hit, detectionMask))
        {
            if (hit.transform == Target.transform)
            {
                Debug.DrawRay(transform.position, (Target.transform.position - transform.position).normalized * hit.distance, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, (Target.transform.position - transform.position).normalized * hit.distance, Color.red);
            }
        }

        return false;
    }

}