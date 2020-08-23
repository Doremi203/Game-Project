using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;

    private void Update()
    {
        Vector3 localMove = transform.InverseTransformDirection(characterController.velocity).normalized;
        animator.SetFloat("Horizontal", localMove.x);
        animator.SetFloat("Vertical", localMove.z);
        animator.SetFloat("Speed", 2f);
    }

}