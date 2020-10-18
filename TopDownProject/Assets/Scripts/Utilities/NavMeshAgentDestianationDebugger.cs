using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentDestianationDebugger : MonoBehaviour
{

    private NavMeshAgent agent;

    private void Awake() => agent = this.GetComponent<NavMeshAgent>();

    private void OnDrawGizmos()
    {
        if (agent == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(agent.destination, new Vector3(0.2f, 0.2f, 0.2f));
    }

}