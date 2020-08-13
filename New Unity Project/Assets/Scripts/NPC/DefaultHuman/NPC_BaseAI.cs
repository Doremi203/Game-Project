using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public abstract class NPC_BaseAI : MonoBehaviour, ISoundsListener
{

    [HideInInspector] public Actor Target;
    [HideInInspector] public Vector3 TargetLastKnownPosition;
    [HideInInspector] public Vector3 LastSoundEventPosition;
    [HideInInspector] public float LastSoundEventExpireTime;

    public float VisionAngle => visionAngle;
    public float AbsoluteVisionRange => absoluteVisionRange;
    public float VisionRange => visionRange;
    public float TargetLostRange => targetLostRange;
    public float AttackRange => attackRange;

    [Header("Vision Raycast")]
    [SerializeField] private LayerMask detectionMask;
    [Header("BaseNPC")]
    [SerializeField] private float visionAngle;
    [SerializeField] private float visionRange;
    [SerializeField] private float absoluteVisionRange;
    [SerializeField] private float targetLostRange;
    [SerializeField] private float attackRange;

    protected StateMachine stateMachine = new StateMachine();
    protected Actor npc;

    private float positionRecordEndsTime;

    public void Test(Actor causer, Vector3 eventPosition)
    {
        if (causer == this) return;
        if (causer.Team == npc.Team)
        {
            // Костыль, ну а хули поделаешь?
            NPC_BaseAI causerAI = causer.GetComponent<NPC_BaseAI>();
            if (causerAI == false) return;
            if (causerAI.Target == false) return;
            TargetLastKnownPosition = causerAI.Target.transform.position;
            LastSoundEventExpireTime = Time.time + 2f;
            return;
        }
        LastSoundEventPosition = eventPosition;
        TargetLastKnownPosition = eventPosition;
        LastSoundEventExpireTime = Time.time + 2f;
    }

    protected virtual void Awake()
    {
        npc = this.GetComponent<Actor>();
    }
    
    protected virtual void Update()
    {
        stateMachine.Tick();
        if (CanSee(Target)) positionRecordEndsTime = Time.time + 2f;
        if (Target != null && Time.time <= positionRecordEndsTime)
        {
            TargetLastKnownPosition = Target.transform.position;
        }
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

                    if (CanSee(actor) == false) continue;

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

    public float GetAngleToTarget()
    {
        Vector3 targetDir = Target.transform.position - npc.transform.position;
        float angle = Vector3.Angle(targetDir, npc.transform.forward);
        return angle;
    }

    public virtual bool CanSee(Actor targetActor)
    {
        if (targetActor == null) return false;
        if (targetActor.IsDead) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, targetActor.transform.position);

        if (distanceToPlayer > absoluteVisionRange)
        {
            // Сравниваем углы.
            Vector3 targetDir = targetActor.transform.position - npc.transform.position;
            float angle = Vector3.Angle(targetDir, npc.transform.forward);

            if (angle > visionAngle) return false;
        }

        Ray ray = new Ray(npc.eyesPosition, (targetActor.transform.position - npc.eyesPosition).normalized * distanceToPlayer);

        RaycastHit hit;

        if (Physics.Linecast(transform.position, targetActor.transform.position, out hit, detectionMask))
        {
            if (hit.transform == targetActor.transform) return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (stateMachine == null) return;

        IState currentState = stateMachine.GetCurrentState();

        if (currentState == null) return;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontSize = 25;

        string targetText = default;
        if (Target != null) targetText = Target.ToString();

        Handles.Label(transform.position, currentState.ToString() + "\n" + targetText, style);
    }

    private void OnDrawGizmosSelected()
    {

        Handles.DrawWireCube(TargetLastKnownPosition, new Vector3(0.5f, 0.1f, 0.5f));

        Handles.color = Color.red;
        Handles.DrawWireDisc(this.transform.position, this.transform.up, attackRange);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(this.transform.position, this.transform.up, visionRange);
        Handles.DrawWireDisc(this.transform.position, this.transform.up, absoluteVisionRange);
        Handles.color = Color.green;
        Handles.DrawWireDisc(this.transform.position, this.transform.up, targetLostRange);
        if (npc == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(npc.transform.position, npc.transform.position + Quaternion.AngleAxis(-visionAngle, Vector3.up) * npc.transform.forward * visionRange);
        Gizmos.DrawLine(npc.transform.position, npc.transform.position + Quaternion.AngleAxis(visionAngle, Vector3.up) * npc.transform.forward * visionRange);
    }

}