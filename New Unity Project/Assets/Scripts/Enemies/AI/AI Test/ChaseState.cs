using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{

    private TestEnemy baseEnemy;

    public ChaseState(TestEnemy target) : base(target.gameObject)
    {
        baseEnemy = target;
    }

    public override Type Tick()
    {
        Debug.Log("Chase");
        baseEnemy.Agent.isStopped = false;
        Player player = GameObject.FindObjectOfType<Player>();
        if (player)
        {
            float f = Vector3.Distance(baseEnemy.transform.position, player.transform.position);
            if (f > 10)
            {
                if (f > baseEnemy.DetectRadius)
                {
                    return typeof(WanderState);
                }
                else
                {
                    baseEnemy.Agent.SetDestination(player.transform.position);
                }
            }
            else
            {              
                return typeof(AttackState);
            }
        }

        return typeof(WanderState);
    }

}
