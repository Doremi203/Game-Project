using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(WeaponHolder))]
public abstract class BaseEnemy : Actor
{

    [SerializeField] private WeaponBase weaponPrefab;
    [SerializeField] private float detectRadius;
    public float DetectRadius => detectRadius;

    [SerializeField] private LayerMask playerDetectionMask;
    [SerializeField] private float attackRadius;
    public float AttackRadius => attackRadius;

    protected WeaponHolder weaponHolder;
    public WeaponHolder WeaponHolder => weaponHolder;

    protected StateMachine stateMachine;

    public bool CanSeeThePlayer()
    {
        Player player = GameObject.FindObjectOfType<Player>();
        if (player == null) return false;

        RaycastHit hit;
        if (Physics.Linecast(transform.position, player.transform.position,  out hit, playerDetectionMask))
        {
            if (hit.transform.GetComponent<Player>())
            {
                Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * hit.distance, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * hit.distance, Color.red);
            }
        }

        return false;
    }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = GetComponent<StateMachine>();
        weaponHolder = GetComponent<WeaponHolder>();
        weaponHolder.EquipWeapon(Instantiate(weaponPrefab, this.transform));
        StartStateMachine();
    }

    protected abstract void StartStateMachine();

}