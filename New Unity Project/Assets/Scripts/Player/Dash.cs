using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : Ability
{
    [SerializeField] private float dashForce = 5f;
    private CharacterController characterController;
    private Vector3 dashDir;

    void Awake()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }
    
    protected override void DoCast()
    {

        dashDir = new Vector3(Input.GetAxisRaw("Horizontal") * dashForce, 0,
                Input.GetAxisRaw("Vertical") * dashForce);
        //dashDir = transform.TransformDirection(dashDir);
        //DashDirection *= dashSpeed * Time.deltaTime;
        characterController.Move(dashDir);
    }
    
    public override float coolDown => 2f;
    
    public override string[] axises => new []{"Jump"};
    //protected override Sprite skillImage =>
}

