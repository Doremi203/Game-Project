using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ablity
{
    [SerializeField] private float dashForce = 5f;
    private CharacterController characterController;
    private Vector3 dashDir;
    private bool dashed;
    private float cooldownTimeDash = 3f;
    void Awake()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CoolDownUpdater();
        if (Input.GetKeyDown(KeyCode.Space) && !dashed)
            StartCoroutine(Cast());
    }

    public override IEnumerator Cast()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dashDir = new Vector3(Input.GetAxis("Horizontal")*dashForce,0, Input.GetAxis("Vertical") * dashForce);
            //dashDir = transform.TransformDirection(dashDir);
            //DashDirection *= dashSpeed * Time.deltaTime;
            characterController.Move(dashDir);
            dashed = true;
            yield return null;
            
        }
    }

    public override void CoolDownUpdater()
    {
        if(dashed)
        {
            cooldownTimeDash -= Time.deltaTime;
            Debug.Log(cooldownTimeDash);
            if (cooldownTimeDash < 0.0001)
            {
                //cooldownTimeDash = 5f;
                dashed = false;
                cooldownTimeDash = 3f; 
                Debug.Log(cooldownTimeDash);
            }
        }
    }
}

