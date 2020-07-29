using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{

    private TestEnemy baseEnemy;
    private float nextFireTime;

    public AttackState(TestEnemy target) : base(target.gameObject)
    {
        baseEnemy = target;
    }

    public override Type Tick()
    {
        Debug.Log("Attack");
        Player player = GameObject.FindObjectOfType<Player>();
        if (!player) return null;

        float f = Vector3.Distance(baseEnemy.transform.position, player.transform.position);

        if (f > 10) return typeof(ChaseState);

        Vector3 pointToLook = (player.transform.position - baseEnemy.transform.position).normalized;
        baseEnemy.transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
        baseEnemy.transform.eulerAngles = new Vector3(0, baseEnemy.transform.eulerAngles.y, 0);


        if (Time.time >= nextFireTime)
        {
            baseEnemy.Agent.isStopped = true;
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