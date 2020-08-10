using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Dash : Ability
{

    [SerializeField] private float dashForce = 5f;
    [SerializeField] private float cooldown = 2f;

    private CharacterController characterController;
    private Vector3 dashDir;

    private void Awake()
    {
        characterController = this.GetComponent<CharacterController>();
    }
    
    protected override void DoCast()
    {
        dashDir = new Vector3(Input.GetAxisRaw("Horizontal") * dashForce, 0,
                Input.GetAxisRaw("Vertical") * dashForce);
        //dashDir = transform.TransformDirection(dashDir);
        //DashDirection *= dashSpeed * Time.deltaTime;
        characterController.Move(dashDir);
    }
    
    public override float coolDown => cooldown;

    public override string[] axises => new []{"Jump"};

}