using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DestinationDebug : MonoBehaviour
{

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (agent == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(agent.destination, agent.destination + Vector3.up * 5f);
    }

#endif

}