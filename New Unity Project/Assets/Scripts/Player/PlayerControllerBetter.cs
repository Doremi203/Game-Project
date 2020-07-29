using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBetter : MonoBehaviour
{
	private Vector3 movementDir;
    public float moveSpeed = 3.0f;
    public float gravity = 9.81f;
    public Animator animator;
    private CharacterController playerController;
    private Vector3 lastMoveDir;
    private bool isIdle;

    void Start () {
        playerController = gameObject.GetComponent<CharacterController>();
    }
	
    void Update () {
		MovementHandler();
		CameraHandler();
		UpdateAnimator();
    }

    private void MovementHandler()
    {
	    movementDir.z = Input.GetAxisRaw("Vertical");
	    
	    movementDir.x = Input.GetAxisRaw("Horizontal");
	    
	    movementDir = Vector3.Normalize(movementDir);
	    
	    movementDir.y -= gravity * Time.deltaTime;

	    isIdle = movementDir.x < 0.0001 && movementDir.z < 0.0001;
	    
	    playerController.Move(movementDir * (moveSpeed * Time.deltaTime)); 
	    
    }

    private void CameraHandler()
    {
	    Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
	    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

	    if(groundPlane.Raycast(cameraRay, out var rayLenght))
	    {
		    Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
		    //Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
		    transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
	    }
    }

    private void UpdateAnimator()
    {
	    Vector3 localMove =transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical")));
	    animator.SetFloat("Horizontal", localMove.x);
	    animator.SetFloat("Vertical",  localMove.z);
    }
}
