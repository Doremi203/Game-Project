using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class StateMachineDebugger : MonoBehaviour
{

    [SerializeField] private NPC_BaseAI ai;
    private TextMesh textMesh;

    private void Awake() => textMesh = this.GetComponent<TextMesh>();

    private void Update() => textMesh.text = ai.StateMachine.GetCurrentState().ToString();

}