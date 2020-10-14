using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float animationSpeedMultiplier = 0.5f;

    private Vector3 localMove;

    private void Update()
    {
        localMove = Vector3.Lerp(localMove, transform.InverseTransformDirection(characterController.velocity), 25f * Time.deltaTime);
        animator.SetFloat("Horizontal", localMove.x);
        animator.SetFloat("Vertical", localMove.z);
        animator.SetFloat("Speed", characterController.velocity.magnitude * animationSpeedMultiplier);
    }

}