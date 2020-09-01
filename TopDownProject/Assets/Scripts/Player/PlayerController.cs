using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public CharacterController CharacterController => playerController;

	[SerializeField] private float moveSpeed = 7.0f;
	[SerializeField] private float gravity = 9.81f;

	private CharacterController playerController;
	private Vector3 currentVelocity;
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
		currentVelocity = Vector3.Lerp(currentVelocity, velocityVector, 25f * Time.deltaTime);
		MovementHandler();
	}

    private void MovementHandler()
    {
	    playerController.Move(currentVelocity * moveSpeed * Time.deltaTime); 	    
    }
}