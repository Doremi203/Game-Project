using UnityEditor;
using UnityEngine;
using AdvancedAI;

[RequireComponent(typeof(Actor))]
public abstract class NPC_BaseAI : MonoBehaviour
{

    public float VisionAngle => visionAngle;
    public float AbsoluteVisionRange => absoluteVisionRange;
    public float VisionRange => visionRange;
    public float TargetLostRange => targetLostRange;

    [Header("Vision Raycast")]
    [SerializeField] private LayerMask detectionMask;
    [Header("BaseNPC")]
    [SerializeField] private float visionAngle;
    [SerializeField] private float visionRange;
    [SerializeField] private float absoluteVisionRange;
    [SerializeField] private float targetLostRange;

    protected Actor npc;
    protected StateMachine stateMachine = new StateMachine();

    protected virtual void Awake()
    {
        npc = GetComponent<Actor>();
        npc.DeathEvent.AddListener(NpcDeath);
    }

    protected virtual void Update() => stateMachine.Tick();

    public float DistanceToPlayer()
    {
        return GameUtilities.GetDistance2D(npc.transform.position, Player.Instance.transform.position);
    }

    public float AngleToPlayer()
    {
        Vector3 _targetDirection = Player.Instance.transform.position - npc.transform.position;
        return Vector3.Angle(_targetDirection, npc.transform.forward);
    }

    public bool CanSeePlayer()
    {
        Player player = Player.Instance;
        float playerDistance = DistanceToPlayer();

        if (playerDistance > visionRange) return false;

        if (playerDistance > absoluteVisionRange)
        {
            if (AngleToPlayer() > visionAngle) return false;
        }

        return !Physics.Linecast(npc.eyesPosition, player.Actor.eyesPosition, detectionMask);
    }

    private void NpcDeath() => Destroy(this);

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (stateMachine == null) return;

        IState currentState = stateMachine.CurrentState;

        if (currentState == null) return;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontSize = 25;

        Handles.Label(transform.position, currentState.ToString(), style);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        //Handles.DrawWireDisc(this.transform.position, this.transform.up, attackRange);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(this.transform.position, this.transform.up, visionRange);
        Handles.DrawWireDisc(this.transform.position, this.transform.up, absoluteVisionRange);
        Handles.color = Color.green;
        Handles.DrawWireDisc(this.transform.position, this.transform.up, targetLostRange);
        Actor _npc = this.GetComponent<Actor>();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_npc.transform.position, _npc.transform.position + Quaternion.AngleAxis(-visionAngle, Vector3.up) * _npc.transform.forward * visionRange);
        Gizmos.DrawLine(_npc.transform.position, _npc.transform.position + Quaternion.AngleAxis(visionAngle, Vector3.up) * _npc.transform.forward * visionRange);
    }

#endif

}