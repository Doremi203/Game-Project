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
        ProjectileBase _bullet = Instantiate(shurikenPrefab, owner.eyesPosition, Quaternion.identity);
        _bullet.Setup(owner, owner.transform.forward, throwPower, 10000f, null);
    }

    protected override bool CanUse()
    {
        return base.CanUse() && currentShurikensAmount > 0;
    }

}