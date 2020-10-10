using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public abstract class NPC_BaseAI : MonoBehaviour, ISoundsListener
{

    [HideInInspector] public Vector3 LastSoundEventPosition;
    [HideInInspector] public float LastSoundEventExpireTime;

    public float VisionAngle => visionAngle;
    public float AbsoluteVisionRange => absoluteVisionRange;
    public float VisionRange => visionRange;
    public float TargetLostRange => targetLostRange;
    public StateMachine StateMachine => stateMachine;

    [Header("Vision Raycast")]
    [SerializeField] private LayerMask detectionMask;
    [Header("BaseNPC")]
    [SerializeField] private float visionAngle;
    [SerializeField] private float visionRange;
    [SerializeField] private float absoluteVisionRange;
    [SerializeField] private float targetLostRange;

    protected StateMachine stateMachine = new StateMachine();
    protected Actor npc;

    public void ApplySoundEvent(Actor causer, Vector3 eventPosition)
    {
        if (causer == this) return;
        if (causer.Team == npc.Team) return;
        LastSoundEventPosition = eventPosition;
        LastSoundEventExpireTime = Time.time + 2f;
    }

    protected virtual void Awake()
    {
        npc = this.GetComponent<Actor>();
        npc.DeathEvent.AddListener(NpcDeath);
    }

    protected virtual void Update() => stateMachine.Tick();

    private void NpcDeath() => this.enabled = false;

    public float DistanceToPlayer()
    {
        Player _player = Player.Instance;
        return GameUtilities.GetDistance2D(npc.transform.position, _player.transform.position);
    }

    public float AngleToPlayer()
    {
        Player _player = Player.Instance;
        Vector3 _targetDirection = _player.transform.position - npc.transform.position;
        return Vector3.Angle(_targetDirection, npc.transform.forward);
    }

    public bool CanSeePlayer()
    {
        Player _player = Player.Instance;
        float _playerDistance = DistanceToPlayer();

        if (_playerDistance > visionRange) return false;

        if (_playerDistance > absoluteVisionRange)
        {
            if (AngleToPlayer() > visionAngle) return false;
        }

        return !Physics.Linecast(npc.eyesPosition, _player.eyesPosition, detectionMask);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (stateMachine == null) return;

        IState currentState = stateMachine.GetCurrentState();

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