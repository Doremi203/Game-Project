using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseNPC))]
public abstract class NPC_BaseAI : MonoBehaviour
{

    [HideInInspector] public Actor Target;

    public float VisionRange => visionRange;
    public float TargetLostRange => targetLostRange;
    public float AttackRange => attackRange;

    [Header("Vision Raycast")]
    [SerializeField] private LayerMask detectionMask;
    [Header("BaseNPC")]
    [SerializeField] private float visionRange;
    [SerializeField] private float targetLostRange;
    [SerializeField] private float attackRange;

    protected StateMachine stateMachine = new StateMachine();
    protected BaseNPC npc;

    protected virtual void Awake()
    {
        npc = this.GetComponent<BaseNPC>();
    }
    
    protected virtual void Update()
    {
        stateMachine.Tick();
    }

    public virtual Actor GetClosestEnemyActor()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, visionRange);

        Actor closestActor = null;

        foreach (var item in hits)
        {
            Actor actor = item.GetComponent<Actor>();
            if (actor)
            {
                if (actor.Team != npc.Team && !actor.IsDead)
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

        return closestActor;
    }

    public virtual float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, Target.transform.position);
    }

    public virtual bool CanSeeTheTarget()
    {
        if (Target == null) return false;
        if (Target.IsDead) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, Target.transform.position);
        if (distanceToPlayer > attackRange) return false;

        RaycastHit hit;
        Ray ray = new Ray(npc.eyesPosition, (Target.transform.position - npc.eyesPosition).normalized * distanceToPlayer);
        if (Physics.Linecast(transform.position, Target.transform.position, out hit, detectionMask))
        {
            if (hit.transform == Target.transform)
            {
                Debug.DrawRay(npc.eyesPosition, (Target.transform.position - npc.eyesPosition).normalized * hit.distance, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(npc.eyesPosition, (Target.transform.position - npc.eyesPosition).normalized * hit.distance, Color.red);
            }
        }

        return false;
    }

}