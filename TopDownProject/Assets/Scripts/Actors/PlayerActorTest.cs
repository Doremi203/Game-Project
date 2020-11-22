using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActorTest : Actor
{

    [SerializeField] private bool hasArmor;

    public override bool ApplyDamage(DamageInfo info)
    {
        if (info.DamageType == DamageType.Bullet && hasArmor)
            info.DamageAmount = info.DamageAmount / 2;
        return base.ApplyDamage(info);
    }

}