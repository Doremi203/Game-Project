using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityShurikens : AbilityBase
{

    [SerializeField] private int startShurikensAmount;
    [SerializeField] private float throwPower = 700f;
    [SerializeField] private ProjectileBase shurikenPrefab;

    private int currentShurikensAmount;

    protected override void Awake()
    {
        base.Awake();
        currentShurikensAmount = startShurikensAmount;
    }

    protected override void OnUse()
    {
        currentShurikensAmount--;
        Vector3 _force = owner.transform.forward * throwPower;
        ProjectileBase _bullet = Instantiate(shurikenPrefab, owner.eyesPosition, Quaternion.identity);
        _bullet.Setup(owner, 10000f, null);
        _bullet.Rigidbody.AddForce(_force);
    }

    protected override bool CanUse()
    {
        return base.CanUse() && currentShurikensAmount > 0;
    }

}