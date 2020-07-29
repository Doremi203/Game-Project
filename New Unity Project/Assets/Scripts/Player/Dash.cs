using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    [SerializeField] private float dashForce = 5f;
    private CharacterController characterController;
    private Vector3 dashDir;

    public float currentCoolDown;
    //private bool dashed;
    void Awake()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    protected override void DoCast()
    {
        dashDir = new Vector3(Input.GetAxis("Horizontal") * dashForce, 0, Input.GetAxis("Vertical") * dashForce);
            //dashDir = transform.TransformDirection(dashDir);
            //DashDirection *= dashSpeed * Time.deltaTime;
        characterController.Move(dashDir);
            //dashed = true;
    }
    
    public override float coolDown => 2f;
}

