using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator animator;

    private CharacterController characterController;
    private Vector3 moveDirection;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {

        Vector3 vector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        vector = transform.TransformDirection(vector);
        animator.SetFloat("Horizontal", vector.x);
        animator.SetFloat("Vertical", vector.z);
        animator.SetFloat("Speed", characterController.velocity.magnitude / 2f);

        // Rotate to the mouse position
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLenght;

        if(groundPlane.Raycast(cameraRay, out rayLenght))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        // Player input
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.z = Input.GetAxisRaw("Vertical");   
        moveDirection = Vector3.Normalize(moveDirection);

        // Gravity
        if (characterController.isGrounded)
        {
            moveDirection.y = 0;
        }
        else
        {
            moveDirection.y -= 2f * Time.deltaTime;
        }

        // Move the player
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

    }

}