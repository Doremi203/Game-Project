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
        Vector3 moveDirection = agent.velocity;
        moveDirection = transform.InverseTransformDirection(moveDirection);
        animator.SetFloat("Horizontal", moveDirection.x);
        animator.SetFloat("Vertical", moveDirection.z);
        animator.SetFloat("Speed", agent.velocity.magnitude / 2f);
    }

}