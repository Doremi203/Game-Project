using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : ProjectileBase
{

    [SerializeField] private Collider trigger;

    protected override void Start()
    {
        base.Start();
        trigger.enabled = false;
    }

    protected override void OnHit(Collision collision)
    {
        transform.SetParent(collision.transform);
        Rigidbody.isKinematic = true;
        trigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        AbilityShurikens abilityShurikens = other.GetComponent<AbilityShurikens>();
        if (!abilityShurikens) return;
        abilityShurikens.AddShurikens(1);
        Destroy(this.gameObject);
    }

}