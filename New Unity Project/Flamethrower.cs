using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : WeaponBase
{

    public override bool CanUse()
    {
        return true;
    }

    protected override void OnUsingEnd()
    {
        
    }

    protected override void OnUsingStart()
    {
        
    }

    private void Update()
    {
        if (!isUsing) return;



    }

}