using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class test : MonoBehaviour
{

    [SerializeField] private Transform target;

    private NavMeshAgent agent;
    
    private void Awake() => agent = this.GetComponent<NavMeshAgent>();

    private void Update() => agent.SetDestination(target.position);

}