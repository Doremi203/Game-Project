using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public abstract class BaseAI : MonoBehaviour
{

    protected Actor npc;
    protected StateMachine stateMachine = new StateMachine();

    protected virtual void Awake()
    {
        npc = GetComponent<Actor>();
    }

    protected virtual void Start()
    {
        npc.HealthComponent.Died += (DamageInfo info) => Destroy(this);
    }

    protected virtual void Update()
    {
        stateMachine.Tick();
    }


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

#endif

}