using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Vector3 movementDir;
	public float moveSpeed = 7.0f;
	public float gravity = 9.81f;
	private CharacterController playerController;
	private Vector3 lastMoveDir;
	private bool isIdle;
	private InputManager inputManager;
	[SerializeField] private Player player;
    private void Awake()
    {
	    //inputManager = gameObject.AddComponent<InputManager>();
        playerController = gameObject.GetComponent<CharacterController>();
    }
    
    
    void Update () {
	    
	    CameraHandler();

	    // if (Input.anyKeyDown)
	    // {
		   //  var key = Input.inputString;
		   //  var action = GetActionByKey(key);
		   //  action?.DoAction();
	    // }
	    //Debug.Log("1");
		    /*foreach (var kvp in player.abilities)
		    {
			    var axises = kvp.Value.axises;
			    if (axises != null)
			    {
				    bool down = false;
				    foreach (var axis in axises)
				    {
					    if (Input.GetAxisRaw(axis) == 1)
					    {
						    kvp.Value.Cast();
						    down = true;
						    break;
					    }
				    }
				    if (down)
					    break;
			    }
		    }*/
	    
	    
	    if (Input.anyKey)
	    {
		    //Debug.Log("0");
		    MovementHandler();
	    }
	    
    }

    // private IAction GetActionByKey(string key)
    // {
	   //  throw new NotImplementedException();
    // }

    private void MovementHandler()
    {
	    movementDir.z = Input.GetAxisRaw("Vertical");
	    
	    movementDir.x = Input.GetAxisRaw("Horizontal");
	    
	    movementDir = Vector3.Normalize(movementDir);
	    
	    movementDir.y -= gravity * Time.deltaTime;

	    isIdle = movementDir.x == 0 && movementDir.z == 0;
	    
	    playerController.Move(movementDir * (moveSpeed * Time.deltaTime)); 
	    
    }

    private void CameraHandler()
    {
	    //bool isMouseX = Input.GetAxisRaw("Mouse X") > 0;
	    //bool isMouseY = Input.GetAxisRaw("Mouse Y") > 0;
	    //if (isMouseX || isMouseY)
	    //{
		    Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

		    if (groundPlane.Raycast(cameraRay, out var rayLenght))
		    {
			    Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
			    //Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
			    transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
		    }
	    //}
    }
}
