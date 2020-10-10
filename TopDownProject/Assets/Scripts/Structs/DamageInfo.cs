using UnityEngine;

public struct DamageInfo
{

    public DamageInfo(Actor instigator, GameObject causer, Vector3 direction, float damageAmount, DamageType damageType)
    {
        this.Instigator = instigator;
        this.Causer = causer;
        this.Direction = direction;
        this.DamageAmount = damageAmount;
        this.DamageType = damageType;
    }

    public Actor Instigator;
    public GameObject Causer;
    public Vector3 Direction;
    public float DamageAmount;
    public DamageType DamageType;

}