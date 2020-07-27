using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletBase : MonoBehaviour
{

    protected Rigidbody rb;
    protected WeaponBase owner;

    public void Setup(WeaponBase owner, float pushForce)
    {
        this.owner = owner;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * pushForce);
        Destroy(this.gameObject, 2f);
    }

}