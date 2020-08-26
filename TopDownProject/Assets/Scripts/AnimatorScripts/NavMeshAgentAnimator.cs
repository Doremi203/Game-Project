using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;

    private void Update()
    {
        Vector3 moveDirection = transform.InverseTransformDirection(agent.velocity);
        animator.SetFloat("Horizontal", moveDirection.x);
        animator.SetFloat("Vertical", moveDirection.z);
        animator.SetFloat("Speed", 2.5f);
    }

}