using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(TextMesh))]
public class NPCVelocityDebugger : MonoBehaviour
{

    [SerializeField] private NavMeshAgent ai;
    private TextMesh textMesh;

    private void Awake() => textMesh = this.GetComponent<TextMesh>();

    private void Update() => textMesh.text = ai.velocity.ToString();

}