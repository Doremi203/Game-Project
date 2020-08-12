using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	[SerializeField] private float moveSpeed = 7.0f;
	[SerializeField] private float gravity = 9.81f;

	private CharacterController playerController;
	private Vector3 velocityVector;

	public void SetVelocity(Vector3 velocityVector)
    {
		this.velocityVector = velocityVector;
	}

	private void Awake()
    {
        playerController = gameObject.GetComponent<CharacterController>();
    }

	private void Update()
	{
		//CameraHandler();
		MovementHandler();
	}

    private void MovementHandler()
    {
	    playerController.Move(velocityVector * moveSpeed * Time.deltaTime); 	    
    }

	private void CameraHandler()
	{
		Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

		if (groundPlane.Raycast(cameraRay, out var rayLenght))
		{
			Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
			//Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
			transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
		}
	}

}