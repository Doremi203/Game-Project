using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	[SerializeField] private float moveSpeed = 4.75f;
	[SerializeField] private float gravity = 9.81f;

	private CharacterController characterController;
	private Vector3 velocityVector;

    public void SetVelocity(Vector3 velocityVector) => this.velocityVector = velocityVector;

    private void Awake() => characterController = gameObject.GetComponent<CharacterController>();

    private void Update()
	{
		velocityVector.y = -gravity * 10f * Time.deltaTime;
		characterController.Move(velocityVector * moveSpeed * Time.deltaTime);
	}

}