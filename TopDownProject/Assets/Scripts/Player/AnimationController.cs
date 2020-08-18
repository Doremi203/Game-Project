using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Player player;
    [SerializeField] private CharacterController characterController;

    //private CharacterController characterController;
    private Vector3 currentVelocity;

    public void Death()
    {
        animator.SetTrigger("Death");
    }

    private void Awake()
    {
       // characterController = this.GetComponent<CharacterController>();
    }

    private void Start()
    {
        player.weaponHolder.OnWeaponChanged.AddListener(OnWeaponUpdated);
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
    
    private void OnWeaponUpdated()
    {
        animator.SetBool("armed", player.weaponHolder.currentWeapon);
        if (player.weaponHolder.currentWeapon)
            animator.SetInteger("weaponType", (int)player.weaponHolder.currentWeapon.AnimationType);
    }

}
