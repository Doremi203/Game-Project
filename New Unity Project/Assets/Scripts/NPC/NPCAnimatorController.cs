using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCAnimatorController : MonoBehaviour
{

    [SerializeField] private Animator animator;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Vector3 moveDirection = agent.velocity;
        moveDirection = transform.TransformDirection(moveDirection);
        animator.SetFloat("Horizontal", moveDirection.x);
        animator.SetFloat("Vertical", moveDirection.z);
        animator.SetFloat("Speed", agent.velocity.magnitude / 2f);
    }

}