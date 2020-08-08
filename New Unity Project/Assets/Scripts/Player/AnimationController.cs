using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Player player;

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
        Vector3 localMove = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal")));
	    animator.SetFloat("Horizontal", localMove.z);
	    animator.SetFloat("Vertical", localMove.x);
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
