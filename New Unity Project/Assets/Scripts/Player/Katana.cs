using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : Ability
{
    private float range = 1;
    protected override void DoCast()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + transform.forward * 1f, range,transform.forward);
        foreach (var hit in hits)
        {
            Debug.Log(hit.collider.name);
        }
    }

    public override float coolDown => 1;
    public override string[] axises  => new []{"Fire3"};
}
