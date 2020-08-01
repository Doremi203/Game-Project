using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{

    void ApplyDamage(Actor damageCauser, float damage, DamageType damageType);

}