using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{

    private TestEnemy baseEnemy;
    private float nextFireTime;
    private float attackRange;

    public AttackState(TestEnemy target) : base(target.gameObject)
    {
        baseEnemy = target;
    }

    public override void OnStateEnter()
    {
        Debug.Log("Attack");
        attackRange = UnityEngine.Random.Range(baseEnemy.AttackRadius - 1f, baseEnemy.AttackRadius + 1f);
    }

    public override Type Tick()
    {
        baseEnemy.Agent.isStopped = true;

        Player player = GameObject.FindObjectOfType<Player>();

        if (!player) return null;

        float f = Vector3.Distance(baseEnemy.transform.position, player.transform.position);

        if (f > baseEnemy.AttackRadius) return typeof(ChaseState);

        if (baseEnemy.CanSeeThePlayer() == false) return typeof(ChaseState);

        baseEnemy.transform.LookAt(player.transform);
        baseEnemy.transform.eulerAngles = new Vector3(0, baseEnemy.transform.eulerAngles.y, 0);

        if (Time.time >= nextFireTime)
        {
            baseEnemy.WeaponHolder.currentWeapon.Use(true);
            nextFireTime = Time.time + 2;
        }
        else
        {
            baseEnemy.WeaponHolder.currentWeapon.Use(false);
        }

        return null;
    }

}