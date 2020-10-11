using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityShurikens : AbilityBase
{

    public int CurrentShurikensAmount { get; private set; }

    [SerializeField] private int startShurikensAmount;
    [SerializeField] private float throwPower = 700f;
    [SerializeField] private ProjectileBase shurikenPrefab;

    protected override void Awake()
    {
        base.Awake();
        CurrentShurikensAmount = startShurikensAmount;
    }

    protected override void OnUse()
    {
        CurrentShurikensAmount--;
        Vector3 _force = owner.transform.forward * throwPower;
        ProjectileBase _bullet = Instantiate(shurikenPrefab, owner.eyesPosition, owner.transform.rotation);
        _bullet.Setup(owner, 1000f);
        _bullet.Rigidbody.AddForce(_force);
    }

    protected override bool CanUse() => base.CanUse() && CurrentShurikensAmount > 0;

}