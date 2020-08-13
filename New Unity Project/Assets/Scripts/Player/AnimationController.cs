using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Player player;

    private CharacterController characterController;
    private Vector3 currentVelocity;

    public void Death()
    {
        animator.SetTrigger("Death");
    }

    private void Awake()
    {
        characterController = this.GetComponent<CharacterController>();
    }

    private void Start()
    {
	    //player.abilities[typeof(Dash)].Casted.AddListener(DoDashAnimation);
	    //player.abilities[typeof(Katana)].Casted.AddListener(DoKatanaAnimation);
    }

    private void Update()
    {
	    UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        currentVelocity = transform.InverseTransformDirection(characterController.velocity);
        Vector3 localMove = currentVelocity;
        animator.SetFloat("Horizontal", localMove.x);
	    animator.SetFloat("Vertical", localMove.z);
        animator.SetFloat("Speed", 2f);
    }
    
    private void DoDashAnimation()
    {
	    animator.SetTrigger("Dash-Use");
    }
    
    private void DoKatanaAnimation()
        {
	        animator.SetTrigger("Katana-Use");
        }
}
